using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace farsiman4
{
    public partial class Form4 : Form
    {

        SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True");
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            CargarTransportista();
            this.FormClosed += new FormClosedEventHandler(cerrar);
        }

        private void cerrar(object sender, EventArgs e)
        {
            Principal formprincipal = new Principal();
            this.Hide();
            formprincipal.Show();
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
                comboBox1.Items.Add(lector.GetString(0));
            }
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Obtener las fechas seleccionadas
            DateTime fechaInicio = dateTimePicker1.Value;
            DateTime fechaFin = dateTimePicker2.Value;

            // Obtener el transportista seleccionado
            string transportista = comboBox1.SelectedItem.ToString();

            cn.Open();

            // Construir la consulta SQL con parámetros
            string consulta = @"SELECT v.id_viaje, v.fecha, t.nombre_transportista, s.nombre_sucursal, v.usuario_registro, v.distancia_km, t.tarifa_km, v.distancia_km * t.tarifa_km AS total_pagar
                    FROM Viajes v
                    INNER JOIN Transportistas t ON v.id_transportista = t.id_transportista
                    INNER JOIN Colaboradores c ON v.id_colaborador = c.id_colaborador
                    INNER JOIN Sucursales s ON v.id_sucursal = s.id_sucursal
                    WHERE v.fecha BETWEEN @fechaInicio AND @fechaFin
                    AND t.nombre_transportista = @transportista";



            // Crear el objeto SqlCommand con la consulta y los parámetros
            SqlCommand comando = new SqlCommand(consulta, cn);
            comando.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            comando.Parameters.AddWithValue("@fechaFin", fechaFin);
            comando.Parameters.AddWithValue("@transportista", transportista);

            // Crear el objeto SqlDataAdapter y DataSet para llenar el DataGridView
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataSet DS = new DataSet();
            adaptador.Fill(DS);

            // Asignar el DataSet como origen de datos del DataGridView
            dataGridView1.DataSource = DS.Tables[0];

            // Calcular el total a pagar
            decimal totalPagar = 0;
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                decimal totalViaje = Convert.ToDecimal(row["total_pagar"]);
                totalPagar += totalViaje;
            }

            // Mostrar el total a pagar en el TextBox
            textBox1.Text = "Total a pagar: $" + totalPagar.ToString();

            cn.Close();
        }
    }
}
