using System;
using System.Collections.Generic;
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
		[JsonProperty("refundId")]
		public long RefundId { get; set; }
		/// <summary>
		/// The merchant reference of the Laybuy order that was refunded against.
		/// </summary>
		[JsonProperty("merchantReference")]
		public string? MerchantReference { get; set; }
	}
}
