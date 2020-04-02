using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides a set of known order 'statuses' as returned when using the <see cref="LaybuyClient.GetOrder(OrderRequest)"/> with a merchant reference.
	/// </summary>
	public static class LaybuyOrderStatus
	{
		/// <summary>
		/// The customer or merchant cancelled the order.
		/// This should be treated as a failure case, and a new order placed if payment via Laybuy is still required.
		/// </summary>
		public const string Cancelled = "CANCELLED";

		/// <summary>
		/// The order has been fully processed and payment approved. This is the ultimate success status for a Laybuy order.
		/// </summary>
		public const string Completed = "COMPLETED";

		/// <summary>
		/// The order was declined by Laybuy or the payment processor (i.e exceeded credit limit, insufficient funds, expired card etc).
		/// This should be treated as a failure case, and a new order placed (with any account/card issue corrected first) if payment via Laybuy is still required.
		/// </summary>
		public const string Declined = "DECLINED";

		/// <summary>
		/// The order expired before being completed. Typically orders expire ten minutes after the link is sent to the customer.
		/// This should be treated as a failure case, and a new order placed if payment via Laybuy is still required.
		/// </summary>
		public const string Expired = "EXPIRED";

		/// <summary>
		/// Some kind of error occurred, additional information about the error should be included in the response. 
		/// This should be treated as a failure case, and a new order placed (with any corrected input if required) if payment via Laybuy is still required.
		/// </summary>
		public const string Error = "ERROR";

		/// <summary>
		/// Layby has received order approval from the customer and is processing the payment, no work is required from the merchant/integration except to keep polling.
		/// This status should be treating as a 'pending' and the status rechecked in another 5-10 seconds until a non-pending status occurs.
		/// </summary>
		public const string Processing = "PROCESSING";

		/// <summary>
		/// Layby is awaiting order approval from the customer. This can take some time, especially if the customer has to sign up to Laybuy first etc.
		/// This status should be treating as a 'pending' and the status rechecked in another 5-10 seconds until a non-pending status occurs.
		/// </summary>
		public const string Waiting = "WAITING";

		/// <summary>
		/// Determines whether if <paramref name="status"/> indicating a pending status and status polling should continue at the next interval.
		/// </summary>
		/// <param name="status">The status to check.</param>
		/// <returns>
		///   <c>true</c> if <paramref name="status"/> has a case insensitive match to <see cref="Processing"/> or <see cref="Waiting"/>; otherwise, <c>false</c>.
		/// </returns>
		/// <seealso cref="IsFailed(string)"/>
		/// <seealso cref="IsApproved(string)"/>
		public static bool IsPendingStatus(string status)
		{
			return String.Equals(status, Processing, StringComparison.OrdinalIgnoreCase) || String.Equals(status, Waiting, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Determines whether the specified status indicates a final, non-pending state, that has not resulted in successful payment/an order being created.
		/// </summary>
		/// <param name="status">The status to check.</param>
		/// <returns>
		///   <c>true</c> if the specified status is not any of the pending or approved statuses; otherwise, <c>false</c>.
		/// </returns>
		/// <seealso cref="IsPendingStatus(string)"/>
		/// <seealso cref="IsApproved(string)"/>
		public static bool IsFailed(string status)
		{
			return !IsPendingStatus(status) && !IsApproved(status);
		}

		/// <summary>
		/// Determines whether the specified status indicates an approved/completed payment/order.
		/// </summary>
		/// <param name="status">The status to check.</param>
		/// <returns>
		///   <c>true</c> if <paramref name="status"/> is a case insensitive match to <see cref="Completed"/>; otherwise, <c>false</c>.
		/// </returns>
		/// <seealso cref="IsPendingStatus(string)"/>
		/// <seealso cref="IsFailed(string)"/>
		public static bool IsApproved(string status)
		{
			return String.Equals(status, Completed, StringComparison.OrdinalIgnoreCase);
		}

	}
}
