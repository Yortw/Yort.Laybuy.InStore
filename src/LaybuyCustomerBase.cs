using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides details about a customer to/from Laybuy.
	/// </summary>
	/// <remarks>
	/// <para>Unfortunately the Laybuy API isn't consistent in it's format of customer entities, with the shape of the entity varying when it's used on a request 
	/// vs. a response. This class provided properties common to a Laybuy customer entity in either direction. See <see cref="RequestLaybuyCustomer"/> 
	/// and <see cref="ResponseLaybuyCustomer"/> classes for the full entities.
	/// </para>
	/// </remarks>
	/// <seealso cref="RequestLaybuyCustomer"/>
	/// <seealso cref="ResponseLaybuyCustomer"/>
	public abstract class LaybuyCustomerBase
	{
		/// <summary>
		/// The first name of the customer.
		/// </summary>
		[JsonProperty("firstname")]
		public string? FirstName { get; set; }
		/// <summary>
		/// The surname of the customer.
		/// </summary>
		[JsonProperty("lastname")]
		public string? LastName { get; set; }
		/// <summary>
		/// The email address of the customer.
		/// </summary>
		[JsonProperty("email")]
		public string? Email { get; set; }
		/// <summary>
		/// Required. The mobile phone number of the customer.
		/// </summary>
		/// <remarks>
		/// <para>The customer's mobile phone number must be provided for <see cref="CreateOrderRequest"/> requests so the can be sent the payment link via SMS.</para>
		/// </remarks>
		[JsonProperty("phone")]
		public string? Phone { get; set; }
	}
}
