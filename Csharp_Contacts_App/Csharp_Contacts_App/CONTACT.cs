using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Csharp_Contacts_App
{
    class CONTACT
    {

        MY_DB mydb = new MY_DB();


        // function to insert a new contact
        public bool insertContact(string fname, string lname, string phone, string address, string email, int userid, int groupid, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("INSERT INTO [mycontact](fname, lname, group_id, phone, email, address, pic, userid) VALUES (@fn, @ln, @grp, @phn, @mail, @adrs, @pic, @uid)", mydb.getConnection);

            command.Parameters.AddWithValue("@fn", fname);
            command.Parameters.AddWithValue("@ln", lname);
            command.Parameters.AddWithValue("@grp", groupid);
            command.Parameters.AddWithValue("@phn", phone);
            command.Parameters.AddWithValue("@mail", email);
            command.Parameters.AddWithValue("@adrs", address);
            command.Parameters.AddWithValue("@uid", userid);
            command.Parameters.AddWithValue("@pic", picture.ToArray());

            mydb.openConnection();

            if ((command.ExecuteNonQuery() == 1))
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



        // function to update the selected contact
        public bool updateContact(int contactid, string fname, string lname, string phone, string address, string email, int groupid, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("UPDATE [mycontact] SET fname= @fn,lname= @ln,group_id= @gid,phone= @phn,email= @mail,address= @adrs,pic= @pic WHERE id = @id", mydb.getConnection);

            command.Parameters.AddWithValue("@id", contactid);
            command.Parameters.AddWithValue("@fn", fname);
            command.Parameters.AddWithValue("@ln", lname);
            command.Parameters.AddWithValue("@gid", groupid);
            command.Parameters.AddWithValue("@phn", phone);
            command.Parameters.AddWithValue("@mail", email);
            command.Parameters.AddWithValue("@adrs", address);
            command.Parameters.AddWithValue("@pic", picture.ToArray());

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


        // function to delete the selected contact
        public bool deleteContact(int contactid)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [mycontact] WHERE id = @id", mydb.getConnection);

            command.Parameters.AddWithValue("@id", contactid);
            
            mydb.openConnection();

            if ((command.ExecuteNonQuery() == 1))
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


        // function to return the contacts list depending on the given command
        public DataTable SelectContactList(SqlCommand command)
        {
            command.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable DS = new DataTable();
            adapter.Fill(DS);
            return DS;
        }



        public DataTable GetContactById(Int32 contactId)
        {
            SqlCommand command = new SqlCommand("SELECT id, fname, lname, group_id, phone, email, address, pic, userid FROM [mycontact] WHERE id = @id", mydb.getConnection);
            command.Parameters.AddWithValue("@id", contactId);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}
