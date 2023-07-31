using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Creating a property so that EF Core can interact with the specified
        //table in the DB. DbSet<ClassNameOfModel> NameOfSqlTable
        public DbSet<Category> Categories { get; set; }
    }
}
