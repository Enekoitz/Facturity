using Facturity.Modules.Auth.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturity.Modules.Auth.Infrastructure;

internal class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();
        builder.HasKey(r => r.Id);
        
        builder.Property(t => t.TokenHash)
            .HasColumnName("token_hash")
            .HasMaxLength(128)
            .IsRequired();
        builder.HasIndex(t => t.TokenHash)
            .IsUnique();

        builder.Property(r => r.FamilyId)
            .HasColumnName("family_id")
            .ValueGeneratedNever()
            .IsRequired();
        builder.HasIndex(r => r.FamilyId);
        
        builder.Property(r => r.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();
        
        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(r => r.RevokedAt)
            .HasColumnName("revoked_at");

        // TODO: versiones posteriores. 
        // Cuando montemos el BackgroundService de borrado o limpieza de tokens viejos/revocados/expirados
        // tiene sentido crear un índice por los campos de filtrado: expires_at y revoked_at
        
        // builder.HasIndex(r => new { r.ExpiresAt, r.RevokedAt });

        builder.Property(r => r.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedNever();
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
