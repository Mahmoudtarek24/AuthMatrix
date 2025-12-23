using System.ComponentModel.DataAnnotations;

namespace AuthMatrix.Dto_s
{
	public class CreateGroupDto
	{
		[Required]
		public string Name { get; set; }
		[Range(1, 10000)]	
		public int Priority { get; set; }
	}
}
