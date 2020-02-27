using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class CreateOrderRequest_ValidationTests
	{

		[TestMethod]
		public void RequiresPhone()
		{
			var request = new CreateOrderRequest()
			{
				Amount = 10,
				Origin = "POS",
				OriginData = new StandardOriginData() { Branch = "Test Branch" },
				MerchantReference = System.Guid.NewGuid().ToString()
			};

			//Throws when customer null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Throws when customer.phone null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when phone is provided
			request.Customer = new RequestLaybuyCustomer()
			{
				Phone = "02100000"
			};
			request.Validate();
		}

		[TestMethod]
		public void RequiresOrigin()
		{
			var request = new CreateOrderRequest()
			{
				Amount = 10,
				Origin = null,
				Customer = new RequestLaybuyCustomer() { Phone = "021000001" },
				OriginData = new StandardOriginData() { Branch = "Test Branch" },
				MerchantReference = System.Guid.NewGuid().ToString()
			};

			//Throws when origin is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.Origin = String.Empty;
			//Throws when origin is empty string
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when origin is not empty
			request.Origin = "POS";
			request.Validate();
		}

		[TestMethod]
		public void RequiresMerchantReference()
		{
			var request = new CreateOrderRequest()
			{
				Amount = 10,
				Customer = new RequestLaybuyCustomer() { Phone = "021000001" },
				OriginData = new StandardOriginData() { Branch = "Test Branch" },
				MerchantReference = null
			};

			//Throws when merchant ref is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			request.MerchantReference = String.Empty;
			//Throws when merchant ref is empty string
			Assert.ThrowsException<ArgumentException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when origin is not empty
			request.MerchantReference = System.Guid.NewGuid().ToString();
			request.Validate();
		}

		[TestMethod]
		public void RequiresOriginData()
		{
			var request = new CreateOrderRequest()
			{
				Amount = 10,
				Customer = new RequestLaybuyCustomer() { Phone = "021000001" },
				MerchantReference = System.Guid.NewGuid().ToString()
			};

			//Throws when merchant ref is null
			Assert.ThrowsException<ArgumentNullException>
			(
				() =>
				{
					request.Validate();
				}
			);

			//Does not throw when origin is not empty
			request.OriginData = new StandardOriginData() { Branch = "Test Branch" };
			request.Validate();
		}

	}
}
