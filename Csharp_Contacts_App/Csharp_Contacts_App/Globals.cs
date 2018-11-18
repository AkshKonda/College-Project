using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Contacts_App
{
    class Globals
    {
        // class to make the logged in user id global everywhere on the application
        // learn more at -> https://www.codeproject.com/Questions/233650/How-to-define-Global-veriable-in-Csharp-net

        public static int GlobalUserId { get; private set; }

        public static void SetGlobalUserIId(int userId)
        {
            GlobalUserId = userId;
        }

    }
}
