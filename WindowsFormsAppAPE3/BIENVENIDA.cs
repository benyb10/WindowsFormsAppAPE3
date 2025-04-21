using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppAPE3
{
    public partial class BIENVENIDA: Form
    {
        public BIENVENIDA()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new REGISTRO().Show();
            this.Hide();
        }
    }
}
