using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides the credentials required to authentication to the Laybuy API.
	/// </summary>
	public class LaybuyCredentials
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LaybuyCredentials"/> class.
		/// </summary>
		/// <param name="merchantId">The merchant identifier.</param>
		/// <param name="apiKey">The API key.</param>
		public LaybuyCredentials(string merchantId, string apiKey)
		{
			this.MerchantId = merchantId;
			this.ApiKey = apiKey;
		}

		/// <summary>
		/// Gets or sets the merchant identifier.
		/// </summary>
		/// <value>
		/// The merchant identifier.
		/// </value>
		public string MerchantId { get; private set; }
		/// <summary>
		/// Gets or sets the API key.
		/// </summary>
		/// <value>
		/// The API key.
		/// </value>
		/// <remarks>
		/// <para>This is a "secret" value and should be stored securely, and protected from unauthorised access.</para>
		/// </remarks>
		public string ApiKey { get; private set; }
	}
}
