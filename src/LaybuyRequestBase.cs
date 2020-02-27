using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Base class for request objects, used to provide some internal plumbing. Not intended for use from application code.
	/// </summary>
	public abstract class LaybuyRequestBase 
	{
		/// <summary>
		/// Validates the properties for this instance are valid before sending the request to the API.
		/// </summary>
		/// <remarks>
		/// <para>Provides simple client side validation, such as required fields beign provided and fields under maximum lengths etc.</para>
		/// </remarks>
		public abstract void Validate();

		/// <summary>
		/// Sets any properties on this object that are null to the appropriate defaults, if possible.
		/// </summary>
		/// <param name="settings">The settings used to construct the <see cref="LaybuyClient"/> instance that is about to send this request.</param>
		public abstract void SetDefaults(LaybuyClientConfiguration settings);
	}
}
