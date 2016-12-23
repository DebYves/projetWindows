namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkingInfo")]
    public partial class ParkingInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParkingInfo()
        {
            CriticismParkingInfoes = new HashSet<CriticismParkingInfo>();
            GeographicPoints = new HashSet<GeographicPoint>();
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Range(0, 100)]
        public int? ValuePertinence { get; set; }

        [Required]
        public bool IsValidatedPertinence { get; set; }

        [Required]
        public bool IsReportedOutDated { get; set; }

        [Required]
        [Range(0, 999)]
        public decimal Rayon { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Range(1, 99999)]
        public int? NumberPlace { get; set; }

        public bool? IsEstimatedPlace { get; set; }

        [StringLength(350)]
        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string IdUserPublication { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        // Reverse navigation
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriticismParkingInfo> CriticismParkingInfoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeographicPoint> GeographicPoints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }

        public ParkingInfo(int id, DateTime date, int? pertinence, bool isValidPert, bool isReportDate, decimal rayon, string type, int? number, bool? isEstimated, string descrip, string idUser)
        {
            #region Contraints
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            if (rayon <= 0) throw new ArgumentException("The rayon is invalid");
            if (number <= 0) throw new ArgumentException("The number place is invalid");
            #endregion
            Id = id;
            Date = date;
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Rayon = rayon;
            Type = type;
            NumberPlace = number;
            IsEstimatedPlace = isEstimated;
            Description = descrip;
            IdUserPublication = idUser;
        }

        public ParkingInfo(DateTime date, int? pertinence, bool isValidPert, bool isReportDate, decimal rayon, string type, int? number, bool? isEstimated, string descrip, string idUser)
        {
            #region Contraints
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            if (rayon <= 0) throw new ArgumentException("The rayon is invalid");
            if (number <= 0) throw new ArgumentException("The number place is invalid");
            #endregion
            Date = date;
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Rayon = rayon;
            Type = type;
            NumberPlace = number;
            IsEstimatedPlace = isEstimated;
            Description = descrip;
            IdUserPublication = idUser;
        }

        public ParkingInfo(int id, DateTime date, bool isValidPert, bool isReportDate, decimal rayon, string type, string descrip, string idUser)
        {
            #region Contraints
            if (rayon <= 0) throw new ArgumentException("The rayon is invalid");
            #endregion
            Id = id;
            Date = date;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Rayon = rayon;
            Type = type;
            Description = descrip;
            IdUserPublication = idUser;
        }

        public ParkingInfo(DateTime date, bool isValidPert, bool isReportDate, decimal rayon, string type, string descrip, string idUser)
        {
            #region Contraints
            if (rayon <= 0) throw new ArgumentException("The rayon is invalid");
            #endregion
            Date = date;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Rayon = rayon;
            Type = type;
            Description = descrip;
            IdUserPublication = idUser;
        }

        public void UpdateParkiningInfo(int? pertinence, bool isValidPert)
        {
            #region contraints check
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            #endregion
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
        }

        public void UpdateParkiningInfo(bool isReportDate)
        {
            IsReportedOutDated = isReportDate;
        }
    }
}
