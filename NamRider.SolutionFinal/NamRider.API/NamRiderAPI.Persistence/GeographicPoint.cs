namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeographicPoint")]
    public partial class GeographicPoint
    {
        [Key]
        [Column(Order = 0)]
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Key]
        [Column(Order = 1)]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        [Required]
        public int IdParking { get; set; }

        public virtual ParkingInfo ParkingInfo { get; set; }

        private GeographicPoint() { }

        public GeographicPoint(decimal latitude, decimal longitude, int idParking)
        {
            #region contraints check
            if (latitude < -90 || latitude > 90) throw new ArgumentException("The latitude is invalid");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("The longitude is invalid");
            #endregion
            Latitude = latitude;
            Longitude = longitude;
            IdParking = idParking;
        }
    }
}
