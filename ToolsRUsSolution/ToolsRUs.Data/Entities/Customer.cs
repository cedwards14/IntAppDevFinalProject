namespace ToolsRUs.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Rentals = new HashSet<Rental>();
        }

        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Last Name is a required field")]
        [StringLength(50, ErrorMessage = "Last name is a maximum of 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        [StringLength(50, ErrorMessage = "First name is a maximum of 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Address is a required field")]
        [StringLength(75, ErrorMessage = "Address is a maximum of 75 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is a required field")]
        [StringLength(30, ErrorMessage = "City is a maximum of 30 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province is a required field")]
        [StringLength(2, ErrorMessage = "Province is a maximum of 2 characters")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal code is a required field")]
        [StringLength(6, ErrorMessage = "Postal code is a maximum of 6 characters")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Contact phone number is a required field")]
        [StringLength(12, ErrorMessage = "Contect phone number is a maximum of 12 characters")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Email address is a required field")]
        [StringLength(50, ErrorMessage = "Email address is a maximum of 50 characters")]
        public string EmailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
