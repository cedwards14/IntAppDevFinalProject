namespace ToolsRUs.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
            StockItems = new HashSet<StockItem>();
        }

        public int VendorID { get; set; }

        [Required(ErrorMessage = "Vendor name is a required field")]
        [StringLength(100, ErrorMessage = "Vendor name is a maximum of 100 characters")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Phone is a required field")]
        [StringLength(12, ErrorMessage = "Phone is a maximum of 12 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is a required field")]
        [StringLength(30, ErrorMessage = "Address is a maximum of 30 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is a required field")]
        [StringLength(50, ErrorMessage = "City is a maximum of 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province is a required field")]
        [StringLength(2, ErrorMessage = "Province is a maximum of 2 characters")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal code is a required field")]
        [StringLength(6, ErrorMessage = "Postal code is a maximum of 6 characters")]
        public string PostalCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockItem> StockItems { get; set; }
    }
}
