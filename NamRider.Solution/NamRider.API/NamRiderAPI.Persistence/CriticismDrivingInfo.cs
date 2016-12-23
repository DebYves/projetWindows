namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CriticismDrivingInfo")]
    public partial class CriticismDrivingInfo
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

        public virtual AspNetUser UserCritismDriving { get; set; }

        public virtual DrivingInfo DrivingInfoCritism { get; set; }

        public CriticismDrivingInfo() { }
    }
}
