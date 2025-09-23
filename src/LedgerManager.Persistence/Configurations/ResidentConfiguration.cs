using LedgerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerManager.Persistence.Configurations;

public class ResidentConfiguration: IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.LastName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(r => r.BirthDate)
            .IsRequired(false);
        
        builder.HasOne(r => r.Account)
            .WithMany(a => a.Residents)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}