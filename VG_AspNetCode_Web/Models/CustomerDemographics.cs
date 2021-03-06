﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VG_AspNetCore_Web.Models
{
    public partial class CustomerDemographics
    {
        public CustomerDemographics()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemo>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [StringLength(10)]
        public string CustomerTypeId { get; set; }
        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [InverseProperty("CustomerType")]
        public ICollection<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
    }
}