using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_Contacts_App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           /* Login_Register_Form fLogin = new Login_Register_Form();
            if (fLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(fLogin);
            }
            else
            {
                Application.Exit();
            }*/
            Login_Register_Form fLogin = new Login_Register_Form();
            if (fLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Main_Form());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
