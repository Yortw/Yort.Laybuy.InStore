# Yort.Laybuy.InStore Quick Start

## Creating a Laybuy Order (Taking Payment)
### Basic Process
1. Create an instance of [LaybuyClient](Yort.Laybuy.InStore.LaybuyClient.html)
2. Call [LaybuyClient.Create](Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Create_Yort_Laybuy_InStore_CreateOrderRequest_)
3. If the response from of step 2 has a success status, use [LaybuyClient.GetStatus](Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_GetStatus_Yort_Laybuy_InStore_OrderStatusRequest_) to poll at one second (or longer) intervals until an accepted, declined or cancelled status is reached. Prior to the order being accepted, declined or cancelled Laybuy will return a status with a result of *Error* and a message of *Order not found*. Handle any transient errors thrown from the polling request.
4. If you decide to timeout/give up polling, call [LaybuyClient.Cancel](Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Cancel_Yort_Laybuy_InStore_CancelOrderRequest_).
5. Once a final status is reached, you can determine whether payment was approved or not by checking the outcome. You need to record the *OrderId* of the result somewhere if you want to be able to process refunds in the future.

**NB This is only a simple/demo implementation. A production quality implementation needs much more robust logic to handle power failure/crash/network outages and other problems.**

### Add the Nuget package
Install the Nuget package;

```powershell
    PM> Install-Package Yort.Laybuy.InStore
```

