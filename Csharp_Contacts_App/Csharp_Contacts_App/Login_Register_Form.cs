using System.Data.SqlClient;
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
    public partial class Login_Register_Form : Form
    {
        public Login_Register_Form()
        {
            InitializeComponent();
        }

        MY_DB mydb = new MY_DB();


        // when this timer will start we will change the panel location to show only the register part
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel2.Location.X > -260)
            {
                panel2.Location = new Point(panel2.Location.X - 10, panel2.Location.Y);
            }
            else
            {
                timer1.Stop();
                labelGoToRegister.Enabled = true;
                labelGoToLogin.Enabled = true;
            }
        }


        // label go to the register panel
        private void labelGoToRegister_Click(object sender, EventArgs e)
        {
            timer1.Start();
            labelGoToRegister.Enabled = false;
            labelGoToLogin.Enabled = false;
        }


        // label go to the login panel
        private void labelGoToLogin_Click(object sender, EventArgs e)
        {
            timer2.Start();
            labelGoToRegister.Enabled = false;
            labelGoToLogin.Enabled = false;
        }


        // when this timer will start we will change the panel location to show only the login part
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (panel2.Location.X < 0)
            {
                panel2.Location = new Point(panel2.Location.X + 10, panel2.Location.Y);
            }
            else
            {
                timer2.Stop();
                labelGoToRegister.Enabled = true;
                labelGoToLogin.Enabled = true;
            }
        }


        // button browse image
        private void button_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                pictureBoxProfileImage.Image = Image.FromFile(opf.FileName);
            }
        }


        // button register
        private void button_Register_Click(object sender, EventArgs e)
        {
            string fname = textBoxFName.Text;
            string lname = textBoxLName.Text;
            string username = textBoxUsernameRegister.Text;
            string password = textBoxPasswordRegister.Text;

            USER user = new USER();


            if (verifFields("register")) // check empty fields
            {
                MemoryStream pic = new MemoryStream();
                pictureBoxProfileImage.Image.Save(pic, pictureBoxProfileImage.Image.RawFormat);

                if (!user.usernameExist(textBoxUsernameRegister.Text, "register")) // check if the username already exists on the database using 'usernameExist' function in USER class
                {
                    if (user.insertUser(fname, lname, username, password, pic)) // add user
                    {
                        MessageBox.Show("Registration Completed Successfully", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Something Wrong", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("This Username Already Exists, Try Another One", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("* Required Fields - username / password / image", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }


        // button login
        private void button_login_Click(object sender, EventArgs e)
        {
            MY_DB db = new MY_DB();

            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE username = @usn AND pass = @pass", db.getConnection);

            if (verifFields("login")) // check empty fields
            {
                command.Parameters.AddWithValue("@usn", TextBoxUsername.Text);
                command.Parameters.AddWithValue("@pass", TextBoxPassword.Text);

                adapter.SelectCommand = command;

                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {

                    int userid = Convert.ToInt32(table.Rows[0][0].ToString());

                    // we will get the user id and make it global ( visible in all forms and classes ) using Globals class
                    Globals.SetGlobalUserIId(userid);


                    // if this user exist 
                    // we will set the dialogResult to OK
                    // that mean the login informatons are correct -> open the mainform
                    // check out the "Program.cs"
                    // learn more here -> https://stackoverflow.com/questions/4759334/how-can-i-close-a-login-form-and-show-the-main-form-without-my-application-closi 
                    /*Hide();
                    Main_Form mForm = new Main_Form();
                    mForm.Show();*/
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Invalid Username Or Password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Empty Username Or Password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // check empty fields
        public bool verifFields(string operation)
        {
            bool check = false;

            // if the operation is register
            if(operation == "register")
            {
                if(textBoxUsernameRegister.Text.Equals("") || textBoxPasswordRegister.Text.Equals("") || pictureBoxProfileImage.Image == null)
                {
                    check = false;
                }
                else{
                    check = true;
                }
            }

            // if the operation is login
            else if(operation == "login")
            {       
                if (TextBoxUsername.Text.Equals("") || TextBoxPassword.Text.Equals(""))
                {
                    check = false;
                }
                else
                {
                    check = true;
                }
            }

            return check;
        }

  
        // button minimize
        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        // button close
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        // display image on the panel ( close and minimize )
        private void Login_Register_Form_Load(object sender, EventArgs e)
        {
            panel3.BackgroundImage = Image.FromFile("../../images/img1.png");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
