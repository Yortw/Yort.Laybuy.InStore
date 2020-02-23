using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ladon;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides access to the REST endpoints exposed by Laybuy API using an idiomatic .Net, object-oriented model.
	/// </summary>
	public sealed class LaybuyClient : ILaybuyClient
	{

		#region Fields

		private readonly HttpClient _Client;
		private readonly LaybuyClientConfiguration _Settings;
		private bool _IsDisposed;

		#endregion

		#region Constructors

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="settings">A <see cref="LaybuyClientConfiguration"/> instance containing configuration details.</param>
		public LaybuyClient(LaybuyClientConfiguration settings)
		{
			settings.GuardNull(nameof(settings));
			settings.Credentials.GuardNull(nameof(settings), nameof(settings.Credentials));
			settings.Credentials?.MerchantId?.GuardNullOrWhiteSpace(nameof(settings), "Credentials.MerchantId");
			settings.Credentials?.ApiKey?.GuardNullOrWhiteSpace(nameof(settings), "Credentials.ApiKey");

			_Settings = settings;

			Newtonsoft.Json.JsonConvert.DefaultSettings = () =>
			{
				var retVal = new Newtonsoft.Json.JsonSerializerSettings
				{
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
				};
				return retVal;
			};

			try
			{
				_Client = settings?.HttpClientFactory?.Invoke() ?? CreateDefaultHttpClient();
				_Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", GetBasicAuthParameter());
			}
			catch
			{
				_Client?.Dispose();

				throw;
			}
		}

		#endregion

		#region ILaybuyClient Members

		/// <summary>
		/// Cancels a Laybuy previously created via the <see cref="Create(CreateOrderRequest)"/> method.
		/// </summary>
		/// <remarks>
		/// <para>An error will be returned if the Laybuy is already confirmed.</para>
		/// </remarks>
		/// <param name="request">A <see cref="CancelOrderRequest"/> instance containing details of the Laybuy to cancel.</param>
		/// <returns>A <see cref="CancelOrderResponse"/> indicating success if the Laybuy was cancelled ok, otherwise error details.</returns>
		/// <seealso cref="Create(CreateOrderRequest)"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <see cref="CancelOrderRequest.Token"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if <see cref="CancelOrderRequest.Token"/> is an empty string or only whitespace.</exception>
		public async Task<CancelOrderResponse> Cancel(CancelOrderRequest request)
		{
			request.GuardNull(nameof(request));
			request.Token.GuardNullOrWhiteSpace("request.Token");

			return await GetAsync<CancelOrderResponse>("order/cancel/" + request.Token).ConfigureAwait(false);
		}

		/// <summary>
		/// Creates a new order at Laybuy.
		/// </summary>
		/// <param name="request">A <see cref="CreateOrderRequest"/> with details of the order to create.</param>
		/// <remarks>
		/// <para>If <see cref="CreateOrderRequest.OriginData"/> is null and <see cref="LaybuyClientConfiguration.CallbackUrlTemplate"/> and/or 
		/// <see cref="LaybuyClientConfiguration.DefaultBranch"/> are non empty in the settings object used to construct this client then a 
		/// new <see cref="StandardOriginData"/> instance is automatically created with appropriate values and applied to the request. If no default 
		/// value can be applied then <see cref="System.ArgumentNullException"/> is thrown.</para>
		/// </remarks>
		/// <returns>A <see cref="CreateOrderResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if request, <see cref="CreateOrderRequest.Origin"/>, <see cref="CreateOrderRequest.OriginData"/>, <see cref="CreateOrderRequest.Customer"/> or <see cref="CreateOrderRequest.MerchantReference"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if request, <see cref="CreateOrderRequest.Origin"/>, <see cref="CreateOrderRequest.OriginData"/>, <see cref="CreateOrderRequest.MerchantReference"/> or <see cref="RequestLaybuyCustomer.Phone"/> is an empty string or only whitespace. Also thrown if <see cref="CreateOrderRequest.Amount"/> is zero or negative.</exception>
		public async Task<CreateOrderResponse> Create(CreateOrderRequest request)
		{
			return await PostAsync<CreateOrderRequest, CreateOrderResponse>(request, "order/create").ConfigureAwait(false);
		}

		/// <summary>
		/// Gets the status of a Laybuy previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="OrderStatusRequest"/> with details of the Laybuy to retrieve the status of.</param>
		/// <returns>A <see cref="OrderStatusResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="System.ArgumentException">Thrown if <see cref="OrderStatusRequest.MerchantReference"/> and <see cref="OrderStatusRequest.OrderId"/> are both null, empty or whitespace. Also thrown if <see cref="OrderStatusRequest.OrderId"/> is zero or negative.</exception>
		public async Task<OrderStatusResponse> GetStatus(OrderStatusRequest request)
		{
			request.GuardNull(nameof(request));

			if (!String.IsNullOrWhiteSpace(request.MerchantReference))
				return await GetAsync<OrderStatusResponse>("order/merchant/" + request.MerchantReference).ConfigureAwait(false);

			return await GetAsync<OrderStatusResponse>("order/" + request.OrderId).ConfigureAwait(false);
		}

		/// <summary>
		/// Refunds an amount of money against a Laybuy previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="RefundRequest"/> with details of the refund to make.</param>
		/// <returns>A <see cref="RefundResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if <see cref="RefundRequest.RefundReference"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if <see cref="RefundRequest.OrderId"/> or <see cref="RefundRequest.Amount"/> are zero or negative. Also thrown if <see cref="RefundRequest.RefundReference"/> is empty or whitespace.</exception>
		public Task<RefundResponse> Refund(RefundRequest request)
		{
			return PostAsync<RefundRequest, RefundResponse>(request, "order/refund");
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Disposes this instance and all internal resources.
		/// </summary>
		public void Dispose()
		{
			if (!_IsDisposed)
			{
				_Client?.Dispose();
				_IsDisposed = true;
			}
		}

		#endregion

		#region Private Methods

		private string GetBasicAuthParameter()
		{
			return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(_Settings.Credentials.MerchantId + ":" + _Settings.Credentials.ApiKey));
		}

		private static HttpClient CreateDefaultHttpClient()
		{
			HttpClientHandler? handler = null;
			HttpClient? retVal = null;
			try
			{
				handler = new HttpClientHandler();
				if (handler.SupportsAutomaticDecompression)
					handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;

				retVal = new HttpClient(handler);
				retVal.DefaultRequestHeaders.UserAgent.ParseAdd("(Yort.Laybuy.InStore; en-nz) Yort.Laybuy.InStore/1.0");

				return retVal;
			}
			catch
			{
				handler?.Dispose();
				retVal?.Dispose();

				throw;
			}
		}

		private static HttpContent ToHttpContent<T>(T request)
		{
			return new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.None));
		}

		private static async Task<T> DeserialiseResponse<T>(HttpResponseMessage response)
		{
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			try
			{
				return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
			}
			catch (Newtonsoft.Json.JsonException je)
			{
				je.Data["ResponseContentType"] = (response?.Content?.Headers?.ContentType?.MediaType ?? String.Empty);
				je.Data["ResponseContent"] = content;
				throw;
			}
		}

		private async Task<TResponseType> PostAsync<TRequestType, TResponseType>(TRequestType? request, string relativePath) where TRequestType : LaybuyRequestBase where TResponseType : LaybuyApiResponseBase
		{
			request.GuardNull(nameof(request));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			request.SetDefaults(_Settings);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			request.Validate();

			if (_IsDisposed) throw new ObjectDisposedException(nameof(LaybuyClient));

			var content = ToHttpContent(request);
			var uri = new System.Uri(_Settings.RootUri, relativePath);

			var response = await _Client.PostAsync(uri, content).ConfigureAwait(false);
			var retVal = await DeserialiseResponse<TResponseType>(response).ConfigureAwait(false);

			if (retVal.Result == LaybuyStatus.Error)
				throw new LaybuyApiException(retVal.Error ?? ErrorMessages.UnspecifiedError);

			return retVal;
		}

		private async Task<TResponseType> GetAsync<TResponseType>(string relativePath)
		{
			var response = await _Client.GetAsync(new System.Uri(_Settings.RootUri, relativePath)).ConfigureAwait(false);
			return await DeserialiseResponse<TResponseType>(response).ConfigureAwait(false);
		}

		#endregion

	}
}
