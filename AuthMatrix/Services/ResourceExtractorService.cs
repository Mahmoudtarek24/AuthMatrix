using AuthMatrix.Dto_s;
using AuthMatrix.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace AuthMatrix.Services
{
	public class ResourceExtractorService
	{
		private readonly IActionDescriptorCollectionProvider actionProvider;
		public ResourceExtractorService(IActionDescriptorCollectionProvider collectionProvider)
		{
			this.actionProvider = collectionProvider;		
		}
		public List<ResourceDto> ExtractResources()
		{
			var actions = actionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>();

			var resources = new List<ResourceDto>();

			foreach (var action in actions)
			{
				var skipController = action.ControllerTypeInfo
									.GetCustomAttribute<SkipPermissionAttribute>() != null;

				if (skipController)
					continue;

				var skipAction =
					action.MethodInfo.GetCustomAttribute<SkipPermissionAttribute>() != null;

				if (skipAction)
					continue;

				resources.Add(new ResourceDto
				{
					Controller = $"{action.ControllerName}Controller",
					Name = action.ControllerName,
				});
			}

			return resources.GroupBy(e => new { e.Name, e.Controller }).Select(e => e.First()).ToList();
		}
	}
}
