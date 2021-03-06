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

    public partial class Customers
    {
        /// <summary>
        /// Initializes a new instance of the Customers class.
        /// </summary>
        public Customers()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Customers class.
        /// </summary>
        public Customers(string companyName, string customerId = default(string), string contactName = default(string), string contactTitle = default(string), string address = default(string), string city = default(string), string region = default(string), string postalCode = default(string), string country = default(string), string phone = default(string), string fax = default(string), IList<CustomerCustomerDemo> customerCustomerDemo = default(IList<CustomerCustomerDemo>), IList<Orders> orders = default(IList<Orders>))
        {
            CustomerId = customerId;
            CompanyName = companyName;
            ContactName = contactName;
            ContactTitle = contactTitle;
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            Phone = phone;
            Fax = fax;
            CustomerCustomerDemo = customerCustomerDemo;
            Orders = orders;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CustomerId")]
        public string CustomerId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ContactName")]
        public string ContactName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ContactTitle")]
        public string ContactTitle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Region")]
        public string Region { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PostalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Fax")]
        public string Fax { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CustomerCustomerDemo")]
        public IList<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }

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
            if (CustomerId != null)
            {
                if (CustomerId.Length > 5)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "CustomerId", 5);
                }
                if (CustomerId.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "CustomerId", 0);
                }
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
            if (ContactName != null)
            {
                if (ContactName.Length > 30)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "ContactName", 30);
                }
                if (ContactName.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "ContactName", 0);
                }
            }
            if (ContactTitle != null)
            {
                if (ContactTitle.Length > 30)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "ContactTitle", 30);
                }
                if (ContactTitle.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "ContactTitle", 0);
                }
            }
            if (Address != null)
            {
                if (Address.Length > 60)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Address", 60);
                }
                if (Address.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Address", 0);
                }
            }
            if (City != null)
            {
                if (City.Length > 15)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "City", 15);
                }
                if (City.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "City", 0);
                }
            }
            if (Region != null)
            {
                if (Region.Length > 15)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Region", 15);
                }
                if (Region.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Region", 0);
                }
            }
            if (PostalCode != null)
            {
                if (PostalCode.Length > 10)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "PostalCode", 10);
                }
                if (PostalCode.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "PostalCode", 0);
                }
            }
            if (Country != null)
            {
                if (Country.Length > 15)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Country", 15);
                }
                if (Country.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Country", 0);
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
            if (Fax != null)
            {
                if (Fax.Length > 24)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Fax", 24);
                }
                if (Fax.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Fax", 0);
                }
            }
            if (CustomerCustomerDemo != null)
            {
                foreach (var element in CustomerCustomerDemo)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (Orders != null)
            {
                foreach (var element1 in Orders)
                {
                    if (element1 != null)
                    {
                        element1.Validate();
                    }
                }
            }
        }
    }
}
