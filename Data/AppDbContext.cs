using Microsoft.EntityFrameworkCore;
using LogisticaApp.Models;

namespace LogisticaApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Conductor> Conductores { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Envio> Envios { get; set; }
    public DbSet<Ruta> Rutas { get; set; }
    public DbSet<Evidencia> Evidencias { get; set; }
    public DbSet<Incidencia> Incidencias { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<Tarifa> Tarifas { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Usuarios ---
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Rol).HasMaxLength(20).IsRequired();
            entity.ToTable(t => t.HasCheckConstraint("CK_usuarios_rol",
                "rol IN ('cliente', 'conductor', 'operador', 'administrador')"));
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        });

        // --- Conductores ---
        modelBuilder.Entity<Conductor>(entity =>
        {
            entity.ToTable("conductores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Licencia).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasIndex(e => e.UsuarioId).IsUnique();
            entity.HasOne(e => e.Usuario)
                .WithOne()
                .HasForeignKey<Conductor>(e => e.UsuarioId)
                .IsRequired();
        });

        // --- Vehículos ---
        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("vehiculos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Placa).HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.Placa).IsUnique();
            entity.Property(e => e.CapacidadKg).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Conductor)
                .WithMany()
                .HasForeignKey(e => e.ConductorId)
                .IsRequired(false);
        });

        // --- Envíos ---
        modelBuilder.Entity<Envio>(entity =>
        {
            entity.ToTable("envios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Estado).HasMaxLength(20).IsRequired().HasDefaultValue("pendiente");
            entity.ToTable(t => t.HasCheckConstraint("CK_envios_estado",
                "estado IN ('pendiente', 'asignado', 'en_transito', 'entregado', 'cancelado')"));
            entity.Property(e => e.Origen).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Destino).HasMaxLength(255).IsRequired();
            entity.Property(e => e.TipoMercancia).HasMaxLength(100);
            entity.Property(e => e.PesoKg).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .IsRequired();
            entity.HasOne(e => e.Conductor)
                .WithMany()
                .HasForeignKey(e => e.ConductorId)
                .IsRequired(false);
            entity.HasOne(e => e.Vehiculo)
                .WithMany()
                .HasForeignKey(e => e.VehiculoId)
                .IsRequired(false);
        });

        // --- Rutas ---
        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.ToTable("rutas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.DistanciaKm).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Envio)
                .WithMany()
                .HasForeignKey(e => e.EnvioId)
                .IsRequired();
        });

        // --- Evidencias ---
        modelBuilder.Entity<Evidencia>(entity =>
        {
            entity.ToTable("evidencias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.FotoUrl).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Latitud).HasColumnType("decimal(10,7)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(10,7)");
            entity.Property(e => e.FechaHora).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Envio)
                .WithMany()
                .HasForeignKey(e => e.EnvioId)
                .IsRequired();
            entity.HasOne(e => e.Conductor)
                .WithMany()
                .HasForeignKey(e => e.ConductorId)
                .IsRequired();
        });

        // --- Incidencias ---
        modelBuilder.Entity<Incidencia>(entity =>
        {
            entity.ToTable("incidencias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Descripcion).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Envio)
                .WithMany()
                .HasForeignKey(e => e.EnvioId)
                .IsRequired();
            entity.HasOne(e => e.Conductor)
                .WithMany()
                .HasForeignKey(e => e.ConductorId)
                .IsRequired();
        });

        // --- Notificaciones ---
        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("notificaciones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Mensaje).IsRequired();
            entity.Property(e => e.Leida).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .IsRequired();
        });

        // --- Tarifas ---
        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.ToTable("tarifas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Valor).HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        });

        // --- Favoritos ---
        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.ToTable("favoritos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Tipo).HasMaxLength(20).IsRequired();
            entity.ToTable(t => t.HasCheckConstraint("CK_favoritos_tipo",
                "tipo IN ('direccion', 'destinatario')"));
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Valor).HasMaxLength(255).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .IsRequired();
        });
    }
}