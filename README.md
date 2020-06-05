# Yort.Laybuy.InStore

# Status
![Yort.Laybuy.InStore.Build](https://github.com/Yortw/Yort.Laybuy.InStore/workflows/Yort.Laybuy.InStore.Build/badge.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) [![Coverage Status](https://coveralls.io/repos/github/Yortw/Yort.Laybuy.InStore/badge.svg?branch=master)](https://coveralls.io/github/Yortw/Yort.Laybuy.InStore?branch=master)

# Documentation

See the [Laybuy API documentation](https://integrations.laybuy.com/reference) for the low level details and what the API is capable of.

For [getting started](https://yortw.github.io/Yort.Laybuy.InStore/docs/api/quickstart.html), samples and API reference for this library [see the docs](https://yortw.github.io/Yort.Laybuy.InStore/docs/articles/intro.html)

### If you really want a sample right here:

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
