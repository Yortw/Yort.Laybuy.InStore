using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents the arguments passed to <see cref="ILaybuyClient.Refund(RefundRequest)"/>.
	/// </summary>
	/// <seealso cref="RefundResponse"/>
	/// <seealso cref="ILaybuyClient.Refund(RefundRequest)"/>
	public class RefundRequest : LaybuyRequestBase
	{
		/// <summary>
		/// The unique id of the Laybuy order to refund against.
		/// </summary>
		[JsonProperty("orderId")]
		public long OrderId { get; set; }
		/// <summary>
		/// A unique client generated reference for the refund request, used to ensure idempotency.
		/// </summary>
		[JsonProperty("refundReference")]
		public string? RefundReference { get; set; }
		/// <summary>
		/// The amount of the refund.
		/// </summary>
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// An optional, humand readable note sent to the recipient of the refund.
		/// </summary>
		[JsonProperty("note")]
		public string? Note { get; set; }

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
		/// <exception cref="NotImplementedException"></exception>
		/// <remarks>
		/// Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.
		/// </remarks>
		protected internal override void Validate()
		{
			RefundReference.GuardNullOrWhiteSpace(nameof(RefundReference));
			OrderId.GuardZeroOrNegative(nameof(OrderId));
			Amount.GuardZeroOrNegative(nameof(Amount));
		}

	}
}
