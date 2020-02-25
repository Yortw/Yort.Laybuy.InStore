# Production Implementation Requirements

In order to provide a production quality integration with Laybuy the application developer must undertake a number of steps.

## Power Failure/Crash Recovery
A reliable integration must handle situation where the POS process dies while a Laybuy order (or refund) is pending, due to a power/hardware failure, OS restart, bug etc. In this case the usual pattern is:
* On start-up, or another suitable event, check a persistent store for a list of outstanding requests. For each pending request, recover at least the *MerchantReference*, and optionally other data such as when the request was made. 
    * If a pending order is found, begin polling for the status of that order. If the order is still pending after the first check, either continue polling or cancel immediately. When a final state is reached the POS should relate this answer back to the pending POS transaction, provide some way for the user to apply it to a new transaction or perform an automatic refund depending on how the POS is designed.
    * If a pending refund is found, retry the refund request using the same merchant reference if you want to ensure the refund occurred. If you want to check whether it occurred, perform a status poll for the order and check the list of refunds in the order status response. 
* Before sending a [Create](../api/Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Create_Yort_Laybuy_InStore_CreateOrderRequest_) or [Refund](../api/Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Refund_Yort_Laybuy_InStore_RefundRequest_) request, generate a unique MerchantReference to be used on that request and save it to persistent storage so it can be used for crash recovery (see above)
* When a POS transaction is completed (or next persisted to permanent storage) with an order or refund that has reached a final status, remove that requests reference from the persistent store of pending requests so it will no longer be rechecked on a restart (see previous steps).

## Error Handling
All methods in the [LaybuyClient](../api/Yort.Laybuy.InStore.LaybuyClient.html) class might throw exceptions. Some exceptions such as System.ArgumentException and System.ArgumentNullException occur before the request is sent, if the request data doesn't meet the minimum known requirements for Laybuy. These should be handled probably presented to the user to tell them what is missing.

In addition, any error normally throwing by the .Net HttpClient class, including System.TaskCanceledException or HttpRequestException could be re-thrown from these methods. TaskCanceled and timeout exceptions should may require special handling. For example if these exceptions are thrown during a [Create](../api/Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Create_Yort_Laybuy_InStore_CreateOrderRequest_) call and the POS is not going to try again then another call should be made to [Cancel]() to ensure the order is subsequently accepted by the user.

HttpResponseExceptions may indicate a transient error, such as the network being briefly down, or a service unavailable/too many requests response from the server. In this case the POS should wait a short period and then retry the operation.

## Polling Timeout
It's possible that due to a long network outage or similar problem that a create order request may never reach an approved or declined state and cannot be immediately cancelled. In this case the POS should have a timeout where it gives up polling. This timeout should not be too short, or should prompt the operator to ask if it should give up, as new customers  can sometimes take several minutes to complete the sign up process. It is up to the POS to decide how to behave, but the usual logic is:
* Treat the request as declined
* Store the request somewhere to be rechecked at a later time when the network is available again.
* If the customer still wants to make payment they will have to choose another payment method (since the network is not available and Laybuy cannot be used)
* On recheck:
    * If the payment is pending, cancel it.
    * If the payment is approved, refund it.
    * If the payment is cancelled or declined, there is nothing to do except remove it from the list of rechecks

## Manual Cancellation
An operator may mis-key the customer's email address or payment amount (if split payments are allowed). It's also possible the cutomer's device may run out of batter, have no network connection to receive the payment request, or get dropped and damaged etc. In any of these cases where the payment cannot be processed for a known reason the POS user should have the option to manually cancel the request without waiting for a long timeout.  This needs to use the [Cancel](../api/Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Cancel_Yort_Laybuy_InStore_CancelOrderRequest_) method to notify Laybuy of the cancellation, and only treat the payment as cancelled if the cancellation request is successful.

## Cancelling Orders
When making a [Cancel](../api/Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Cancel_Yort_Laybuy_InStore_CancelOrderRequest_) order request an error response will be returned if the order has been accepted or declined already. This can happen even if you just polled for the status of the order and were told it is pending, due to race conditions between your poll and the user or payment back end providing a final response. Therefore, any cancellation request must have it's result checked and if an error is returned the order status should be polled again to ensure it is not now accepted/declined.

