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
      .HasColumType("BIGINT");

      builder.Property(x => x.Name)
      .IsRequired()
      .HaxMaxLength(80)
      .HasColumnType("VARCHAR(80)")
      .HasColumnName("name");

      builder.Property(x => x.password)
      .IsRequired()
      .HaxMaxLength(30)
      .HasColumnType("VARCHAR(30)")
      .HasColumnName("password");

      builder.Property(x => x.Email)
      .IsRequired()
      .HaxMaxLength(100)
      .HasColumnType("VARCHAR(100)")
      .HasColumnName("email");


    }
  }
}
