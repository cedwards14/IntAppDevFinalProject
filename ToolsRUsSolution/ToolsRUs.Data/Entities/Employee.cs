namespace ToolsRUs.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
            Rentals = new HashSet<Rental>();
            SaleRefunds = new HashSet<SaleRefund>();
            Sales = new HashSet<Sale>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "First name is a required field")]
        [StringLength(25, ErrorMessage = "First name is a maximum of 25 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field")]
        [StringLength(25, ErrorMessage = "Last name is a maximum of 25 characters")]
        public string LastName { get; set; }

        public DateTime DateHired { get; set; }

        public DateTime? DateReleased { get; set; }

        public int PositionID { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rental> Rentals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleRefund> SaleRefunds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
