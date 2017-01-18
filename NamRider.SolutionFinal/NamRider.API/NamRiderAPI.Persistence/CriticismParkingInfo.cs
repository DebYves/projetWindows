namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CriticismParkingInfo")]
    public partial class CriticismParkingInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(128)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdParking { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ParkingInfo ParkingInfo { get; set; }

        public CriticismParkingInfo() { }

    }
}
