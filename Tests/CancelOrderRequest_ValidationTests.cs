using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class CancelOrderRequest_ValidationTests
	{

		[TestMethod]
		public void RequiresPaymentToken()
		{
			var request = new CancelOrderRequest()
			{
				Token = null
			};

			//Throws when token is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.Token = String.Empty;
			//Throws when Token is empty string
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when Token is not empty
			request.Token = System.Guid.NewGuid().ToString();
			request.Validate();
		}

	}
}
