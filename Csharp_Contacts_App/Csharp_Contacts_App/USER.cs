using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Contacts_App
{
    class USER
    {
        private static int USERID = 0;
        MY_DB db = new MY_DB();

        public DataTable getUserById(Int32 userid)
        {
            
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable table = new DataTable();

            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE id = @uid", db.getConnection);
            command.Parameters.AddWithValue("@uid", userid);

            adapter.SelectCommand = command;

            adapter.Fill(table);

            return table;
        }


        // function to insert a new user
        public bool insertUser(string fname, string lname, string username, string password, MemoryStream picture)
        {

            SqlCommand command = new SqlCommand("INSERT INTO [user](fname, lname, username, pass, pic) VALUES(@fn, @ln, @un, @pass, @pic)", db.getConnection);


            command.Parameters.AddWithValue("@id", USERID);
            command.Parameters.AddWithValue("@fn", fname);
            command.Parameters.AddWithValue("@ln", lname);
            command.Parameters.AddWithValue("@un", username);
            command.Parameters.AddWithValue("@pass", password);
            command.Parameters.AddWithValue("@pic", picture.ToArray());

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }

        }


        // function to check if the username already exists
        public bool usernameExist(string username, string operation, int userid = 0)
        {
            string query = "";

            if (operation == "register")
            {
                // if a new user want to register we will check if the username already exists
                query = "SELECT * FROM [user] WHERE username = @un";
            }
            else if(operation == "edit")
            {
                // if an existing student want to edit his information 
                // we will check if he enter an existing username ( not including his own username )
                query = "SELECT * FROM [user] WHERE username = @un AND id = @uid";
            }

            SqlCommand command = new SqlCommand(query, db.getConnection);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@un", username);
            command.Parameters.AddWithValue("@uid", userid);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DS = new DataSet();

            adapter.Fill(DS);

            adapter.Fill(DS);
            DataTable dtable = DS.Tables[0];

            if (dtable.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        // function to update the logged user data
        public bool updateUser(int userid, string fname, string lname, string username, string password, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("UPDATE user SET fname= @fn,lname= @ln,username= @un,pass= @pass,pic= @pic WHERE id = @uid", db.getConnection);



            command.Parameters.AddWithValue("@fn", fname);
            command.Parameters.AddWithValue("@ln", lname);
            command.Parameters.AddWithValue("@un", username);
            command.Parameters.AddWithValue("@pass", password);
            command.Parameters.AddWithValue("@pic", picture.ToArray());
            command.Parameters.AddWithValue("@uid", userid);

            db.openConnection();

            if ( command.ExecuteNonQuery() == 1 )
            {
                db.closeConnection();
                return true;
            }
            else
            {
                db.closeConnection();
                return false;
            }

        }


    }
}
