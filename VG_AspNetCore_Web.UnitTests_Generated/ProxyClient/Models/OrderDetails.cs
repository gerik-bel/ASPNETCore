// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Swagger.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class OrderDetails
    {
        /// <summary>
        /// Initializes a new instance of the OrderDetails class.
        /// </summary>
        public OrderDetails()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OrderDetails class.
        /// </summary>
        public OrderDetails(int? orderId = default(int?), int? productId = default(int?), double? unitPrice = default(double?), int? quantity = default(int?), double? discount = default(double?), Orders order = default(Orders), Products product = default(Products))
        {
            OrderId = orderId;
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
            Order = order;
            Product = product;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "OrderId")]
        public int? OrderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ProductId")]
        public int? ProductId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UnitPrice")]
        public double? UnitPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Discount")]
        public double? Discount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Order")]
        public Orders Order { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Product")]
        public Products Product { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Order != null)
            {
                Order.Validate();
            }
            if (Product != null)
            {
                Product.Validate();
            }
        }
    }
}
