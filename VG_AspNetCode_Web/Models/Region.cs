using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VG_AspNetCore_Web.Models
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territories>();
        }

        [Column("RegionID")]
        public int RegionId { get; set; }
        [Required]
        [StringLength(50)]
        public string RegionDescription { get; set; }

        [InverseProperty("Region")]
        public ICollection<Territories> Territories { get; set; }
    }
}