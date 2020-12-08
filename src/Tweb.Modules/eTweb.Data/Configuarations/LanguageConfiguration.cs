using eTweb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Data.Configuarations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder
                .ToTable("Languages");

            builder
                .HasKey(x => x.Id);

            builder
                .HasMany(pt => pt.ProductTranslations)
                .WithOne();

            builder
                .HasMany(ct => ct.CategoryTranslations)
                .WithOne();
        }
    }
}
