using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamRider.Model
{
    public class CriticismDrivingModel
    {
        public int IdDriving { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public int Value { get; set; }
    }

    public class CriticismDrivingInputModel
    {
        [Required]
        public int IdDriving { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }
    }
}
