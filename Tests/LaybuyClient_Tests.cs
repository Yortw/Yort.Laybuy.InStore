using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yort.Laybuy.InStore;

namespace Tests
{
	[TestClass]
	public class LaybuyClient_Integration_Tests
	{
		[TestCategory("Integration")]
		[TestMethod]
		public async Task Sandbox_Payment_FullOrderLifecycleTest()
		{
			//This is an integration test, it requires user interaction to complete.
			//You need to set the environment variable Yort_Laybuy_InStore_TestMobileNumber to a valid mobile phone number.
			//It will create an order in the Laybuy sandbox environment
			//Poll until the order reaches success or declined status (requires user to login to sandbox portal and accept payment)
			//Refund the order in full

			var client = CreateClient();

			// Create the 'order'
			var createRequest = new CreateOrderRequest() 
			{ 
				Amount = 10, 
				MerchantReference = System.Guid.NewGuid().ToString(),
				Customer = new RequestLaybuyCustomer()
				{
					Phone = Environment.GetEnvironmentVariable("Yort_Laybuy_InStore_TestMobileNumber")
				}
			};
			System.Diagnostics.Trace.WriteLine("Merchant Ref: " + createRequest.MerchantReference);

			var result = await client.Create(createRequest).ConfigureAwait(false);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Result, LaybuyStatus.Success);
			Assert.IsFalse(String.IsNullOrWhiteSpace(result.Token));

			//Poll until 'order' reaches a final status
			var done = false;
			var started = DateTime.Now;
			var statusRequest = new OrderStatusRequest() { MerchantReference = createRequest.MerchantReference };
			OrderStatusResponse status = null;
			while (!done)
			{
				await Task.Delay(1000);
				status = await client.GetStatus(statusRequest).ConfigureAwait(false);
				done = status.Result == LaybuyStatus.Success || status.Result == LaybuyStatus.Cancelled;
				if (DateTime.Now.Subtract(started).TotalMinutes > 5) throw new TimeoutException("Order not accepted after 5 minutes.");
			}

			Assert.AreEqual(status.Result, LaybuyStatus.Success);
			System.Diagnostics.Trace.WriteLine("Order Id: " + status.OrderId);
			System.Diagnostics.Trace.WriteLine("Payment token: " + status.Token);

			//Refund the order
			var refundRequest = new RefundRequest() { RefundReference = System.Guid.NewGuid().ToString(), OrderId = status.OrderId, Amount = status.Amount, Note = "Test refund" };
			System.Diagnostics.Trace.WriteLine("Refund Ref: " + refundRequest.RefundReference);
			var refundResult = await client.Refund(refundRequest).ConfigureAwait(false);
			Assert.IsNotNull(refundResult);
			Assert.AreEqual(refundResult.Result, LaybuyStatus.Success);
		}

		private Yort.Laybuy.InStore.ILaybuyClient CreateClient()
		{
			var settings = new LaybuyClientConfiguration
			(
				new LaybuyCredentials
				(
					Environment.GetEnvironmentVariable("Yort_Laybuy_InStore_SandboxMerchantId"), 
					Environment.GetEnvironmentVariable("Yort_Laybuy_InStore_SandboxApiKey")
				)
			)
			{
				Environment = LaybuyEnvironment.Sandbox,
				DefaultBranch = "Test Branch",
				DefaultOrigin = "POS" 				
			};

			return new LaybuyClient(settings);
		}
	}
}
