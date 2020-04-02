using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Provides properties for values that (can) appear on all (any) API responses.
	/// </summary>
	public abstract class LaybuyApiResponseBase
	{
		/// <summary>
		/// The result of the API call, usually one of the <see cref="LaybuyStatus"/> for success or failure.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("result")]
		public string? Result { get; set; }
		/// <summary>
		/// A human readable description of the error that occurred.
		/// </summary>
		[ExcludeFromCodeCoverage]
		[JsonProperty("error")]
		public string? Error { get; set; }
	}
}
