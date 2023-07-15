using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace farsiman4
{
    public partial class Form3 : Form
    {

        SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True");
        public string UsuarioLogueado { get; set; }


        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            CargarSucursal();
            CargarTransportista();
            this.FormClosed += new FormClosedEventHandler(cerrar);
        }

        private void cerrar(object sender, EventArgs e)
        {
            Principal formprincipal = new Principal();
            this.Hide();
            formprincipal.Show();
        }


        private void CargarSucursal()
        {
            cn.Open();
            string consulta = "select nombre_sucursal from Sucursales ";
            SqlCommand comando = new SqlCommand(consulta, cn);
            SqlDataReader lector;
            lector = comando.ExecuteReader();


            while (lector.Read())
            {
                comboBox1.Items.Add(lector.GetString(0));
            }
            
            cn.Close();
        }

        private void CargarTransportista()
        {
            cn.Open();
            string consulta = "select nombre_transportista from Transportistas ";
            SqlCommand comando = new SqlCommand(consulta, cn);
            SqlDataReader lector;
            lector = comando.ExecuteReader();


            while (lector.Read())
            {
                comboBox2.Items.Add(lector.GetString(0));
            }

            cn.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sucursalSeleccionada = comboBox1.SelectedItem.ToString();
            CargarColaboradores(sucursalSeleccionada);
        }

        private void CargarColaboradores(string sucursal)
        {
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True"))
            {
                cn.Open();
                string consulta = "SELECT c.nombre_colaborador FROM Colaboradores c " +
                    "INNER JOIN Asignaciones a ON c.id_colaborador = a.id_colaborador " +
                    "INNER JOIN Sucursales s ON a.id_sucursal = s.id_sucursal " +
                    "WHERE s.nombre_sucursal = @sucursal";
                SqlCommand comando = new SqlCommand(consulta, cn);
                comando.Parameters.AddWithValue("@sucursal", sucursal);
                SqlDataReader lector = comando.ExecuteReader();

                checkedListBox1.Items.Clear();
                while (lector.Read())
                {
                    string nombreColaborador = lector.GetString(0);
                    checkedListBox1.Items.Add(nombreColaborador);
                }

                lector.Close();
            }
        }

        private void Registrar_Click(object sender, EventArgs e)
        {
            // Obtener los valores seleccionados del combobox
            string sucursalSeleccionada = comboBox1.SelectedItem.ToString();
            string colaboradorSeleccionado = checkedListBox1.SelectedItem.ToString();
            string transportistaSeleccionado = comboBox2.SelectedItem.ToString();

            // Obtener el usuario que realiza el registro
            string usuarioRegistro = UsuarioLogueado; // Obtén el valor del usuario logueado desde la propiedad UsuarioLogueado de Form3
            cn.Open();                                        // Reemplazar por la lógica adecuada para obtener el usuario actualmente logueado
            string consultaPerfil = "SELECT perfil FROM Usuarios WHERE nombre_usuario = @usuario";
            SqlCommand comandoPerfil = new SqlCommand(consultaPerfil, cn);
            comandoPerfil.Parameters.AddWithValue("@usuario", usuarioRegistro);
            string perfilUsuario = comandoPerfil.ExecuteScalar()?.ToString();

            if (perfilUsuario != "Gerente de tienda")
            {
                MessageBox.Show("No tiene permisos para registrar.");
                return;
            }
           

            // Obtener la fecha actual
            DateTime fechaRegistro = DateTime.Now;

            string consultaVerificacion = @"SELECT COUNT(*) FROM Viajes
                                    WHERE id_colaborador = (SELECT id_colaborador FROM Colaboradores WHERE nombre_colaborador = @colaborador)
                                    AND fecha >= CONVERT(date, @fechaRegistro)";
            SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, cn);
            comandoVerificacion.Parameters.AddWithValue("@colaborador", colaboradorSeleccionado);
            comandoVerificacion.Parameters.AddWithValue("@fechaRegistro", fechaRegistro);

            int viajesRegistrados = Convert.ToInt32(comandoVerificacion.ExecuteScalar());
            if (viajesRegistrados > 0)
            {
                MessageBox.Show("El colaborador ya tiene un viaje registrado para la fecha actual.");
                return;
            }

            // Calcular la distancia total del viaje
            decimal distanciaTotalKm = CalcularDistanciaTotal();

            // Validar la distancia total
            if (distanciaTotalKm <= 0 || distanciaTotalKm > 100)
            {
                MessageBox.Show("La distancia total debe ser mayor que cero y no mayor que 100.");
                return;
            }

            // Guardar los datos en la tabla "Viajes"
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True"))
            {
                cn.Open();
                string consulta = "INSERT INTO Viajes (id_colaborador, id_sucursal, id_transportista, distancia_km, fecha, usuario_registro) " +
                    "VALUES ((SELECT id_colaborador FROM Colaboradores WHERE nombre_colaborador = @colaborador), " +
                    "(SELECT id_sucursal FROM Sucursales WHERE nombre_sucursal = @sucursal), " +
                    "(SELECT id_transportista FROM Transportistas WHERE nombre_transportista = @transportista), " +
                    "@distancia, @fecha, @usuario)";
                SqlCommand comando = new SqlCommand(consulta, cn);
                comando.Parameters.AddWithValue("@colaborador", colaboradorSeleccionado);
                comando.Parameters.AddWithValue("@sucursal", sucursalSeleccionada);
                comando.Parameters.AddWithValue("@transportista", transportistaSeleccionado);
                comando.Parameters.AddWithValue("@distancia", distanciaTotalKm);
                comando.Parameters.AddWithValue("@fecha", fechaRegistro);
                comando.Parameters.AddWithValue("@usuario", usuarioRegistro);
                comando.ExecuteNonQuery();

                MessageBox.Show("Viaje registrado correctamente.");
            }
        }

        private decimal CalcularDistanciaTotal()
        {
            decimal distanciaTotalKm = 0;

            // Obtener los colaboradores seleccionados del checklist
            List<string> colaboradoresSeleccionados = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                string colaborador = item.ToString();
                colaboradoresSeleccionados.Add(colaborador);
            }

            // Consultar la tabla "Asignaciones" y sumar las distancias correspondientes a los colaboradores seleccionados
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True"))
            {
                cn.Open();
                foreach (string colaborador in colaboradoresSeleccionados)
                {
                    string sucursalSeleccionada = comboBox1.SelectedItem.ToString();
                    string consulta = "SELECT distancia_km FROM Asignaciones WHERE id_colaborador = (SELECT id_colaborador FROM Colaboradores WHERE nombre_colaborador = @colaborador) AND id_sucursal = (SELECT id_sucursal FROM Sucursales WHERE nombre_sucursal = @sucursal)";

                    SqlCommand comando = new SqlCommand(consulta, cn);
                    comando.Parameters.AddWithValue("@colaborador", colaborador);
                    comando.Parameters.AddWithValue("@sucursal", sucursalSeleccionada);

                    SqlDataReader lector = comando.ExecuteReader();

                    if (lector.Read())
                    {
                        decimal distanciaColaborador = lector.GetDecimal(0);
                        distanciaTotalKm += distanciaColaborador;
                    }

                    lector.Close();
                }
            }

            return distanciaTotalKm;
        }
    }
}
