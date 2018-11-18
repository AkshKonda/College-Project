using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Csharp_Contacts_App
{
    class GROUP
    {

        MY_DB mydb = new MY_DB();


        // function to insert a new group for a specific user
        public bool insertGroup(string gname, int userid)
        {
            SqlCommand command = new SqlCommand("INSERT INTO [mygroups](name, userid) VALUES (@gn, @uid)", mydb.getConnection);

            command.Parameters.AddWithValue("@gn", gname);
            command.Parameters.AddWithValue("@uid", userid);

            mydb.openConnection();

            if ( command.ExecuteNonQuery() == 1 )
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }

        }



        // function to update the selected group
        public bool updateGroup(int gid, string gname)
        {
            SqlCommand command = new SqlCommand("UPDATE [mygroups] SET name= @name WHERE id=@id", mydb.getConnection);

            command.Parameters.AddWithValue("@name", gname);
            command.Parameters.AddWithValue("@id", gid);

            mydb.openConnection();

            if ( command.ExecuteNonQuery() == 1 )
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }

        }


        // function to delete the selected group
        public bool deleteGroup(int groupid)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [mygroups] WHERE id = @id", mydb.getConnection);

            command.Parameters.AddWithValue("@id", groupid);

            mydb.openConnection();

            if ( command.ExecuteNonQuery() == 1 )
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }

        }


        // function to get contact groups for a specific user
        public DataTable getGroups( int userid )
        {
            SqlCommand command = new SqlCommand("SELECT * FROM mygroups WHERE userid = @uid", mydb.getConnection);
           
            command.Parameters.AddWithValue("@uid", userid);
            command.Connection = mydb.getConnection;
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            
            DataTable table = new DataTable();
            
            adapter.Fill(table);
            
            return table;
        }



        // function to check if the group name already exists ( for the logged user )
        public bool groupExist(string name, string operation, int userid = 0, int groupid = 0)
        {
            string query = "";

            SqlCommand command = new SqlCommand();

            if (operation == "add") // when inserting a new group
            {
                // if the new group name already exists
                query = "SELECT * FROM [mygroups] WHERE name = @name AND userid = @uid";
                
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@uid", userid);
            }
            else if (operation == "edit") // when editing a group name
            {
                // we will check if the enter an existing group name
                query = "SELECT * FROM [mygroups] WHERE name = @name AND userid = @uid AND id <> @gid";

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@uid", userid);
                command.Parameters.AddWithValue("@gid", groupid);
            }

            command.Connection = mydb.getConnection;
            command.CommandText = query;
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            adapter.SelectCommand = command;

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}
