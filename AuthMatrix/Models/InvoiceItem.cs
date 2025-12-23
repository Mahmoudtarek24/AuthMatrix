namespace AuthMatrix.Models
{
	public class InvoiceItem
	{
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; }

		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
