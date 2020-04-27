// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Swagger.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Shippers
    {
        /// <summary>
        /// Initializes a new instance of the Shippers class.
        /// </summary>
        public Shippers()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Shippers class.
        /// </summary>
        public Shippers(string companyName, int? shipperId = default(int?), string phone = default(string), IList<Orders> orders = default(IList<Orders>))
        {
            ShipperId = shipperId;
            CompanyName = companyName;
            Phone = phone;
            Orders = orders;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ShipperId")]
        public int? ShipperId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Orders")]
        public IList<Orders> Orders { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (CompanyName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "CompanyName");
            }
            if (CompanyName != null)
            {
                if (CompanyName.Length > 40)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "CompanyName", 40);
                }
                if (CompanyName.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "CompanyName", 0);
                }
            }
            if (Phone != null)
            {
                if (Phone.Length > 24)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Phone", 24);
                }
                if (Phone.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Phone", 0);
                }
            }
            if (Orders != null)
            {
                foreach (var element in Orders)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
