namespace ToolsRUs.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RentalEquipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RentalEquipment()
        {
            RentalDetails = new HashSet<RentalDetail>();
        }

        public int RentalEquipmentID { get; set; }

        [Required(ErrorMessage = "Description is a required field")]
        [StringLength(50, ErrorMessage = "Description is a maximum of 50 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Model number is a required field")]
        [StringLength(15, ErrorMessage = "Model number is a maximum of 15 characters")]
        public string ModelNumber { get; set; }

        [Required(ErrorMessage = "Serial number is a required field")]
        [StringLength(20, ErrorMessage = "Serial number is a maximum of 20 characters")]
        public string SerialNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal DailyRate { get; set; }

        [Required(ErrorMessage = "Condition is a required field")]
        [StringLength(100, ErrorMessage = "condition is a maximum of 100 characters")]
        public string Condition { get; set; }

        public bool Available { get; set; }

        public bool Retired { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RentalDetail> RentalDetails { get; set; }
    }
}
