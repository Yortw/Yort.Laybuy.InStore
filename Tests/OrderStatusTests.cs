using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Laybuy.InStore.Tests
{
	[TestClass]
	public class OrderStatus_Tests
	{
		[TestMethod]
		public void Waiting_IsPending_NotFailed_NotApproved()
		{
			IsOnlyPending(LaybuyOrderStatus.Waiting);
		}

		[TestMethod]
		public void Processing_IsPending_NotFailed_NotApproved()
		{
			IsOnlyPending(LaybuyOrderStatus.Processing);
		}

		[TestMethod]
		public void Completed_NotPending_NotFailed_IsApproved()
		{
			IsOnlyApproved(LaybuyOrderStatus.Completed);
		}

		[TestMethod]
		public void Cancelled_NotPending_IsFailed_NotApproved()
		{
			IsOnlyFailed(LaybuyOrderStatus.Cancelled);
		}

		[TestMethod]
		public void Declined_NotPending_IsFailed_NotApproved()
		{
			IsOnlyFailed(LaybuyOrderStatus.Declined);
		}

		[TestMethod]
		public void Error_NotPending_IsFailed_NotApproved()
		{
			IsOnlyFailed(LaybuyOrderStatus.Error);
		}

		[TestMethod]
		public void Expired_NotPending_IsFailed_NotApproved()
		{
			IsOnlyFailed(LaybuyOrderStatus.Expired);
		}


		private void IsOnlyPending(string status)
		{
			Assert.IsTrue(LaybuyOrderStatus.IsPendingStatus(status));
			Assert.IsFalse(LaybuyOrderStatus.IsFailed(status));
			Assert.IsFalse(LaybuyOrderStatus.IsApproved(status));
		}

		private void IsOnlyApproved(string status)
		{
			Assert.IsFalse(LaybuyOrderStatus.IsPendingStatus(status));
			Assert.IsFalse(LaybuyOrderStatus.IsFailed(status));
			Assert.IsTrue(LaybuyOrderStatus.IsApproved(status));
		}

		private void IsOnlyFailed(string status)
		{
			Assert.IsFalse(LaybuyOrderStatus.IsPendingStatus(status));
			Assert.IsTrue(LaybuyOrderStatus.IsFailed(status));
			Assert.IsFalse(LaybuyOrderStatus.IsApproved(status));
		}

	}
}
