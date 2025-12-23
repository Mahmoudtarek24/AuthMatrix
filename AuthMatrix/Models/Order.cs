namespace AuthMatrix.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
