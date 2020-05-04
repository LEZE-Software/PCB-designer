using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPD
{
    public partial class Form_Center : Form
    {
        public Form_Center()
        {
            InitializeComponent();
        }

        public bool displayDesigner = false;
        Form_Designer designForm;

        private void cmd_create_Click(object sender, EventArgs e)
        {
            int
                raw_height = Convert.ToInt32(txt_height.Text),
                raw_width = Convert.ToInt32(txt_width.Text),
                calc_height=0,
                calc_width=0;

            if(!Values.calcSizeByHoles)
            {
                calc_height = Convert.ToInt32(raw_height / 2.54);
                calc_width = Convert.ToInt32(raw_width / 2.54);
            }
            else
            {
                calc_height = raw_height;
                calc_width = raw_width;
            }

            if(!displayDesigner)
            {
                displayDesigner = true;
                Form_Designer local_form = new Form_Designer(this, calc_width, calc_height);
                designForm = local_form;
                designForm.Show();
            }
        }

        private void rad_sizeByHoles_CheckedChanged(object sender, EventArgs e)
        {
            Values.calcSizeByHoles = rad_sizeByHoles.Checked;

            if(Values.calcSizeByHoles)
            {
                lbl_indic_height.Text = 
                    lbl_indic_width.Text = "Löcher";
            }
            else
            {
                lbl_indic_height.Text =
                    lbl_indic_width.Text = "mm";
            }
        }

        private void Form_Center_Load(object sender, EventArgs e)
        {
            MessageBox.Show("PCB-Designer V1\n\n by LEZE-Software \n\n Mail to leze.software@gmail.com for support.","Releasenotes");
        }
    }
}
