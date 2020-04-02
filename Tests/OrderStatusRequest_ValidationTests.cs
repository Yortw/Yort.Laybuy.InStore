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
		public void RequiresMerchantReferenceIsNotBlank()
		{
			var request = new OrderStatusRequest()
			{
				MerchantReference = null
			};

			//Throws when merchant reference is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Throws when order id is empty string
			request.MerchantReference = String.Empty;
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when merchante reference is nto blank.
			request.MerchantReference = "12355";
			request.Validate();
		}

	}
}
