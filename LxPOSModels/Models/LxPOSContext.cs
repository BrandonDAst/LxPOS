using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LxPOSModels.Models
{
	public partial class LxPOSContext : DbContext
	{
		private string ConnectionString { get; set; }

		public LxPOSContext(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public LxPOSContext(DbContextOptions<LxPOSContext> options, string connectionString)
				: base(options)
		{
			ConnectionString = connectionString;
		}
		/*Use default constructors and local connection string to point a local database.*/
		public LxPOSContext() { }
		public LxPOSContext(DbContextOptions<LxPOSContext> options) : base(options) { }

		public virtual DbSet<Catalog> Catalog { get; set; }
		public virtual DbSet<Products> Products { get; set; }
		public virtual DbSet<Settings> Settings { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
				/*Use local connection string and execute current migrations, in order to Entity Framework to create the Database.*/
				/*Via Package Manager Console type "PM> update-database –verbose */
				optionsBuilder.UseSqlServer("Server=LAPTOP-HP;Database=LxPOS;Trusted_Connection=True;");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Catalog>(entity => {
				entity.Property(e => e.Id).HasColumnName("id");

				entity.Property(e => e.Category)
						.IsRequired()
						.HasColumnName("category")
						.HasMaxLength(100)
						.IsUnicode(false);

				entity.Property(e => e.Subcat)
						.IsRequired()
						.HasColumnName("subcat")
						.HasMaxLength(100)
						.IsUnicode(false);

				entity.Property(e => e.Value)
						.IsRequired()
						.HasColumnName("value")
						.HasMaxLength(50)
						.IsUnicode(false);
			});

			modelBuilder.Entity<Products>(entity => {
				entity.Property(e => e.Id).HasColumnName("id");

				entity.Property(e => e.Name)
						.IsRequired()
						.HasColumnName("name")
						.HasMaxLength(50)
						.IsUnicode(false);

				entity.Property(e => e.Price)
						.HasColumnName("price")
						.HasColumnType("decimal(18, 2)");
			});

			modelBuilder.Entity<Settings>(entity => {
				entity.Property(e => e.Id).HasColumnName("id");

				entity.Property(e => e.Name)
						.IsRequired()
						.HasColumnName("name")
						.HasMaxLength(50)
						.IsUnicode(false);

				entity.Property(e => e.Value)
						.IsRequired()
						.HasColumnName("value")
						.HasMaxLength(50)
						.IsUnicode(false);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
