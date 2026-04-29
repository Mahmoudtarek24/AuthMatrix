using AuthMatrix.Models;
using AuthMatrix.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthMatrix.Services
{
	public class AuthService
	{
		private readonly ApplicationDbContext context;
		public AuthService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<bool> LoginAsync(string username, string password)
		{
			var result = await context.Users.AnyAsync(e => e.Name == username && e.Password == password);
			return false;


		}	
		public async Task<bool> RegisterAsync(string username, string password)
		{
			await context.Users.AddAsync(new User
			{
				Name = username,
				Password = password,
				IsSuperAdmin = false,
			});	

			await context.SaveChangesAsync();

			return true;	
		}


		//public async Task<JwtSecurityToken> GenerateJWTToken(User user)
		//{
		//	List<Claim> claims = new List<Claim>();

		//	claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
		//	claims.Add(new Claim("IsSupperAdmin",user.IsSuperAdmin.ToString()));
		//	claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

		//	return new JwtSecurityToken(
		//		claims: claims,
		//		issuer 
		//		);
		//}
	}
}
