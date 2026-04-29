using AuthMatrix.Models;
using AuthMatrix.Models.UserModel;
using AuthMatrix.Services;
using Microsoft.EntityFrameworkCore;

namespace AuthMatrix.Infrastructure.Seed
{
	public static class ResourceSeedService
	{
		public static async Task SeedResourcesAsync(IServiceProvider serviceProvider)
		{
			var scope = serviceProvider.CreateScope();		
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();		
			var resources = scope.ServiceProvider.GetRequiredService<ResourceExtractorService>();		

			foreach(var resource in resources.ExtractResources())
				if(!await context.Resources.AnyAsync(e => e.Name == resource.Name))
					context.Resources.Add(new Resource
					{
						Controller = resource.Controller,
						Name = resource.Name,
					});
			
			await context.SaveChangesAsync();
		}
	}
}
