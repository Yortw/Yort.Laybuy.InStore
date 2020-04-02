using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents arguments passed to <see cref="ILaybuyClient.GetOrder(OrderRequest)"/>.
	/// </summary>
	/// <seealso cref="OrderResponse"/>
	/// <seealso cref="ILaybuyClient.GetOrder(OrderRequest)"/>
	public class OrderRequest : LaybuyRequestBase
	{
		/// <summary>
		/// The unique Laybuy id of the Laybuy order to retrieve. Can be null if <see cref="MerchantReference"/> is provided.
		/// </summary>
		[JsonProperty("orderId")]
		public long? OrderId { get; set; }
		/// <summary>
		/// The unique merchant reference of the Laybuy order to retrieve. Can be null if <see cref="OrderId"/> is provided.
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
			if (OrderId == null && String.IsNullOrEmpty(MerchantReference)) throw new ArgumentException(ErrorMessages.OrderIdOrMerchantReferenceRequired);
			if (OrderId != null)
				OrderId.Value.GuardZeroOrNegative(nameof(OrderId));
		}
	}
}
