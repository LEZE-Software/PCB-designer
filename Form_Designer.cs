using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace LPD
{
    public partial class Form_Designer : Form
    {
        public Form_Designer(Form_Center main, int width, int height)
        {
            this.mainForm = main;
            this.width = width;
            this.height = height;
            InitializeComponent();
        }

        public int width, height;

        PanelObject[,] panelArray;

        Form_Center mainForm;

        Point nullPosition = new Point(12, 12);

        private void Form_Designer_Load(object sender, EventArgs e)
        {
            pan_draw.Height = Values.POINT_HEIGHT * height +
                Values.DIST_Y * (height - 1);

            pan_draw.Width = Values.POINT_WIDTH * width +
                Values.DIST_X * (width - 1);

            cob_kindOfMark.SelectedIndex = (Int32)MarkMode.Free;

            lbl_mode.Text = "Modus: " + Values.modes[(Int32)Values.markMode];

            panelArray = new PanelObject[height, width];

            CreatePlate();
        }

        private void CreatePlate()
        {
            Point newLocation = new Point(0, 0);

            for (int i = 0; i < height; i++)
            {
                newLocation.X = 0;
                for (int k = 0; k < width; k++)
                {
                    Panel newPanel = new Panel()
                    {
                        Width = Values.POINT_WIDTH,
                        Height = Values.POINT_HEIGHT,
                        BackColor = Color.ForestGreen,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };

                    newPanel.MouseEnter += new EventHandler(pan_field_MouseEnter);
                    newPanel.MouseLeave += new EventHandler(pan_field_MouseLeave);

                    newPanel.Name = "field1_" + i + "x" + k;
                    newPanel.Tag = i + "x" + k;
                    newPanel.Parent = pan_draw;

                    newPanel.Location = newLocation;

                    panelArray[i, k].cableDirection = CableMode.None;
                    panelArray[i, k]._panel = newPanel;
                    panelArray[i,k].mode = MarkMode.Free;

                    pan_draw.Controls.Add(newPanel);

                    newLocation.X += Values.DIST_X + Values.POINT_WIDTH;
                }
                newLocation.Y += Values.DIST_Y + Values.POINT_HEIGHT;
            }
        }

        private void pan_field_MouseEnter(object sender, EventArgs e)
        {
            Panel senderPanel = sender as Panel;
            bool noSettingHere = false;
            string panelTag = senderPanel.Tag.ToString();

            int
                y = Convert.ToInt32(panelTag.Split('x')[0]),
                x = Convert.ToInt32(panelTag.Split('x')[1]);

            if (ModifierKeys == Keys.Shift)
            {
                Values.workMode = true;
            }         

            // Changes wanted?
            if (Values.workMode)
            {
                switch(Values.markMode)
                {
                    case MarkMode.Hole:
                        {
                            panelArray[y, x].cableDirection = CableMode.None;
                            panelArray[y, x].isSolder = false;
                            senderPanel.BackgroundImage = null;

                            // Is Cursor at the border?
                            if (x==0 || x==width-1 || y==0 || y==height-1)
                            {
                                noSettingHere = true;
                            }
                            break;
                        }
                    case MarkMode.CableLeft:
                        {
                            panelArray[y, x].cableDirection = CableMode.LeftRight;
                            panelArray[y, x].isSolder = true;
                            senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Left];
                            break;
                        }
                    case MarkMode.CableUp:
                        {
                            panelArray[y, x].cableDirection = CableMode.UpDown;
                            panelArray[y, x].isSolder = true;
                            senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Up];
                            break;
                        }
                    case MarkMode.SolderPoint:
                        {
                            panelArray[y, x].cableDirection = CableMode.None;
                            panelArray[y, x].isSolder = true;
                            senderPanel.BackgroundImage = cableImages.Images[2];
                            break;
                        }
                    default:
                        {
                            panelArray[y, x].cableDirection = CableMode.None;
                            panelArray[y, x].isSolder = false;
                            senderPanel.BackgroundImage = null;
                            break;
                        }
                }

                // Errors occured?
                if(noSettingHere==false)
                {
                    senderPanel.BackColor = Values.colors[(Int32)Values.markMode];
                    panelArray[y, x].mode = (MarkMode)Values.markMode;
                }
            }
            else
            {
                if(ModifierKeys==Keys.Control)
                {
                    switch (Values.markMode)
                    {
                        case MarkMode.Connector:
                        case MarkMode.Pin:
                        case MarkMode.Resistor:
                        case MarkMode.LED:
                        case MarkMode.Diode:
                        case MarkMode.Sensor:
                            {
                                senderPanel.BackgroundImage = cableImages.Images[2];
                                panelArray[y, x].cableDirection = CableMode.None;
                                panelArray[y, x].isSolder = true;
                                Values.workMode = true;
                                break;
                            }
                    }
                }           
            }

            // Display point information.
            lbl_legend.Text = Values.modes[(int)panelArray[y, x].mode];
            lbl_position.Text = String.Format("Position: {0} - {1}", y+1, x+1);
        }

        private void pan_field_MouseLeave(object sender, EventArgs e)
        {
            Values.workMode = false;
            Panel senderPanel = sender as Panel;

            string panelTag = senderPanel.Tag.ToString();

            int
                y = Convert.ToInt32(panelTag.Split('x')[0]),
                x = Convert.ToInt32(panelTag.Split('x')[1]);

            if(panelArray[y,x].isSolder==true && chb_afterCheck.Checked)
            {
                try
                {
                    // Refresh panel.
                    if (
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight)
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Cross_Left_Up];
                    }

                    if (
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight)
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Cross_Right_Up];
                    }

                    if (
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight)
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Cross_Right_Down];
                    }

                    if (
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight)
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Cross_Left_Down];
                    }

                    if (
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight &&
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown)
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.T_Right];
                    }
                    if (
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight &&
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown
                        )
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.T_Left];
                    }

                    if (
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight &&
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight
                        )
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.T_Up];
                    }

                    if (
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight &&
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight
                        )
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.T_Down];
                    }

                    if (
                        panelArray[y + 1, x].isSolder &&
                        panelArray[y + 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y - 1, x].isSolder &&
                        panelArray[y - 1, x].cableDirection == CableMode.UpDown &&
                        panelArray[y, x - 1].isSolder &&
                        panelArray[y, x - 1].cableDirection == CableMode.LeftRight &&
                        panelArray[y, x + 1].isSolder &&
                        panelArray[y, x + 1].cableDirection == CableMode.LeftRight
                        )
                    {
                        senderPanel.BackgroundImage = cableImages.Images[(Int32)ImgIdx.Cross_All];
                    }
                }
                catch
                {

                }
            }
          
        }

        private void cob_kindOfMark_SelectedIndexChanged(object sender, EventArgs e)
        {
            Values.markMode = (MarkMode)cob_kindOfMark.SelectedIndex;
            lbl_mode.Text = "Modus: " + Values.modes[(Int32)Values.markMode];
        }

        private void cmd_leftRight_Click(object sender, EventArgs e)
        {
            Values.markMode = MarkMode.CableLeft;
            lbl_mode.Text = "Modus: " + Values.modes[(Int32)Values.markMode];
        }

        private void cmd_updown_Click(object sender, EventArgs e)
        {
            Values.markMode = MarkMode.CableUp;
            lbl_mode.Text = "Modus: " + Values.modes[(Int32)Values.markMode];
        }

        private void Form_Designer_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.displayDesigner = false;
        }
    }
}
