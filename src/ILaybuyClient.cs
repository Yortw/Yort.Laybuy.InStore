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
		/// <seealso cref="GetStatus(OrderStatusRequest)"/>
		/// <seealso cref="Cancel(CancelOrderRequest)"/>
		/// <seealso cref="Refund(RefundRequest)"/>
		Task<CreateOrderResponse> Create(CreateOrderRequest request);

		/// <summary>
		/// Gets the status of a Laybuy previously created via <see cref="Create"/>.
		/// </summary>
		/// <param name="request">A <see cref="OrderStatusRequest"/> with details of the Laybuy to retrieve the status of.</param>
		/// <returns>A <see cref="OrderStatusResponse"/> indicating the outcome of the request.</returns>
		/// <exception cref="LaybuyApiException">Thrown if Laybuy sends a valid response but the response itself indicates an error. See the exception's message property for details.</exception>
		Task<OrderStatusResponse> GetStatus(OrderStatusRequest request);

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
