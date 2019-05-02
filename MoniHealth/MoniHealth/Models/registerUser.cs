using System;
using System.Collections.Generic;
using System.Text;

namespace MoniHealth.Models
{
    public class registerUser
    {
        private string Fname;
        private string Lname;
        private string email;
        private string password;

        public String FirstName { get{ return Fname; } set { Fname = value; } }
        public String LastName { get { return Lname; } set { Lname = value; } }
        public String Email { get { return email; } set { email = value; } }
        public String Password { get { return password; } set { password = value; } }
    }
}
