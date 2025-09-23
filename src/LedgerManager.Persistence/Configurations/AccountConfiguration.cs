using LedgerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerManager.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.AccountNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(a => a.AccountNumber)
            .IsUnique();
        
        builder.Property(a => a.StartDate)
            .IsRequired();

        builder.Property(a => a.EndDate)
            .IsRequired();
        
        builder.Property(a => a.Address)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.Area)
            .IsRequired();
        
        builder.HasMany(a => a.Residents)
            .WithOne(a => a.Account)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}