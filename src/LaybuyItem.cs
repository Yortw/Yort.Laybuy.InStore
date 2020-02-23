﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Yort.Laybuy.InStore
{
	/// <summary>
	/// Represents an item purchased via Laybuy.
	/// </summary>
	/// <seealso cref="CreateOrderRequest.Items"/>
	public class LaybuyItem
	{
		/// <summary>
		/// The merchant's unique identifier (id, PLU/SKU, barcode, etc.) for the product.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }
		/// <summary>
		/// The description of the product.
		/// </summary>
		[JsonProperty("description")]
		public string? Description { get; set; }
		/// <summary>
		/// The quantity of the item purchased.
		/// </summary>
		[JsonProperty("quantity")]
		public int Quantity { get; set; }
		/// <summary>
		/// The unit price of the product, in the currency specified on the <seealso cref="CreateOrderRequest"/>.
		/// </summary>
		[JsonProperty("price")]
		public decimal Price { get; set; }
	}
}
