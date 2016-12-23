namespace NamRider.API.NamRiderAPI.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DrivingInfo")]
    public partial class DrivingInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DrivingInfo()
        {
            CriticismDrivingInfoes = new HashSet<CriticismDrivingInfo>();
            Evaluations = new HashSet<Evaluation>();
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

        [StringLength(300)]
        public string AdditionalInfo { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        [Required]
        [StringLength(200)]
        public string StreetName { get; set; }

        [Required]
        [StringLength(350)]
        public string Description { get; set; }

        [Range(0, 100)]
        public int? Severity { get; set; }

        [Required]
        [StringLength(128)]
        public string IdUserPublication { get; set; }

        // foreign key
        public virtual AspNetUser UserPublication { get; set; }

        // Reverse navigation
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CriticismDrivingInfo> CriticismDrivingInfoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }

        public DrivingInfo(DateTime date, bool isValidPert, bool isReportDate, decimal latitude, decimal longitude, string street, string description, string idUser)
        {
            #region contraints check
            if (latitude < -90 || latitude > 90) throw new ArgumentException("The latitude is invalid");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("The longitude is invalid");
            if (street.Length < 2 || street.Length > 200) throw new ArgumentNullException("The street name is not valable");
            if (description.Length < 2 || description.Length > 350) throw new ArgumentNullException("The description is not valable");
            #endregion
            Date = date;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Latitude = latitude;
            Longitude = longitude;
            StreetName = street;
            Description = description;
            IdUserPublication = idUser;
        }

        public DrivingInfo(int id, DateTime date, bool isValidPert, bool isReportDate, decimal latitude, decimal longitude, string street, string description, string idUser)
        {
            #region contraints check
            if (latitude < -90 || latitude > 90) throw new ArgumentException("The latitude is invalid");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("The longitude is invalid");
            if (street.Length < 2 || street.Length > 200) throw new ArgumentNullException("The street name is not valable");
            if (description.Length < 2 || description.Length > 350) throw new ArgumentNullException("The description is not valable");
            #endregion
            Id = id;
            Date = date;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            Latitude = latitude;
            Longitude = longitude;
            StreetName = street;
            Description = description;
            IdUserPublication = idUser;
        }


        public DrivingInfo(int id, DateTime date, int? pertinence, bool isValidPert, bool isReportDate, string addnfo, decimal latitude, decimal longitude, string street, string description, int? severity, string idUser)
        {
            #region contraints check
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            if (latitude < -90 || latitude > 90) throw new ArgumentException("The latitude is invalid");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("The longitude is invalid");
            if (street.Length < 2 || street.Length > 200) throw new ArgumentNullException("The street name is not valable");
            if (description.Length < 2 || description.Length > 350) throw new ArgumentNullException("The description is not valable");
            if (severity < 0 || severity > 100) throw new ArgumentException("The servity is invalid");
            #endregion
            Id = id;
            Date = date;
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            AdditionalInfo = addnfo;
            Latitude = latitude;
            Longitude = longitude;
            StreetName = street;
            Description = description;
            Severity = severity;
            IdUserPublication = idUser;
        }

        public DrivingInfo(DateTime date, int? pertinence, bool isValidPert, bool isReportDate, string addnfo, decimal latitude, decimal longitude, string street, string description, int? severity, string idUser)
        {
            #region contraints check
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            if (latitude < -90 || latitude > 90) throw new ArgumentException("The latitude is invalid");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("The longitude is invalid");
            if (street.Length < 2 || street.Length > 200) throw new ArgumentNullException("The street name is not valable");
            if (description.Length < 2 || description.Length > 350) throw new ArgumentNullException("The description is not valable");
            if (severity < 0 || severity > 100) throw new ArgumentException("The servity is invalid");
            #endregion
            Date = date;
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
            IsReportedOutDated = isReportDate;
            AdditionalInfo = addnfo;
            Latitude = latitude;
            Longitude = longitude;
            StreetName = street;
            Description = description;
            Severity = severity;
            IdUserPublication = idUser;
        }

        public void UpdateDrivingInfo(int? pertinence, bool isValidPert)
        {
            #region contraints check
            if (pertinence < 0 || pertinence > 100) throw new ArgumentException("The pertinence is invalid");
            #endregion
            ValuePertinence = pertinence;
            IsValidatedPertinence = isValidPert;
        }

        public void UpdateDrivingInfo(int severity)
        {
            #region contraints check
            if (severity < 0 || severity > 100) throw new ArgumentException("The servity is invalid");
            #endregion
            Severity = severity;
        }

        public void UpdateDrivingInfo( bool isReportDate)
        {
            IsReportedOutDated = isReportDate;
        }
    }
}
