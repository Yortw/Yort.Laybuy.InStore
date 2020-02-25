using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides configuration settings for <see cref="ILaybuyClient"/> and default values for some requests..
	/// </summary>

	public class LaybuyClientConfiguration
	{
		private LaybuyEnvironment _Environment;
		private Uri? _RootUri;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LaybuyClientConfiguration(LaybuyCredentials credentials)
		{
			this.Credentials = credentials.GuardNull(nameof(credentials));

			Environment = LaybuyEnvironment.Sandbox;
		}

		/// <summary>
		/// Gets or sets a value from the <see cref="LaybuyEnvironment"/> enum specifying whether this client connects to the production or sandboxed (test) Laybuy API.
		/// </summary>
		/// <remarks>
		/// <para>The default value is <see cref="LaybuyEnvironment.Sandbox"/> to prevent accidental live transactions. Production systems must specify <see cref="LaybuyEnvironment.Production"/> for real payments to occur.</para>
		/// </remarks>
		public LaybuyEnvironment Environment
		{
			get { return _Environment; }
			set
			{
				if (_Environment != value)
				{
					_Environment = value;
					_RootUri = GetRootUri();
				}
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="LaybuyCredentials"/> instance used to authenticate to the Laybuy API.
		/// </summary>
		public LaybuyCredentials Credentials { get; private set; }

		/// <summary>
		/// Gets or sets a string containing a template used to automatically generate callback urls for <see cref="CreateOrderRequest"/>.
		/// </summary>
		/// <remarks>
		/// <para>You can use the (case-sensitive) tag {merchantReference} in the template string, it will be replaced with the merchant reference of the request the url is generated for.</para>
		/// <para>Sample: https://www.mydomain.com/callbacks/laybuy/{merchantReference} </para>
		/// </remarks>
#pragma warning disable CA1056 // Uri properties should not be strings
		public string? CallbackUrlTemplate { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

		/// <summary>
		/// Gets or sets a function that create an <see cref="HttpClient"/> to be used to communicate with Laybuy. Can be null, in which case the system will create it's own instance.
		/// </summary>
		public Func<HttpClient>? HttpClientFactory { get; set; }

		/// <summary>
		/// Gets or sets the default <see cref="CreateOrderRequest.Origin"/> value.
		/// </summary>
		/// <value>
		/// The default origin.
		/// </value>
		/// <remarks>
		/// <para>The default value is "POS".</para>
		/// </remarks>
		public string? DefaultOrigin { get; set; }

		/// <summary>
		/// Gets or sets the default value for <see cref="StandardOriginData.Branch"/> which is applied if no origin data is specified on a <see cref="CreateOrderRequest"/>.
		/// </summary>
		/// <value>
		/// The default branch identifier (usually name).
		/// </value>
		public string? DefaultBranch { get; set; }

		internal Uri RootUri
		{
			get { return _RootUri ?? GetRootUri(); }
		}

		private Uri GetRootUri()
		{
			if (_Environment == LaybuyEnvironment.Production)
				return new Uri("https://api.laybuy.com/");

			return new Uri("https://sandbox-api.laybuy.com/");
		}
	}

	/// <summary>
	/// Represents the possible environments for Laybuy.
	/// </summary>
	public enum LaybuyEnvironment
	{
		/// <summary>
		/// Test environment.
		/// </summary>
		Sandbox = 0,
		/// <summary>
		/// Live/production environment.
		/// </summary>
		Production
	}

}
