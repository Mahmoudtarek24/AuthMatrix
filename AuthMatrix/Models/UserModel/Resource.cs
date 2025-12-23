namespace AuthMatrix.Models.UserModel
{
	public class Resource
	{
		public int Id { get; set; }
		public string Name { get; set; }        // Invoices, Orders
		public string Controller { get; set; }  // InvoicesController
		
		// dint need to save it , can change with versions , v1=>v2
		//public string Route { get; set; }       // /api/invoices 

		public int? ParentResourceId { get; set; }
		public Resource ParentResource { get; set; }	
		public ICollection<Resource> ChildResources { get; set; } = new List<Resource>();


		public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
	}
}
