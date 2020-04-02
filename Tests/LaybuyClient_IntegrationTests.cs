using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yort.Laybuy.InStore;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class LaybuyClient_Integration_Tests
	{
		[TestCategory("Integration")]
		[TestMethod]
		public async Task Sandbox_Payment_FullOrderLifecycleTest_MinimumOrder()
		{
			//This is an integration test, it requires user interaction to complete.
			//You need to set the environment variable Yort_Laybuy_InStore_TestMobileNumber to a valid mobile phone number.
			//It will create an order in the Laybuy sandbox environment
			//Poll until the order reaches success or declined status (requires user to login to sandbox portal and accept payment)
			//Refund the order in full

			var client = CreateClient();

			// Create the 'order'
			var createRequest = CreateMinimalLaybuyOrderRequest();

			await TestLaybyOrderLifecycle(client, createRequest).ConfigureAwait(false);
		}

		[TestCategory("Integration")]
		[TestMethod]
		public async Task Sandbox_Payment_FullOrderLifecycleTest_FullOrder()
		{
			//This is an integration test, it requires user interaction to complete.
			//You need to set the environment variable Yort_Laybuy_InStore_TestMobileNumber to a valid mobile phone number.
			//It will create an order in the Laybuy sandbox environment
			//Poll until the order reaches success or declined status (requires user to login to sandbox portal and accept payment)
			//Refund the order in full

			var client = CreateClient();

			// Create the 'order'
			var createRequest = CreateFullLaybuyOrderRequest();

			await TestLaybyOrderLifecycle(client, createRequest).ConfigureAwait(false);
		}

		private static async Task TestLaybyOrderLifecycle(ILaybuyClient client, CreateOrderRequest createRequest)
		{
			System.Diagnostics.Trace.WriteLine("Merchant Ref: " + createRequest.MerchantReference);

			var createOrderResponse = await client.Create(createRequest).ConfigureAwait(false);

			Assert.IsNotNull(createOrderResponse);
			Assert.AreEqual(createOrderResponse.Result, LaybuyStatus.Success);
			Assert.IsFalse(String.IsNullOrWhiteSpace(createOrderResponse.Token));
			Assert.IsTrue(String.IsNullOrWhiteSpace(createOrderResponse.Error));
			Assert.IsNotNull(createOrderResponse.PaymentUrl);

			//Poll until 'order' reaches a final status
			var done = false;
			var started = DateTime.Now;
			var statusRequest = new OrderStatusRequest() { MerchantReference = createRequest.MerchantReference };
			OrderStatusResponse statusResponse;
			while (!done)
			{
				await Task.Delay(5000); //Laybuy say poll every 5-10 seconds
				statusResponse = await client.GetOrderStatus(statusRequest).ConfigureAwait(false);
				done = !LaybuyOrderStatus.IsPendingStatus(statusResponse.Status);

				if (DateTime.Now.Subtract(started).TotalMinutes > 5) throw new TimeoutException("Order not accepted after 5 minutes.");
				if (LaybuyOrderStatus.IsFailed(statusResponse.Status))
					throw new LaybuyApiException(statusResponse.Status + " - " + statusResponse.Error);
			}

			var orderResponse = await client.GetOrder(new OrderRequest() { MerchantReference = statusRequest.MerchantReference }).ConfigureAwait(false);
			Assert.AreEqual(orderResponse.Result, LaybuyStatus.Success);
			System.Diagnostics.Trace.WriteLine("Order Id: " + orderResponse.OrderId);
			System.Diagnostics.Trace.WriteLine("Payment token: " + orderResponse.Token);
			Assert.IsNotNull(orderResponse.Refunds);
			Assert.IsFalse(String.IsNullOrEmpty(orderResponse.Currency));
			Assert.IsNotNull(orderResponse.Processed);
			Assert.IsTrue(orderResponse.OrderId > 0);
			Assert.AreEqual(orderResponse.MerchantReference, createRequest.MerchantReference);
			//TODO: Laybuy doesn't actually return this here?
			//Assert.AreEqual(orderResponse.Token, createOrderResponse.Token);

			//Refund the order
			var refundRequest = new RefundRequest() { RefundMerchantReference = System.Guid.NewGuid().ToString(), OrderId = orderResponse.OrderId, Amount = orderResponse.Amount, Note = "Test refund" };
			System.Diagnostics.Trace.WriteLine("Refund Ref: " + refundRequest.RefundMerchantReference);
			var refundResponse = await client.Refund(refundRequest).ConfigureAwait(false);
			Assert.IsNotNull(refundResponse);
			Assert.AreEqual(refundResponse.Result, LaybuyStatus.Success);
			Assert.IsTrue(refundResponse.RefundId > 0);
			Assert.AreEqual(createRequest.MerchantReference, refundResponse.MerchantReference);

			//Can get the order by order id
			var orderRequest = new OrderRequest() { OrderId = orderResponse.OrderId };
			orderResponse = await client.GetOrder(orderRequest).ConfigureAwait(false);
			Assert.AreEqual(orderResponse.Result, LaybuyStatus.Success);
			Assert.AreEqual(1, orderResponse.Refunds.Count());
		}

		private static CreateOrderRequest CreateMinimalLaybuyOrderRequest()
		{
			return new CreateOrderRequest()
			{
				Amount = 10,
				MerchantReference = System.Guid.NewGuid().ToString(),
				Customer = new RequestLaybuyCustomer()
				{
					Phone = Environment.GetEnvironmentVariable("Yort_Laybuy_InStore_TestMobileNumber")
				}
			};
		}

		private static CreateOrderRequest CreateFullLaybuyOrderRequest()
		{
			return new CreateOrderRequest()
			{
				Amount = 10,
				MerchantReference = System.Guid.NewGuid().ToString(),
				Customer = new RequestLaybuyCustomer()
				{
					FirstName = "John",
					LastName = "Smith",
					Email = "test@example.com",
					BillingAddress = new LaybuyCustomerAddress()
					{
						Address1 = "Test Address",
						Address2 = "1 Somewhere Place",
						City = "Auckland",
						Country = "New Zealand",
						Name = "Mr John Smith",
						Phone = "555-555",
						Postcode = "1010",
						State = "Auckland",
						Suburb = "Auckland Central"
					},
					ShippingAddress = new LaybuyCustomerAddress()
					{
						Address1 = "Test Address",
						Address2 = "1 Somewhere Place",
						City = "Auckland",
						Country = "New Zealand",
						Name = "Mr John Smith",
						Phone = "555-555",
						Postcode = "1010",
						State = "Auckland",
						Suburb = "Auckland Central"
					},
					Phone = Environment.GetEnvironmentVariable("Yort_Laybuy_InStore_TestMobileNumber")
				},
				Items = new List<LaybuyItem>()
				{
					new LaybuyItem() { Description = "Some Product", Id = "Sku123", Price = 5M, Quantity = 1  },
					new LaybuyItem() { Description = "Some Other Product", Id = "Sku555", Price = 2.5M, Quantity = 2  }
				},
				Currency = "NZD",
				Tax = 1.3M,
				Origin = "POS",
				OriginData = new StandardOriginData()
				{
					Branch = "Albany"
				}
			};
		}

		[TestCategory("Integration")]
		[TestMethod]
		public async Task Sandbox_Payment_CanCancel()
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

			await Task.Delay(2000).ConfigureAwait(false);
			var response = await client.Cancel(new CancelOrderRequest() { Token = result.Token }).ConfigureAwait(false);

			Assert.IsNotNull(response);
			Assert.AreEqual(response.Result, LaybuyStatus.Success);

			await Task.Delay(2000).ConfigureAwait(false);
			var statusResponse = await client.GetOrderStatus(new OrderStatusRequest() { MerchantReference = createRequest.MerchantReference }).ConfigureAwait(false);
			
			Assert.IsNotNull(statusResponse);
			Assert.AreEqual(statusResponse.Status, LaybuyOrderStatus.Cancelled);
		}

		[TestCategory("Integration")]
		[TestMethod]
		public async Task Sandbox_Payment_ThrowsLaybuyApiExceptionOnInvalidRequest()
		{
			//This is an integration test, it requires user interaction to complete.
			//You need to set the environment variable Yort_Laybuy_InStore_TestMobileNumber to a valid mobile phone number.
			//It will create an order in the Laybuy sandbox environment
			//Poll until the order reaches success or declined status (requires user to login to sandbox portal and accept payment)
			//Refund the order in full

			var client = CreateClient();

			// Create the 'order'
			var createRequest = CreateMinimalLaybuyOrderRequest();
			createRequest.Customer.Phone = "5555555555555555555555555555555";

			await Assert.ThrowsExceptionAsync<LaybuyApiException>
			(
				async () =>
				{
					await TestLaybyOrderLifecycle(client, createRequest).ConfigureAwait(false);
				}
			);
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
