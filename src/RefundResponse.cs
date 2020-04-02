using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents the values returned by a call to <see cref="ILaybuyClient.Refund(RefundRequest)"/>.
	/// </summary>
	public class RefundResponse : LaybuyApiResponseBase
	{
		/// <summary>
		/// The unique id of the refund transaction created.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("refundId")]
		public long RefundId { get; set; }
		/// <summary>
		/// The merchant reference of the (original) Laybuy order that was refunded against.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("merchantReference")]
		public string? MerchantReference { get; set; }
	}
}
