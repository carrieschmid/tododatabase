using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Models
{
    public class ToDoListContext : IdentityDbContext<ApplicationUser>
    {
 
    public virtual DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<CategoryItem> CategoryItem { get; set; }
    
    public ToDoListContext(DbContextOptions options) : base(options) { }
  
    }
}