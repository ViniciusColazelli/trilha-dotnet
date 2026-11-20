using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.ShortUrls;

namespace UrlShortener.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.Property(x => x.Code).HasMaxLength(ShortUrl.CodeMaxLength);
            entity.Property(x => x.OriginalUrl).HasMaxLength(ShortUrl.OriginalUrlMaxLength);

            // Índice único: garante no banco que dois links nunca terão o mesmo código,
            // mesmo que duas requisições gerem o mesmo código ao mesmo tempo.
            entity.HasIndex(x => x.Code).IsUnique();
        });
    }
}
