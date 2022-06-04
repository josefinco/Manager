using Manager.Domain.Entities;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Infra.Context
{
  public class ManagerContext : DbContext
  {
    public ManagerContext() { }

    public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    // Configuração para execução Inicial do Banco de Dados
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //   optionsBuilder.UseSqlServer(@"Server = 127.0.0.1; Database = USERMANAGERAPI; User Id = admin; Password = Admin@123456");
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new UserMap());
    }

  }
}
