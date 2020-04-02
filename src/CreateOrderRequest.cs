using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Ladon;
using System.Diagnostics.CodeAnalysis;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Contains the values/arguments to the <see cref="ILaybuyClient.Create(CreateOrderRequest)"/> call.
	/// </summary>
	/// <seealso cref="CreateOrderResponse"/>
	/// <seealso cref="ILaybuyClient.Create(CreateOrderRequest)"/>
	public class CreateOrderRequest : LaybuyRequestBase
	{
		/// <summary>
		/// Required. The amount of money to request.
		/// </summary>
		/// <remarks>
		/// <para>The total for the customer to pay. This amount is validated against the sum of the amounts in the items attribute.</para>
		/// </remarks>
		/// <seealso cref="Currency"/>
		[ExcludeFromCodeCoverage]
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
		/// <summary>
		/// Optional. The tax included in <see cref="Amount"/>.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("tax")]
		public decimal Tax { get; set; }
		/// <summary>
		/// Optional (but recommended). The currency the <see cref="Amount"/> is in.
		/// </summary>
		/// <remarks>
		/// <para>If omitted then the default currency associated with the merchant id is used. It is highly recommended a specific currency be provided rather than relying on defaults.</para>
		/// </remarks>
		/// <seealso cref="Amount"/>
		[ExcludeFromCodeCoverage]
		[JsonProperty("currency")]
		public string? Currency { get; set; }
		/// <summary>
		/// Required. A client generated reference for this request, used for idempotency.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("merchantReference")]
		public string? MerchantReference { get; set; }
		/// <summary>
		/// Required. A string identifying the calling system. Must be pre-registered with Laybuy.
		/// </summary>
		/// <remarks>
		/// <para>Uses a default of POS if not specified and no default is specified via the <see cref="LaybuyClientConfiguration.DefaultOrigin"/> value.</para>
		/// </remarks>
		[ExcludeFromCodeCoverage]
		[JsonProperty("origin")]
		public string? Origin { get; set; } = "POS";
		/// <summary>
		/// Required. An object to be serialised as json containing data specific to the <see cref="Origin"/> value provided.
		/// </summary>
		/// <seealso cref="StandardOriginData"/>
		[ExcludeFromCodeCoverage]
		[JsonProperty("originData")]
		public object? OriginData { get; set; }
		/// <summary>
		/// Required (must provide <see cref="LaybuyCustomerBase.Phone"/>). Sets or returns a <see cref="RequestLaybuyCustomer"/> instance representing details of the customer for this request.
		/// </summary>
		/// <remarks>
		/// <para>Requests made by systems for physical stores/branches *must* provide a value for the customer phone number.</para>
		/// </remarks>
		[ExcludeFromCodeCoverage]
		[JsonProperty("customer")]
		public RequestLaybuyCustomer? Customer { get; set; }

		/// <summary>
		/// Optional. Null or a collection of <see cref="LaybuyItem"/> objects being purchased. Default value is null.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[ExcludeFromCodeCoverage]
		[JsonProperty("items")]
		public IList<LaybuyItem>? Items { get; set; } = new List<LaybuyItem>();

		/// <summary>
		/// Validates the properties for this instance are valid before sending the request to the API.
		/// </summary>
		/// <remarks>
		/// Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.
		/// </remarks>
		public override void Validate()
		{
			Amount.GuardZeroOrNegative(nameof(Amount));
			MerchantReference.GuardNullOrWhiteSpace(nameof(MerchantReference));
			Origin.GuardNullOrWhiteSpace(nameof(Origin));
			OriginData.GuardNull(nameof(OriginData));
			Customer.GuardNull(nameof(Customer));
			Customer?.Phone.GuardNullOrWhiteSpace(nameof(Customer.Phone));
		}

		/// <summary>
		/// Sets any properties on this object that are null to the appropriate defaults, if possible.
		/// </summary>
		/// <param name="settings">The settings used to construct the <see cref="LaybuyClient" /> instance that is about to send this request.</param>
		public override void SetDefaults(LaybuyClientConfiguration settings)
		{
			if (settings == null) return;

			if (String.IsNullOrWhiteSpace(this.Origin))
				this.Origin = settings.DefaultOrigin.GuardNullOrWhiteSpace("request", nameof(Origin));

			if (OriginData == null)
			{
				if (!String.IsNullOrEmpty(settings.DefaultBranch))
				{
					this.OriginData = new StandardOriginData()
					{
						Branch = settings.DefaultBranch
					};
				}
				else
					this.OriginData.GuardNull(nameof(OriginData));
			}
		}
	}
}
