using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamRider.Model
{
    public class Response
    {
        public Response(){ }

        public bool IsSuccess { get; set; }
        public int Id { get; set; }
        public string ErrorMsg { get; set; }
    }
}
