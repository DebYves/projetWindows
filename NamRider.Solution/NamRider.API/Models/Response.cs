using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamRider.API.Models
{
    /// <summary>
    /// Operation return class
    /// </summary>
    public class Response
    {
        public Response()
        {}

        public bool IsSuccess { get; set; }

        public int Id { get; set; }
        public string IdString { get; set; }
        public string ErrorMsg { get; set; }
        public string SucessrMsg { get; set; }

    }
}