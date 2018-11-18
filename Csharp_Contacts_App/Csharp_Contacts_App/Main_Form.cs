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
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }

        MY_DB mydb = new MY_DB();
        GROUP group = new GROUP();


        // in the form load
        private void Form2_Load(object sender, EventArgs e)
        {
            // display image on the panel ( close and minimize )
            panel4.BackgroundImage = Image.FromFile("../../images/img2.png");

            // get and display user username and proile picture
            getImageAndUsername();

            // populate comboboxes
            getGroups();
        }


        // button add a new group
        private void buttonAddGroup_Click(object sender, EventArgs e)
        {

            string groupName = textBoxAddGroupName.Text;

            if(!groupName.Trim().Equals(""))
            {
                // check if this group name already exist for this user
                if(!group.groupExist(groupName,"add",Globals.GlobalUserId))
                {
                    if (group.insertGroup(groupName, Globals.GlobalUserId)) // insert group for this user
                    {
                        MessageBox.Show("New Group Inserted", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Group Not Inserted", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    getGroups();
                }
                else
                {
                    MessageBox.Show("This Group Name Already Exists", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            else
            {
                MessageBox.Show("Enter a Group Name Before Inserting", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // function to populate comboboxes with groups name and id
        public void getGroups()
        {
            comboBoxEditGroupId.DataSource = group.getGroups(Globals.GlobalUserId);
            comboBoxEditGroupId.DisplayMember = "name";
            comboBoxEditGroupId.ValueMember = "id";

            comboBoxRemoveGroupId.DataSource = group.getGroups(Globals.GlobalUserId);
            comboBoxRemoveGroupId.DisplayMember = "name";
            comboBoxRemoveGroupId.ValueMember = "id";
        }

       

        // button edit selected group from combobox
        private void buttonEditGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string newGroupName = textBoxEditGroupName.Text;
                string groupName = comboBoxEditGroupId.SelectedText.ToString();
                int groupid = Convert.ToInt32(comboBoxEditGroupId.SelectedValue.ToString());

                if (!newGroupName.Trim().Equals(""))
                {
                    if (!group.groupExist(newGroupName, "edit",Globals.GlobalUserId, groupid))
                    {
                        if (group.updateGroup(groupid, newGroupName))
                        {
                            MessageBox.Show("Group Name Updated", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Group Not Updated", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        getGroups();
                    }
                    else
                    {
                        MessageBox.Show("This Group Name Already Exists", "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                else
                {
                    MessageBox.Show("Enter a Group Name Before Editing", "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // "Select a Group Before Editing"
                MessageBox.Show(ex.Message, "Edit Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        // button remove group
        private void buttonRemoveGroup_Click(object sender, EventArgs e)
        {
            try
            {
                int groupid = Convert.ToInt32(comboBoxRemoveGroupId.SelectedValue.ToString());

                if ((MessageBox.Show("Are You sure You Want To Delete This Group", "Remove Group", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if (group.deleteGroup(groupid))
                    {
                        MessageBox.Show("Group Deleted", "Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Group Not Deleted", "Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    getGroups();
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Select a Group Before Deleting", "Remove Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        // button show add contact form
        private void buttonAddContact_Click(object sender, EventArgs e)
        {
            Add_Contact_Form addContactForm = new Add_Contact_Form();
            addContactForm.Show(this);
        }


        // button show edit contact form
        private void buttonEditContact_Click(object sender, EventArgs e)
        {
            Edit_Contact_Form editContactForm = new Edit_Contact_Form();
            editContactForm.Show(this);
        }


        // button remove selected contact by id
        private void buttonRemoveContact_Click(object sender, EventArgs e)
        {
            CONTACT contact = new CONTACT();

            if (!TextBoxContactId.Text.Equals("")) // check if TextBoxContactId is empty
            {
                int contactid = Convert.ToInt32(TextBoxContactId.Text);

                if (contact.deleteContact(contactid))
                {
                    MessageBox.Show("Contact Deleted", "Remove Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Remove Contact", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                MessageBox.Show("No Contact Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        // button select contact to show a datagridview with contacts name and id to select one from it
        private void buttonSelectContact_Click(object sender, EventArgs e)
        {
            Select_Contact_Form SelectContactF = new Select_Contact_Form();

            SelectContactF.ShowDialog();

            try
            {
                TextBoxContactId.Text = SelectContactF.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            catch(Exception)
            {
                MessageBox.Show("No Contact Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }


        // button show full contacts list
        private void buttonShowFullList_Click(object sender, EventArgs e)
        {
            Contacts_Full_List_Form fullListForm = new Contacts_Full_List_Form();
            fullListForm.Show(this);
        }


        // function to get the logged user image and username 
        public void getImageAndUsername()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataSet DS = new DataSet();

            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE id = @uid", mydb.getConnection);
            command.Parameters.AddWithValue("@uid", Globals.GlobalUserId);

            adapter.SelectCommand = command;

            adapter.Fill(DS);
            DataTable dtable = DS.Tables[0];

            if ((DS.Tables.Count > 0))
            {
                byte[] pic = (byte[])dtable.Rows[0][5];
                MemoryStream picture = new MemoryStream(pic);
                pictureBox1.Image = Image.FromStream(picture);

                labelUsername.Text = "Welcome Back ( " + dtable.Rows[0]["username"].ToString() + " )";
            }
        }


        // label edit user data click event
        private void labelEditUserData_Click(object sender, EventArgs e)
        {
            Edit_User_Data_Form editUserForm = new Edit_User_Data_Form();
            editUserForm.Show(this);
        }


        // label refresh click event
        private void labelRefresh_Click(object sender, EventArgs e)
        {
            getImageAndUsername();
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

        private void labelLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login_Register_Form lForm = new Login_Register_Form();
            lForm.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }

}

