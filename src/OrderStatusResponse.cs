using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides (only) the status of a Laybuy order, returned by <see cref="ILaybuyClient.GetOrderStatus(OrderStatusRequest)"/>.
	/// </summary>
	/// <seealso cref="OrderStatusRequest"/>
	/// <seealso cref="ILaybuyClient.GetOrderStatus(OrderStatusRequest)"/>
	public class OrderStatusResponse : LaybuyApiResponseBase
	{
		/// <summary>
		/// Gets or sets the status of the order. See <see cref="LaybuyOrderStatus"/> for a list of known statuses and their meanings.
		/// </summary>
		/// <value>
		/// A string containing the status.
		/// </value>
		[JsonProperty("status")]
		public string? Status { get; set; }
	}
}
