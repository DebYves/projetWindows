using NamRider.API.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// User out data class model
    /// </summary>
    public class UserModel
    {
        public string UserId {get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// User input EmailAddress data class model
    /// </summary>
    public class UserEmailModel
    {
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// User input UserName data class model
    /// </summary>
    public class UsernameModel
    {
        public string UserName { get; set; }
    }

}