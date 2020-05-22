using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrownianMotion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count;
            try
            {
                count = Convert.ToInt32(comboBox1.Text);
            }
            catch
            {
                count = 1;
            }
            movingForm mf = new movingForm(count);
                 
            mf.Show();
        }
    }
}
