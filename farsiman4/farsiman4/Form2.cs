using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace farsiman4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }
        SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True");

       

        private void Form2_Load(object sender, EventArgs e)
        {
            CargarColaborador();
            CargarSucursal();
            this.FormClosed += new FormClosedEventHandler(cerrar);
        }

        private void cerrar(object sender, EventArgs e)
        {
            Principal formprincipal = new Principal();
            this.Hide();
            formprincipal.Show();
        }
        private void CargarColaborador()
        {
            cn.Open();
            string consulta = "select nombre_colaborador from Colaboradores ";
            SqlCommand comando = new SqlCommand(consulta, cn);
            SqlDataReader lector;
            lector = comando.ExecuteReader();

            while (lector.Read())
            {
                comboBox1.Items.Add(lector.GetString(0));
            }
            cn.Close();
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
                comboBox2.Items.Add(lector.GetString(0));
            }
            cn.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Obtener los valores seleccionados del combobox
            string colaboradorSeleccionado = comboBox1.SelectedItem.ToString();
            string sucursalSeleccionada = comboBox2.SelectedItem.ToString();
            decimal distanciaKm = decimal.Parse(textBox1.Text);

            // Validar la distancia
            if (distanciaKm <= 0 || distanciaKm > 50)
            {
                MessageBox.Show("La distancia debe ser mayor que cero y no mayor que 50.");
                return;
            }

            // Guardar los datos en la tabla "Asignaciones"
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True"))
            {
                cn.Open();
                string consulta = "INSERT INTO Asignaciones (id_colaborador, id_sucursal, distancia_km) VALUES ((SELECT id_colaborador FROM Colaboradores WHERE nombre_colaborador = @colaborador), (SELECT id_sucursal FROM Sucursales WHERE nombre_sucursal = @sucursal), @distanciaKm)";
                SqlCommand comando = new SqlCommand(consulta, cn);
                comando.Parameters.AddWithValue("@colaborador", colaboradorSeleccionado);
                comando.Parameters.AddWithValue("@sucursal", sucursalSeleccionada);
                comando.Parameters.AddWithValue("@distanciaKm", distanciaKm);
                comando.ExecuteNonQuery();

                MessageBox.Show("Asignación guardada correctamente.");
            }
        }
    }
}
