using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyInstitute.SchoolManagement.Shared.EntitiesSoftSec;

namespace MyInstitute.SchoolManagement.AccesData.EntitiesSoftSecConfig;

public class CategoryConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");
        builder.HasKey(e => e.UsuarioId);
        builder.HasIndex(e => e.UserName).IsUnique();
        //Evitar el borrado en cascada
        //builder.HasOne(e => e.Corporation).WithMany(c => c.Usuarios).OnDelete(DeleteBehavior.Restrict);
    }
}