using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents the response from a <see cref="ILaybuyClient.Create(CreateOrderRequest)"/>.
	/// </summary>
	/// <seealso cref="CreateOrderRequest"/>
	/// <seealso cref="ILaybuyClient.Create(CreateOrderRequest)"/>
	public class CreateOrderResponse : LaybuyApiResponseBase
	{
		/// <summary>
		/// The Laybuy payment token for the payment. Populated when the result is SUCCESS.
		/// </summary>
		/// <seealso cref="LaybuyStatus"/>
		[JsonProperty("token")]
		public string? Token { get; set; }
		/// <summary>
		/// The URL to use that begins the Laybuy payment process. Populated when the result is SUCCESS.
		/// </summary>
		/// <seealso cref="LaybuyStatus"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[JsonProperty("paymentUrl")]
		public string? PaymentUrl { get; set; }
	}
}
