using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
		[ExcludeFromCodeCoverage]
		public int RefundId { get; set; }
		/// <summary>
		/// Gets or sets the date time at which the refund occurred.
		/// </summary>
		/// <value>
		/// The date time.
		/// </value>
		[ExcludeFromCodeCoverage]
		[JsonProperty("dateTime")]
		public DateTime DateTime { get; set; }
		/// <summary>
		/// Gets or sets the amount of the refund.
		/// </summary>
		/// <value>
		/// The amount.
		/// </value>
		[ExcludeFromCodeCoverage]
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// Gets or sets the refund reference.
		/// </summary>
		/// <value>
		/// The refund reference.
		/// </value>
		[ExcludeFromCodeCoverage]
		[JsonProperty("refundReference")]
		public string? RefundReference { get; set; }
		/// <summary>
		/// Gets or sets the user that made the refund.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		[ExcludeFromCodeCoverage]
		[JsonProperty("user")]
		public string? User { get; set; }
		/// <summary>
		/// Gets or sets the user entered note of the refund.
		/// </summary>
		/// <value>
		/// The user note.
		/// </value>
		[ExcludeFromCodeCoverage]
		[JsonProperty("userNote")]
		public string? UserNote { get; set; }
	}
}
