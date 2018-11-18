using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_Contacts_App
{
    public partial class Edit_User_Data_Form : Form
    {
        public Edit_User_Data_Form()
        {
            InitializeComponent();
        }

        USER user = new USER();

        // on form load
        private void Edit_User_Data_Form_Load(object sender, EventArgs e)
        {
            // display image on the panel ( close and minimize )
            panel4.BackgroundImage = Image.FromFile("../../images/img4.png");
            
            // display the user data
            DataTable table = user.getUserById(Globals.GlobalUserId);

            textBoxFName.Text = table.Rows[0][1].ToString();
            textBoxLName.Text = table.Rows[0][2].ToString();
            textBoxUsername.Text = table.Rows[0][3].ToString();
            textBoxPassword.Text = table.Rows[0][4].ToString();

            byte[] pic = (byte[])table.Rows[0]["pic"];
            MemoryStream picture = new MemoryStream(pic);
            pictureBoxUserImage.Image = Image.FromStream(picture);
        }


        // button browse image
        private void button_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                pictureBoxUserImage.Image = Image.FromFile(opf.FileName);
            }
        }


        // button edit the logged user data
        private void button_Edit_User_Click(object sender, EventArgs e)
        {
            MY_DB mydb = new MY_DB();

            Int32 id = Globals.GlobalUserId; // get the user id
            string fname = textBoxFName.Text;
            string lname = textBoxLName.Text;
            string uname = textBoxUsername.Text;
            string pass = textBoxPassword.Text;

            MemoryStream pic = new MemoryStream();
            pictureBoxUserImage.Image.Save(pic, pictureBoxUserImage.Image.RawFormat);

            if(uname.Equals("") || pass.Equals("")) // check if the username or the password are empty
            {
                MessageBox.Show("Required Fields: username and password", "Edit Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (user.updateUser(id, fname, lname, uname, pass, pic))
                {
                    MessageBox.Show("Your Inormation Has Been UpDated", "Edit Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error", "Edit Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }


        // button minimize
        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        // button minimize
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBoxUserImage_Click(object sender, EventArgs e)
        {

        }
    }
}
