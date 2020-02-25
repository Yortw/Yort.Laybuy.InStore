using System;
using System.Collections.Generic;
using System.Text;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides details of a request to cancel a Laybuy previously created via <see cref="ILaybuyClient.Create(CreateOrderRequest)"/>.
	/// </summary>
	public class CancelOrderRequest : LaybuyRequestBase
	{
		/// <summary>
		/// Required. The token value returned by the <see cref="ILaybuyClient.Create(CreateOrderRequest)"/> method, used to uniquely identify the Laybuy to cancel.
		/// </summary>
		public string? Token { get; set; }

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
		/// <remarks>
		/// Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.
		/// </remarks>
		protected internal override void Validate()
		{
			Token.GuardNullOrWhiteSpace(nameof(CancelOrderRequest), nameof(Token));
		}

	}
}
