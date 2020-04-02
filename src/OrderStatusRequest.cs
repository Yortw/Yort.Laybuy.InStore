using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents arguments passed to <see cref="ILaybuyClient.GetOrderStatus(OrderStatusRequest)"/>.
	/// </summary>
	/// <seealso cref="OrderStatusResponse"/>
	/// <seealso cref="ILaybuyClient.GetOrderStatus(OrderStatusRequest)"/>
	public class OrderStatusRequest : LaybuyRequestBase
	{
		/// <summary>
		/// Required. The unique merchant reference of the Laybuy order to retrieve.
		/// </summary>
		[JsonProperty("merchantReference")]
		public string? MerchantReference { get; set; }

		/// <summary>
		/// Validates the properties for this instance are valid before sending the request to the API.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		/// <remarks>
		/// Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.
		/// </remarks>
		public override void Validate()
		{
			if (MerchantReference == null) throw new ArgumentNullException(ErrorMessages.MerchantReferenceRequired);
			if (String.IsNullOrEmpty(MerchantReference)) throw new ArgumentException(ErrorMessages.MerchantReferenceRequired);
		}
	}
}
