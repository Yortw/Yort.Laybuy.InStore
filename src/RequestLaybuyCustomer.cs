using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// A customer entity used on Laybuy API requests.
	/// </summary>
	/// <seealso cref="Yort.Laybuy.InStore.LaybuyCustomerBase" />
	/// <seealso cref="Yort.Laybuy.InStore.CreateOrderRequest"/>
	public class RequestLaybuyCustomer : LaybuyCustomerBase
	{
		/// <summary>
		/// Optional. Gets or sets the billing address.
		/// </summary>
		/// <value>
		/// The billing address.
		/// </value>
		[JsonProperty("billingAddress")]
		public LaybuyCustomerAddress? BillingAddress { get; set; }

		/// <summary>
		/// Optional. Gets or sets the shipping address.
		/// </summary>
		/// <value>
		/// The shipping address.
		/// </value>
		[JsonProperty("shippingAddress")]
		public LaybuyCustomerAddress? ShippingAddress { get; set; }
	}
}
