using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Models;

namespace Web.Blog.Data.EntityConfiguration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.Property(x => x.Titulo)
                   .IsRequired()
                   .HasColumnType("NVarchar")
                   .HasMaxLength(500);

            builder.Property(a => a.Descricao)
                   .IsRequired()
                   .HasColumnType("NVarchar(max)")
                   .HasMaxLength(40000);

            builder.Property(a => a.DataPostagem)
                   .IsRequired();

            builder.Property(a => a.DataAlteracaoPostagem)
                   .IsRequired(false);

            builder.Property(a => a.Ativo)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.HasOne(a => a.Autor)
                   .WithMany()
                   .HasForeignKey(a => a.UsuarioId)
                   .IsRequired();
        }
    }
}