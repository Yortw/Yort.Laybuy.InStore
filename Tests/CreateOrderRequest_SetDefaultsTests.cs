using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class CreateOrderRequest_SetDefaults_Tests
	{

		[TestMethod]
		public void DoesNothingIfSettingsNull()
		{
			var request = new OrderRequest();
			request.SetDefaults(null);
		}

		[TestMethod]
		public void SetsOriginFromDefault()
		{
			var settings = new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))
			{
				DefaultOrigin = "TestOrigin"
			};
			var request = new CreateOrderRequest() { Origin = null, OriginData = new StandardOriginData() { Branch = "Albany" } };
			request.SetDefaults(settings);

			Assert.AreEqual(settings.DefaultOrigin, request.Origin);
		}

		[TestMethod]
		public void DoesNotOverwriteExistingOrigin()
		{
			var settings = new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))
			{
				DefaultOrigin = "TestOrigin"
			};
			var request = new CreateOrderRequest() { Origin = "TestOrigin2", OriginData = new StandardOriginData() { Branch = "Albany" } };
			request.SetDefaults(settings);

			Assert.AreEqual("TestOrigin2", request.Origin);
		}

		[TestMethod]
		public void SetsOriginDataFromDefaultBranch()
		{
			var settings = new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))
			{
				DefaultOrigin = "TestOrigin",
				DefaultBranch = "Albany"
			};
			var request = new CreateOrderRequest();
			request.SetDefaults(settings);

			Assert.IsNotNull((request.OriginData as StandardOriginData).Branch);
			Assert.AreEqual(settings.DefaultBranch, (request.OriginData as StandardOriginData).Branch);
		}

		[TestMethod]
		public void DoesNotOverwriteExistingOriginData()
		{
			var settings = new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"))
			{
				DefaultOrigin = "TestOrigin",
				DefaultBranch = "Albany"
			};
			var request = new CreateOrderRequest() { Origin = "TestOrigin2", OriginData = new StandardOriginData() { Branch = "Westgate" } };
			request.SetDefaults(settings);

			Assert.IsNotNull((request.OriginData as StandardOriginData).Branch);
			Assert.AreEqual("Westgate", (request.OriginData as StandardOriginData).Branch);
		}

		[TestMethod]
		public void ThrowsIfOriginDataCannotBeSet()
		{
			var settings = new LaybuyClientConfiguration(new LaybuyCredentials("A", "A"));
			var request = new CreateOrderRequest();

			Assert.ThrowsException<ArgumentNullException>(() => request.SetDefaults(settings));
		}

	}
}