[![NuGet Badge](https://buildstats.info/nuget/Yort.Laybuy.InStore)]

### Full Code Sample
A full (demo quality) code sample for creating payment, with minimum comments/noise.
See the following sections for a breakdown of what's going on and why.

```c#
    var credentials = new LaybuyCredentials("YourMerchantId", "YourApiKey");
    var settings = new LaybuyClientConfiguration(credentials)
    {
        Environment = LaybuyEnvironment.Sandbox,
        DefaultBranch = "Test Branch"
    };

    ILaybuyClient client = new LaybuyClient(settings);

    var createRequest = new CreateOrderRequest()
    {
        MerchantReference = System.Guid.NewGuid().ToString(),
        Amount = 10,
        Customer = new RequestLaybuyCustomer() { Phone = "0210000000" }
    };

    var createResponse = await client.Create(createRequest);
    if (createResponse.Result != LaybuyStatus.Success)
        throw new InvalidOperationException(createResponse.Result);

    var done = false;
    var statusRequest = new OrderStatusRequest() 
    {  MerchantReference = createRequest.MerchantReference};
    OrderStatusResponse status = null;

    while (!done)
    {
        await Task.Delay(1000); 

        status = await client.GetStatus(statusRequest);

        if (status.Result == LaybuyStatus.Cancelled) throw new OperationCanceledException();
        done = status.Result == LaybuyStatus.Success;
    }

    return status.OrderId;
```

### Configure and Create A LaybuyClient Instance
The LaybuyClient is the main class in this library to use for accessing the Laybuy API functionality. To create an instance first you must create a [LaybuyClientConfiguration](Yort.Laybuy.InStore.LaybuyClientConfiguration.html) object that tells the client how to behave. This sample configures a client for accessing the sandbox version of the API.

You'll need a merchant id and api key issued by Laybuy for the sandbox environment for this to work.

Normally you would create the Laybuy instance on start-up or first use, and then re-use it across requests instead of creating a new one each time. This allows HTTP connection pooling and improves performance. The LaybuyClient instance is thread-safe in so much as you can run multiple requests through the same instance for different payments from different threads.

```c#
    //Laybuy will issue a merchantid and api key for each environment,
    //you need to contact Laybuy to get these.
    var credentials = new LaybuyCredentials("YourMerchantId", "YourApiKey");
    var settings = new LaybuyClientConfiguration(credentials)
    {
        //The sandbox environment is used for test purposes, specify
        //the production environment to take real payments.
        Environment = LaybuyEnvironment.Sandbox,
        //Set this if you are running inside a single 
        //POS process and always operating as the same branch, 
        //otherwise leave this null set the branch property 
        //on each request
        DefaultBranch = "Test Branch" 
    };

    //We use ILaybuyClient as the type for the variable holding 
    //the LaybuyClient instance here as it makes it easier if we 
    //want a test implementation later.
    ILaybuyClient client = new LaybuyClient(settings);
```

### Call Create Order
Now we can create an order. Create a CreateOrderRequest with details of the payment to take, and pass it to the [LaybuyClient.Create](Yort.Laybuy.InStore.LaybuyClient.html#Yort_Laybuy_InStore_LaybuyClient_Create_Yort_Laybuy_InStore_CreateOrderRequest_) method.

```c#
    //We'll set the minimum required properties for this example
    var createRequest = new CreateOrderRequest()
    {
        //This is your unique reference for this payment, required
        //for status checks. Production quality
        //implementations needs to persist this somewhere and check
        //the status of incomplete payments on system restart etc.
        MerchantReference = System.Guid.NewGuid().ToString(),
        //The amount of the payment as a decimal value in the currency specified, 
        //i.e 10 here represents $NZ10 
        Amount = 10,
        //Create a customer instance and set the mobile number for Laybuy to 
        //send the payment SMS or push notification to.
        Customer = new RequestLaybuyCustomer()
        {
            Phone = "0210000000"
        }
    };

    //Note, this method could throw exceptions. Production quality code
    //would handle them appropriately. 
    var createResponse = await client.Create(createRequest);
    if (createResponse.Result != LaybuyStatus.Success)
    {
        //The payment request failed for some reason.
        //createResponse.Error contains a description of why.
        //Handle the failure here.
        throw new InvalidOperationException(createResponse.Result);
    }
```

### Poll for Status
Now we need to wait for the customer to approve or decline the payment request via the Laybuy app or website. If they approve the payment then it will be sent by Laybuy to the payment processor, and if the payment processor accepts then we'll get a success response. If either the customer or the payment processor declines the response we'll get a cancel or error response.

Unfortunately the Laybuy API doesn't return a status that clearly indicates the payment is pending. Prior to the customer accepting or declining the payment the response will be an 'Error' result with a message indicating the order was not found. This sample treats any error status 
as indicating the payment is still pending.

```c#
    var done = false;
    // Create a status request. You can create a new one on each loop or 
    // reuse the same one, which is more efficient so that's what we do here.
    var statusRequest = new OrderStatusRequest() 
    { 
        MerchantReference = createRequest.MerchantReference 
    };
    OrderStatusResponse status = null;

    //Production quality code would also have a timeout, in case the user
    //is unable to approve or cancel the order in a timely fashion at 
    //the till. There should also be an option for the operator to manually 
    //cancel, in the case where the customer's mobile phone number or payment 
    //amount has been incorrectly entered.
    while (!done)
    {
        await Task.Delay(1000); // Wait one second between poll requests
        //Request the current status
        status = await client.GetStatus(statusRequest);
        //Detect if we got a final status
        if (status.Result == LaybuyStatus.Cancelled) throw new OperationCanceledException();
        done = status.Result == LaybuyStatus.Success;
    }

    //At this point status.Result should be "Success" and the payment plan
    //should be in place. You need to keep the Laybuy order id for future use
    //with refunds.
    return status.OrderId;  
```

## Cancelling an Order
To cancel a LaybuyOrder that is still pending approval (not accepted, declined or cancelled) you will need the [payment token](Yort.Laybuy.InStore.CreateOrderResponse.html#Yort_Laybuy_InStore_CreateOrderResponse_Token) returned in the [CreateOrderResponse](Yort.Laybuy.InStore.CreateOrderResponse.html). A response with a *Result* of *Success* indicates the order was cancelled.

```c#
    var cancelResponse = await client.Cancel
    (
        new CancelOrderRequest() { Token = createResponse.Token }
    );
    if (cancelResponse.Result != LaybuyStatus.Success) 
        throw new InvalidOperationException(cancelResponse.Error);
```

## Refunding an Order
Once an order has been approved you can make one or more refunds against it, up to the total value of the original order. For each refund create a CreateRefundRequest, specify the order id that was returned when the order was created, the refund amount and a unique reference for the refund request. If you get interrupted (power failure, crash, network outage) or suffer a transient error sending a refund, resend it with the same refund reference until you get a valid response. Using the same reference on the retries should ensure idempotency and prevent multiple refunds being created if the earlier attempts did actually succeed.

```c#
    var refundRequest = new RefundRequest() 
    { 
        Amount = 10, 
        Note = "Test refund", 
        OrderId = 123, 
        RefundReference = System.Guid.NewGuid().ToString() 
    };
    var refundResponse = await client.Refund(refundRequest);
    if (refundResponse.Result != LaybuyStatus.Success) 
        throw new InvalidOperationException(refundResponse.Error);
```
