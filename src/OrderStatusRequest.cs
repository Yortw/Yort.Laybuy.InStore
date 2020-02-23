using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents arguments passed to <see cref="ILaybuyClient.GetStatus(OrderStatusRequest)"/>.
	/// </summary>
	/// <seealso cref="OrderStatusResponse"/>
	/// <seealso cref="ILaybuyClient.GetStatus(OrderStatusRequest)"/>
	public class OrderStatusRequest : LaybuyRequestBase
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
		/// Sets any properties on this object that are null to the appropriate defaults, if possible.
		/// </summary>
		/// <param name="settings">The settings used to construct the <see cref="LaybuyClient" /> instance that is about to send this request.</param>
		protected internal override void SetDefaults(LaybuyClientConfiguration settings)
		{
		}

		/// <summary>
		/// Validates the properties for this instance are valid before sending the request to the API.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		/// <remarks>
		/// Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.
		/// </remarks>
		protected internal override void Validate()
		{
			if (OrderId == null && String.IsNullOrEmpty(MerchantReference)) throw new ArgumentException(ErrorMessages.OrderIdOrMerchantReferenceRequired);
			if (OrderId != null)
				OrderId.Value.GuardZeroOrNegative(nameof(OrderId));
		}
	}
}
