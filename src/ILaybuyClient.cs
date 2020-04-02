using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Interface for components that actually communicate with the Laybuy API. Provided to enable mocking/stubbing/faking in test scenarios.
	/// </summary>
	/// <remarks>
	/// <para>Application code should declare references to hold <see cref="LaybuyClient"/> instance as <see cref="ILaybuyClient"/> to help enable unit testing.</para>
	/// </remarks>
	public interface ILaybuyClient : IDisposable
	{
		/// <summary>
		/// Creates a new Laybuy (order) at Laybuy.
		/// </summary>
		/// <param name="request">A <see cref="CreateOrderRequest"/> with details of the Laybuy to create.</param>
		/// <returns>A <see cref="CreateOrderResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="LaybuyApiException">Thrown if Laybuy sends a valid response but the response itself indicates an error. See the exception's message property for details.</exception>
		/// <seealso cref="GetOrder(OrderRequest)"/>
		/// <seealso cref="Cancel(CancelOrderRequest)"/>
		/// <seealso cref="Refund(RefundRequest)"/>
		Task<CreateOrderResponse> Create(CreateOrderRequest request);

		/// <summary>
		/// Gets the full details of a Laybuy order previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="OrderRequest"/> with details of the Laybuy order to retrieve the status of.</param>
		/// <remarks>
		/// <para>This method will only return valid details for a completed order, orders in any other status will return an error indicating the order is not found. This makes it 
		/// unsuitable for status checking/polling. See <see cref="GetOrderStatus(OrderStatusRequest)"/>.</para>
		/// </remarks>
		/// <returns>A <see cref="OrderResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="LaybuyApiException">Thrown if Laybuy sends a valid response but the response itself indicates an error. See the exception's message property for details.</exception>
		/// <see cref="GetOrderStatus(OrderStatusRequest)"/>
		Task<OrderResponse> GetOrder(OrderRequest request);

		/// <summary>
		/// Gets the status of a Laybuy order previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="OrderStatusRequest"/> with details of the Laybuy order to retrieve the status of.</param>
		/// <remarks>
		/// <para>This method will only return the status of the order, and an error message if appropriate. Once this method returns a <see cref="LaybuyOrderStatus.Completed"/> 
		/// result the full details of the order can be retrived using <see cref="GetOrder(OrderRequest)"/>.</para>
		/// </remarks>
		/// <returns>A <see cref="OrderStatusResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="LaybuyApiException">Thrown if Laybuy sends a valid response but the response itself indicates an error. See the exception's message property for details.</exception>
		/// <seealso cref="GetOrder(OrderRequest)"/>
		Task<OrderStatusResponse> GetOrderStatus(OrderStatusRequest request);

		/// <summary>
		/// Refunds an amount of money against a Laybuy previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="RefundRequest"/> with details of the refund to make.</param>
		/// <returns>A <see cref="RefundResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="LaybuyApiException">Thrown if Laybuy sends a valid response but the response itself indicates an error. See the exception's message property for details.</exception>
		Task<RefundResponse> Refund(RefundRequest request);

		/// <summary>
		/// Cancels a Laybuy previously created via the <see cref="Create(CreateOrderRequest)"/> method.
		/// </summary>
		/// <remarks>
		/// <para>An error will be returned if the Laybuy is already confirmed.</para>
		/// </remarks>
		/// <param name="request">A <see cref="CancelOrderRequest"/> instance containing details of the Laybuy to cancel.</param>
		/// <returns>A <see cref="CancelOrderResponse"/> indicating success if the Laybuy was cancelled ok, otherwise error details.</returns>
		/// <seealso cref="Create(CreateOrderRequest)"/>
		Task<CancelOrderResponse> Cancel(CancelOrderRequest request);

	}
}
