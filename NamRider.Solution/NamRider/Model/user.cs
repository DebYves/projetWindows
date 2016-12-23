using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamRider.Model
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
    }

    public class UserInput
    {
        public string Email { get; set; }public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class UserLogin
    {

        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UserEmailModel
    {
        public string EmailAddress { get; set; }
    }

    public class UsernameModel
    {
        public string UserName { get; set; }
    }
}
