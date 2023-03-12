﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Core.Entities.Identity;

namespace Shared.Infrastructure.Config.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"__Identity_{nameof(User)}");

            builder.HasIndex(x => new { x.NormalizedEmail }).IsUnique();
            builder.HasIndex(x => new { x.NormalizedUserName }).IsUnique();

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Email)
                .IsRequired();
            builder.Property(u => u.NormalizedEmail)
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .IsRequired();
        }
    }
}
