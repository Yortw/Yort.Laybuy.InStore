using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represent a billing or shipping address for a <see cref="RequestLaybuyCustomer"/>.
	/// </summary>
	public class LaybuyCustomerAddress
	{
		/// <summary>
		/// The name of the person associated with the address, the person/department receiving the bill or the goods, 
		/// depending on whether this is a billing or shipping address. If not specified, the customer's name will be used.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
		/// <summary>
		/// The phone number of the contact person or department for this address.
		/// </summary>
		/// <remarks>
		/// <para>If not provided the customer's phone number is used.</para>
		/// </remarks>
		[JsonProperty("phone")]
		public string? Phone { get; set; }

		/// <summary>
		/// The first line of address for the customer.
		/// </summary>
		[JsonProperty("address1")]
		public string? Address1 { get; set; }
		/// <summary>
		/// The second line of address for the customer, if any.
		/// </summary>
		[JsonProperty("address2")]
		public string? Address2 { get; set; }
		/// <summary>
		/// The suburb asssociated with the customer's address.
		/// </summary>
		[JsonProperty("suburb")]
		public string? Suburb { get; set; }
		/// <summary>
		/// The city asssociated with the customer's address.
		/// </summary>
		[JsonProperty("city")]
		public string? City { get; set; }
		/// <summary>
		/// The state asssociated with the customer's address.
		/// </summary>
		[JsonProperty("state")]
		public string? State { get; set; }
		/// <summary>
		/// The post code asssociated with the customer's address.
		/// </summary>
		[JsonProperty("postcode")]
		public string? Postcode { get; set; }
		/// <summary>
		/// The country asssociated with the customer's address.
		/// </summary>
		[JsonProperty("country")]
		public string? Country { get; set; }
	}
}
