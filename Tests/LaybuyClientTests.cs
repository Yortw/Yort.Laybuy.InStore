using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class LaybuyClient_Tests
	{

		[TestMethod]
		public void Disposes_Successfully()
		{
			using (var client = new LaybuyClient(new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))))
			{
			}
		}

		[TestMethod]
		public void Disposes_Successfully_MultipleTimes()
		{
			using (var client = new LaybuyClient(new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))))
			{
				client.Dispose();
				client.Dispose();
			}
		}

		[TestMethod]
		public void Throws_On_Post_Request_After_Dispose()
		{
			using (var client = new LaybuyClient(new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))))
			{
				client.Dispose();

				Assert.ThrowsExceptionAsync<ObjectDisposedException>
				(
					async () =>
					{
						_ = await client.Refund(new RefundRequest() { Amount = 10, Note = "Test", OrderId = 1234, RefundMerchantReference = System.Guid.NewGuid().ToString() });
					}
				);
			}
		}

		[TestMethod]
		public void Throws_On_Get_Request_After_Dispose()
		{
			using (var client = new LaybuyClient(new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))))
			{
				client.Dispose();

				Assert.ThrowsExceptionAsync<ObjectDisposedException>
				(
					async () =>
					{
						_ = await client.GetOrderStatus(new OrderStatusRequest() { MerchantReference = System.Guid.NewGuid().ToString() });
					}
				);
			}
		}

	}

}
