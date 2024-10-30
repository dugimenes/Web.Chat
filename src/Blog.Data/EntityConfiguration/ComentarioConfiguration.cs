﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog.Data.Models;

namespace Web.Blog.Data.EntityConfiguration
{
    public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.ToTable("Comentarios");

            builder.Property(x => x.Descricao)
                   .IsRequired()
                   .HasColumnType("TEXT")
                   .HasMaxLength(10000);

            builder.Property(x => x.DataCadastro)
                   .IsRequired();

            builder.HasOne(x => x.Autor)
                   .WithMany()
                   .HasForeignKey(x => x.UsuarioId)
                   .IsRequired();

            builder.HasOne(x => x.Post)
                   .WithMany()
                   .HasForeignKey(x => x.PostId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Ativo)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
