using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents origin data normally used with a <see cref="CreateOrderRequest"/>.
	/// </summary>
	/// <remarks>
	/// <para>Origin data required/expected can vary based on the <see cref="CreateOrderRequest.Origin"/> provided and the details of the specific
	/// implementation. This class provides the origin data used with *most* integrations.</para>
	/// </remarks>
	public class StandardOriginData
	{

		/// <summary>
		/// The absoulte URL to callback to when the payment process is complete.
		/// </summary>
		[JsonProperty("callbackUrl")]
		public Uri? CallbackUrl { get; set; }

		/// <summary>
		/// Gets or sets the name or identifier of the branch the POS making the request belongs to.
		/// </summary>
		/// <value>
		/// The branch identifier (usually name).
		/// </value>
		[JsonProperty("branch")]
		public string? Branch { get; set; }
	}
}
