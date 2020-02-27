using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class OrderStatusRequest_ValidationTests
	{

		[TestMethod]
		public void RequiresOrderIdIfMerchantReferenceBlank()
		{
			var request = new OrderStatusRequest()
			{
				OrderId = null,
				MerchantReference = null
			};

			//Throws when order id is null
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Throws when order id is zero is null
			request.OrderId = 0;
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.OrderId = -1;
			//Throws when order is negative
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when order is is positive
			request.OrderId = 123456;
			request.Validate();
		}

		[TestMethod]
		public void RequiresMerchantReferenceIfOrderIdIsNull()
		{
			var request = new OrderStatusRequest()
			{
				OrderId = null,
				MerchantReference = null
			};

			//Throws when merchant reference is null
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.MerchantReference = String.Empty;
			//Throws when merchant reference is negative
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when merchant reference is not blank
			request.MerchantReference = System.Guid.NewGuid().ToString();
			request.Validate();
		}

	}
}
