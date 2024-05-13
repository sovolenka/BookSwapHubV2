using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class PostgresContext : IdentityDbContext<ApplicationUser>
{
    public virtual DbSet<Book>? Books { get; set; }
    public virtual DbSet<BookPosting>? BookPostings { get; set; }
    public virtual DbSet<SwapProposal>? SwapProposals { get; set; }
    public virtual DbSet<Address>? Addresses { get; set; }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public PostgresContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // swap proposal and sender relationship
        builder.Entity<SwapProposal>()
            .HasOne(s => s.Sender)
            .WithMany()
            .HasForeignKey(s => s.SenderId);

        // swap proposal and receiver relationship
        builder.Entity<SwapProposal>()
            .HasOne(s => s.Receiver)
            .WithMany()
            .HasForeignKey(s => s.ReceiverId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=bookswaphub-postgres.postgres.database.azure.com;Database=BookSwapHub;Port=5432;User Id=pgadmin;Password=CustomerHub0;Ssl Mode=VerifyFull;");
    }
}
