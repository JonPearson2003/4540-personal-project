using BuildTrack.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildTrack.Api.Data;

public sealed class BuildTrackDbContext(DbContextOptions<BuildTrackDbContext> options)
	: DbContext(options)
{
	public DbSet<Users> Users => Set<Users>();
	public DbSet<Projects> Projects => Set<Projects>();
	public DbSet<ProjectStatus> ProjectStatuses => Set<ProjectStatus>();
	public DbSet<TimeRecords> TimeRecords => Set<TimeRecords>();
	public DbSet<ProjectMaterials> ProjectMaterials => Set<ProjectMaterials>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Users>(entity =>
		{
			entity.HasKey(user => user.UserId);
			entity.Property(user => user.Name).IsRequired();
			entity.Property(user => user.Email).IsRequired();
			entity.Property(user => user.Password).IsRequired();
			entity.Property(user => user.Role).IsRequired();
		});

		modelBuilder.Entity<Projects>(entity =>
		{
			entity.HasKey(project => project.ProjectId);
			entity.Property(project => project.Name).IsRequired();
			entity.Property(project => project.Description).IsRequired();
			entity.Property(project => project.Location).IsRequired();
		});

		modelBuilder.Entity<ProjectStatus>(entity =>
		{
			entity.HasKey(status => status.ProjectStatusId);
			entity.Property(status => status.Status).IsRequired();
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(status => status.ProjectId);
		});

		modelBuilder.Entity<TimeRecords>(entity =>
		{
			entity.HasKey(record => record.TimeRecordId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(record => record.UserId);
		});

		modelBuilder.Entity<ProjectMaterials>(entity =>
		{
			entity.HasKey(material => material.ProjectMaterialId);
			entity.Property(material => material.Name).IsRequired();
			entity.Property(material => material.Description).IsRequired();
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(material => material.ProjectId);
		});
	}
}