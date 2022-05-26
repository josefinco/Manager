using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Manager.Infra.Mappings
{
  public class UserMap : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
       builder.ToTable("User");

       builder.HasKey(x => x.Id);

       builder.Property(x => x.Id)
           .UseIdentityColumn()
           .HasColumnType("BIGINT");

       builder.Property(x => x.Name)
      .IsRequired()
      .HasMaxLength(80)
      .HasColumnType("VARCHAR(80)")
      .HasColumnName("name");

      builder.Property(x => x.Password)
      .IsRequired()
      .HasMaxLength(30)
      .HasColumnType("VARCHAR(30)")
      .HasColumnName("password");

      builder.Property(x => x.Email)
      .IsRequired()
      .HasMaxLength(100)
      .HasColumnType("VARCHAR(100)")
      .HasColumnName("email");


    }
  }
}
