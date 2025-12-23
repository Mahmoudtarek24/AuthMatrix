using AuthMatrix.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthMatrix.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserGroup>().HasKey(x => new { x.UserId,x.GroupId});
			modelBuilder.Entity<UserGroup>().HasOne(e=>e.User).WithMany(e=>e.UserGroups).HasForeignKey(e=>e.UserId);
			modelBuilder.Entity<UserGroup>().HasOne(e => e.Group).WithMany(e => e.UserGroups).HasForeignKey(e => e.GroupId);
			modelBuilder.Entity<GroupPermission>().HasKey(x => new { x.GroupId, x.PermissionId });
			modelBuilder.Entity<GroupPermission>().HasOne(e => e.Permission).WithMany(e => e.GroupPermissions).HasForeignKey(e => e.PermissionId);
			modelBuilder.Entity<GroupPermission>().HasOne(e => e.Group).WithMany(e => e.GroupPermissions).HasForeignKey(e => e.GroupId);

			modelBuilder.Entity<Permission>().HasOne(e => e.Resource).WithMany(e => e.Permissions).HasForeignKey(e => e.ResourceId);

			modelBuilder.Entity<Resource>().HasOne(e => e.ParentResource).WithMany(e => e.ChildResources).HasForeignKey(e => e.ParentResourceId);


		}
		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }
		public DbSet<GroupPermission> GroupPermissions { get; set; }

		public DbSet<Invoice> Invoices { get; set; }	
		public DbSet<InvoiceItem> InvoiceItems { get; set; }	
		public DbSet<Order> Orders { get; set; }
		public DbSet<Student> Students { get; set; }	
	}
}
