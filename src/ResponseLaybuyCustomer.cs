using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// A customer entity as returned by the Laybuy API.
	/// </summary>
	/// <seealso cref="Yort.Laybuy.InStore.LaybuyCustomerBase" />
	/// <seealso cref="Yort.Laybuy.InStore.ResponseLaybuyCustomer"/>
	public class ResponseLaybuyCustomer : LaybuyCustomerBase
	{
		/// <summary>
		/// The unique id Laybuy knows this customer as.
		/// </summary>
		/// <remarks>
		/// <para>This value is normally returned from Laybuy, and not provided as an input to it.</para>
		/// </remarks>
		[ExcludeFromCodeCoverage]
		[JsonProperty("customerId")]
		public long? CustomerId { get; set; }


		/// <summary>
		/// The first line of address for the customer.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("address1")]
		public string? Address1 { get; set; }
		/// <summary>
		/// The second line of address for the customer, if any.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("address2")]
		public string? Address2 { get; set; }
		/// <summary>
		/// The suburb asssociated with the customer's address.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("suburb")]
		public string? Suburb { get; set; }
		/// <summary>
		/// The city asssociated with the customer's address.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("city")]
		public string? City { get; set; }
		/// <summary>
		/// The state asssociated with the customer's address.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("state")]
		public string? State { get; set; }
		/// <summary>
		/// The post code asssociated with the customer's address.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("postcode")]
		public string? Postcode { get; set; }
		/// <summary>
		/// The country asssociated with the customer's address.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("country")]
		public string? Country { get; set; }

	}
}
