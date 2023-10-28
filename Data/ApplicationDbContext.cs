using ImprovedPicpay.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImprovedPicpay.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
