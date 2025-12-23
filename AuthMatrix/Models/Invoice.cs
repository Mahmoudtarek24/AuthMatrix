namespace AuthMatrix.Models
{
	public class Invoice
	{
		public int Id { get; set; }
		public string InvoiceNumber { get; set; }
		public decimal Amount { get; set; }
		public string Status { get; set; }       // Pending, Approved, Rejected
		public string Department { get; set; }   // Sales, IT, HR
		public int CreatedBy { get; set; }    // UserId
		public DateTime CreatedAt { get; set; }

		public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();


	}
}
