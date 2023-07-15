using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace farsiman4
{
    public partial class Principal : Form
    {

        public string UsuarioLogueado { get; set; }

        public Principal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 formularioPrincipal = new Form2();
            formularioPrincipal.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 formularioViajes = new Form3();
            formularioViajes.UsuarioLogueado = UsuarioLogueado; // Asigna el valor del usuario logueado
            formularioViajes.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 formularioPrincipal = new Form4();
            formularioPrincipal.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 formularioPrincipal = new Form1();
            formularioPrincipal.Show();
            this.Hide();

        }
    }
}
