using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class RefundRequest_ValidationTests
	{

		[TestMethod]
		public void RequiresOrderId()
		{
			var request = new RefundRequest()
			{
				RefundReference = System.Guid.NewGuid().ToString(),
				Amount = 10,
				OrderId = 0,
				Note = "Test refund"
			};

			//Throws when order id zero
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.OrderId = -1;
			//Throws when order id is negative
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when order id is valid
			request.OrderId = 1234;
			request.Validate();
		}

		[TestMethod]
		public void RequiresRefundReference()
		{
			var request = new RefundRequest()
			{
				RefundReference = null,
				Amount = 10,
				OrderId = 123456,
				Note = "Test refund"
			};

			//Throws when RefundReference is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.RefundReference = String.Empty;
			//Throws when RefundReference is empty string
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when RefundReference is not empty
			request.RefundReference = System.Guid.NewGuid().ToString();
			request.Validate();
		}

		[TestMethod]
		public void RequiresPositiveAmount()
		{
			var request = new RefundRequest()
			{
				RefundReference = System.Guid.NewGuid().ToString(),
				Amount = 0,
				OrderId = 123456,
				Note = "Test refund"
			};

			//Throws when amount is zero
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.Amount = -10;
			//Throws when amount is negative
			Assert.ThrowsException<ArgumentOutOfRangeException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when amount is valid
			request.Amount = 10;
			request.Validate();
		}

	}
}
