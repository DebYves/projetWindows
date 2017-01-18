namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Evaluation")]
    public partial class Evaluation
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(128)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdDriving { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        // foreigns Keys
        public virtual AspNetUser AspNetUser { get; set; }

        public virtual DrivingInfo DrivingInfo { get; set; }

        public Evaluation() { }

    }
}
