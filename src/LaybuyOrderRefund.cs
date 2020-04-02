using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents a refund previously made against an order.
	/// </summary>
	/// <seealso cref="OrderResponse.Refunds"/>
	public class LaybuyOrderRefund
	{
		/// <summary>
		/// Gets or sets the refund identifier.
		/// </summary>
		/// <value>
		/// The refund identifier.
		/// </value>
		[JsonProperty("refundId")]
		public int RefundId { get; set; }
		/// <summary>
		/// Gets or sets the date time at which the refund occurred.
		/// </summary>
		/// <value>
		/// The date time.
		/// </value>
		[JsonProperty("dateTime")]
		public DateTime DateTime { get; set; }
		/// <summary>
		/// Gets or sets the amount of the refund.
		/// </summary>
		/// <value>
		/// The amount.
		/// </value>
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// Gets or sets the refund reference.
		/// </summary>
		/// <value>
		/// The refund reference.
		/// </value>
		[JsonProperty("refundReference")]
		public string? RefundReference { get; set; }
		/// <summary>
		/// Gets or sets the user that made the refund.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		[JsonProperty("user")]
		public string? User { get; set; }
		/// <summary>
		/// Gets or sets the user entered note of the refund.
		/// </summary>
		/// <value>
		/// The user note.
		/// </value>
		[JsonProperty("userNote")]
		public string? UserNote { get; set; }
	}
}
