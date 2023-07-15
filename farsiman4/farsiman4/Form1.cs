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
using farsiman4.FARSIMANDataSetTableAdapters;
using Microsoft.Win32;

namespace farsiman4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection("Data Source=DESKTOP-R2U3GDA\\SQLEXPRESS;Initial Catalog=FARSIMAN;Integrated Security=True");

        private string nombreUsuario;

        public string UsuarioLogueado { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
       

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cn.Open();
            string consulta = "select * from Usuarios where nombre_usuario='"+usuario.Text+"' and contrasena='"+contra.Text+ "' ";
            SqlCommand comando = new SqlCommand(consulta, cn);
            SqlDataReader lector;
            lector = comando.ExecuteReader();

            if (lector.HasRows == true)
            {
                lector.Close();
                cn.Close();

                nombreUsuario = usuario.Text;

                Principal formularioViajes = new Principal();
                formularioViajes.UsuarioLogueado = nombreUsuario; // Asignar el nombre de usuario al Form2
                formularioViajes.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Usuario o contrasena Incorrecto");
            }
            cn.Close();



        }
    }
}
