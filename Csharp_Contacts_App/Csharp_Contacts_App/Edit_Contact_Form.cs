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
using System.IO;

namespace Csharp_Contacts_App
{
    public partial class Edit_Contact_Form : Form
    {
        public Edit_Contact_Form()
        {
            InitializeComponent();
        }

        MY_DB mydb = new MY_DB();

        // on form load
        private void Edit_Contact_Form_Load(object sender, EventArgs e)
        {
            // display image on the panel ( close and minimize )
            panel3.BackgroundImage = Image.FromFile("../../images/img3.png");
            
            GROUP group = new GROUP();

            // populate combobox with the logged user groups
            comboBoxGroup.DataSource = group.getGroups(Globals.GlobalUserId);
            comboBoxGroup.DisplayMember = "name";
            comboBoxGroup.ValueMember = "id";
        }


        // button browse image
        private void button_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                pictureBoxContactImage.Image = Image.FromFile(opf.FileName);
            }
        }


        // button select the contact you want to edit from the list
        private void buttonSelectContact_Click(object sender, EventArgs e)
        {
            // open the form to select a contact from it
            Select_Contact_Form SelectContactF = new Select_Contact_Form();
            SelectContactF.ShowDialog();

            try
            {
                    // get the contact id
                    int contactId = Convert.ToInt32(SelectContactF.dataGridView1.CurrentRow.Cells[0].Value.ToString());

                    CONTACT contact = new CONTACT();
                    DataTable table = contact.GetContactById(contactId);

                    textBoxContactId.Text = table.Rows[0]["id"].ToString();
                    textBoxFName.Text = table.Rows[0]["fname"].ToString();
                    textBoxLName.Text = table.Rows[0]["lname"].ToString();
                    comboBoxGroup.SelectedValue = table.Rows[0]["group_id"];
                    textBoxPhone.Text = table.Rows[0]["phone"].ToString();
                    textBoxEmail.Text = table.Rows[0]["email"].ToString();
                    textBoxAddress.Text = table.Rows[0]["address"].ToString();

                    byte[] pic = (byte[])table.Rows[0]["pic"];
                    MemoryStream picture = new MemoryStream(pic);
                    pictureBoxContactImage.Image = Image.FromStream(picture);

            }
            catch(Exception)
            {
                // if no contact is selected
            }
        }


        // button edit contact
        private void button_Edit_Contact_Click(object sender, EventArgs e)
        {
            CONTACT contact = new CONTACT();

            string fname = textBoxFName.Text;
            string lname = textBoxLName.Text;
            string phone = textBoxPhone.Text;
            string address = textBoxAddress.Text;
            string email = textBoxEmail.Text;

            try
            {
                Int32 id = Convert.ToInt32(textBoxContactId.Text);

                int groupid = (int)comboBoxGroup.SelectedValue;

                MemoryStream pic = new MemoryStream();
                pictureBoxContactImage.Image.Save(pic, pictureBoxContactImage.Image.RawFormat);

                if (contact.updateContact(id, fname, lname, phone, address, email, groupid, pic))
                {
                    MessageBox.Show("Contact Inormation UpDated", "Edit Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error", "Add Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Contact", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
