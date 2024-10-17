using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Models;

namespace Web.Blog.Data.Configurations
{
    public class AutorConfiguration : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.ToTable("Autores");

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.SobreNome)
                .HasMaxLength(100);

            builder.Property(a => a.DataCadastro)
                .IsRequired();

            builder.Property(a => a.Admin)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
