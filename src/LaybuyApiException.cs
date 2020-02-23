using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents an exception thrown when a Laybuy API returns a non-successful result.
	/// </summary>
	[Serializable]
	public class LaybuyApiException : Exception
	{
		/// <summary>
		/// Default constructor. Not recommended, used <see cref="LaybuyApiException(String)"/>.
		/// </summary>
		public LaybuyApiException() { }
		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="message">The error message associated with the exception.</param>
		public LaybuyApiException(string message) : base(message) { }
		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="message">The error message associated with the exception.</param>
		/// <param name="inner">Another exception wrapped by this one.</param>
		public LaybuyApiException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Deserialisation constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected LaybuyApiException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
