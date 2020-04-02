using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides various constant values used by/with Laybuy.
	/// </summary>
	public static class LaybuyStatus
	{
		/// <summary>
		/// Indicates a successful call to the Laybuy API (see	<see cref="LaybuyApiResponseBase.Result"/>).
		/// </summary>
		public const string Success = "SUCCESS";
		/// <summary>
		/// Indicates an unsuccessful call to the Laybuy API (see	<see cref="LaybuyApiResponseBase.Result"/>).
		/// </summary>
		public const string Error = "ERROR";
	}
}
