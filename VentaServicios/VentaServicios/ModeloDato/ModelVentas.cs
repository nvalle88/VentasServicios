namespace VentaServicios.ModeloDato
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelVentas : DbContext
    {
        public ModelVentas()
            : base("name=ModelVentas")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Agenda> Agenda { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Compromiso> Compromiso { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Formulario> Formulario { get; set; }
        public virtual DbSet<FormularioTipoDato> FormularioTipoDato { get; set; }
        public virtual DbSet<FormularioVisita> FormularioVisita { get; set; }
        public virtual DbSet<Gerente> Gerente { get; set; }
        public virtual DbSet<LogRutaVendedor> LogRutaVendedor { get; set; }
        public virtual DbSet<Noticia> Noticia { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<ProductoVisita> ProductoVisita { get; set; }
        public virtual DbSet<Promocion> Promocion { get; set; }
        public virtual DbSet<PromocionProducto> PromocionProducto { get; set; }
        public virtual DbSet<Supervisor> Supervisor { get; set; }
        public virtual DbSet<Suscripcion> Suscripcion { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TipoCliente> TipoCliente { get; set; }
        public virtual DbSet<TipoCompromiso> TipoCompromiso { get; set; }
        public virtual DbSet<TipoDato> TipoDato { get; set; }
        public virtual DbSet<TipoSuscripcion> TipoSuscripcion { get; set; }
        public virtual DbSet<TipoVisita> TipoVisita { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Vendedor> Vendedor { get; set; }
        public virtual DbSet<Visita> Visita { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agenda>()
                .Property(e => e.Notas)
                .IsUnicode(false);

            modelBuilder.Entity<Agenda>()
                .Property(e => e.idCliente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Chat>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.idCliente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Firma)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Apellido)
                .IsFixedLength();

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Telefono)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Agenda)
                .WithRequired(e => e.Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Compromiso>()
                .Property(e => e.IdCompromiso)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Compromiso>()
                .Property(e => e.IdTipoCompromiso)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Compromiso>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Compromiso>()
                .Property(e => e.Solucion)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Ruc)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.RazonSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Noticia)
                .WithRequired(e => e.Empresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Formulario>()
                .Property(e => e.Titulo)
                .IsFixedLength();

            modelBuilder.Entity<FormularioTipoDato>()
                .Property(e => e.idFormularioTipoDato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FormularioTipoDato>()
                .Property(e => e.Nombre)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FormularioTipoDato>()
                .Property(e => e.idTipoDato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FormularioVisita>()
                .Property(e => e.Valor)
                .IsUnicode(false);

            modelBuilder.Entity<FormularioVisita>()
                .Property(e => e.idFormularioTipoDato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Gerente>()
                .HasMany(e => e.Supervisor)
                .WithRequired(e => e.Gerente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Noticia>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Noticia>()
                .Property(e => e.UrlFoto)
                .IsUnicode(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.ProductoVisita)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Promocion>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Promocion>()
                .Property(e => e.Detalle)
                .IsUnicode(false);

            modelBuilder.Entity<Supervisor>()
                .Property(e => e.IdSupervisor)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipoCliente>()
                .Property(e => e.Tipo)
                .IsFixedLength();

            modelBuilder.Entity<TipoCompromiso>()
                .Property(e => e.IdTipoCompromiso)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipoCompromiso>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDato>()
                .Property(e => e.idTipoDato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipoDato>()
                .Property(e => e.Nombre)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipoSuscripcion>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoSuscripcion>()
                .HasMany(e => e.Suscripcion)
                .WithRequired(e => e.TipoSuscripcion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoVisita>()
                .Property(e => e.Icono)
                .IsUnicode(false);

            modelBuilder.Entity<TipoVisita>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<TipoVisita>()
                .Property(e => e.Detalle)
                .IsFixedLength();

            modelBuilder.Entity<TipoVisita>()
                .HasMany(e => e.Visita)
                .WithRequired(e => e.TipoVisita)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.TokenContrasena)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Correo)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Identificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nombres)
                .IsFixedLength();

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Apellidos)
                .IsFixedLength();

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Telefono)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Chat)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioEnvia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Chat1)
                .WithRequired(e => e.Usuario1)
                .HasForeignKey(e => e.UsuarioRecibe)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendedor>()
                .Property(e => e.IdSupervisor)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Vendedor>()
                .HasMany(e => e.Agenda)
                .WithRequired(e => e.Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendedor>()
                .HasMany(e => e.Cliente)
                .WithRequired(e => e.Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendedor>()
                .HasMany(e => e.LogRutaVendedor)
                .WithRequired(e => e.Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendedor>()
                .HasMany(e => e.Visita)
                .WithRequired(e => e.Vendedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Visita>()
                .Property(e => e.Firma)
                .IsUnicode(false);

            modelBuilder.Entity<Visita>()
                .Property(e => e.Foto)
                .IsUnicode(false);

            modelBuilder.Entity<Visita>()
                .Property(e => e.idCliente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Visita>()
                .HasMany(e => e.ProductoVisita)
                .WithRequired(e => e.Visita)
                .WillCascadeOnDelete(false);
        }
    }
}
