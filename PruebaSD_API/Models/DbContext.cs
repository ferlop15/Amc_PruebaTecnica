using Microsoft.EntityFrameworkCore;


public class PruebaSDContext : DbContext
{
    public PruebaSDContext(DbContextOptions<PruebaSDContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
}