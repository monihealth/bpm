using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.GalenPCL;
using System.Net;
using System.Threading;
using System.IO;
using System.IO.Compression;


namespace MoniHealth.Models
{
    public class UserAccountInformation
    {
        private string Fname;
        private string Lname;
        private string email;
        private string password;

        public String FirstName { get { return Fname; } set { Fname = value; } }
        public String LastName { get { return Lname; } set { Lname = value; } }
        public String Email { get { return email; } set { email = value; } }
        public String Password { get { return password; } set { password = value; } }
    }
}
