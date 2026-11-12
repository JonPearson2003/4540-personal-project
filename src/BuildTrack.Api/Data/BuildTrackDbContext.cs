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
	public DbSet<ProjectAssignment> ProjectAssignments => Set<ProjectAssignment>();
	public DbSet<MaterialRequest> MaterialRequests => Set<MaterialRequest>();
	public DbSet<MaterialUsage> MaterialUsages => Set<MaterialUsage>();
	public DbSet<ReceiptInvoice> ReceiptInvoices => Set<ReceiptInvoice>();

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
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(record => record.ProjectId);
		});

		modelBuilder.Entity<ProjectMaterials>(entity =>
		{
			entity.HasKey(material => material.ProjectMaterialId);
			entity.HasAlternateKey(material =>
				new { material.ProjectId, material.ProjectMaterialId });
			entity.Property(material => material.Name).IsRequired();
			entity.Property(material => material.Description).IsRequired();
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(material => material.ProjectId);
		});

		modelBuilder.Entity<ProjectAssignment>(entity =>
		{
			entity.HasKey(assignment =>
				new { assignment.ProjectId, assignment.UserId });
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(assignment => assignment.ProjectId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(assignment => assignment.UserId);
		});

		modelBuilder.Entity<MaterialRequest>(entity =>
		{
			entity.HasKey(request => request.MaterialRequestId);
			entity.Property(request => request.Unit).IsRequired().HasMaxLength(40);
			entity.Property(request => request.Priority)
				.IsRequired().HasMaxLength(20);
			entity.Property(request => request.Status)
				.IsRequired().HasMaxLength(30);
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(request => request.ProjectId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(request => request.RequestedByUserId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(request => request.ReviewedByUserId);
			entity.HasOne<ProjectMaterials>()
				.WithMany()
				.HasForeignKey(request =>
					new { request.ProjectId, request.MaterialId })
				.HasPrincipalKey(material =>
					new { material.ProjectId, material.ProjectMaterialId });
		});

		modelBuilder.Entity<MaterialUsage>(entity =>
		{
			entity.HasKey(usage => usage.MaterialUsageId);
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(usage => usage.ProjectId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(usage => usage.UserId);
			entity.HasOne<ProjectMaterials>()
				.WithMany()
				.HasForeignKey(usage =>
					new { usage.ProjectId, usage.MaterialId })
				.HasPrincipalKey(material =>
					new { material.ProjectId, material.ProjectMaterialId });
		});

		modelBuilder.Entity<ReceiptInvoice>(entity =>
		{
			entity.HasKey(receipt => receipt.ReceiptInvoiceId);
			entity.Property(receipt => receipt.Vendor)
				.IsRequired().HasMaxLength(200);
			entity.Property(receipt => receipt.Amount).HasPrecision(18, 2);
			entity.HasOne<Projects>()
				.WithMany()
				.HasForeignKey(receipt => receipt.ProjectId);
			entity.HasOne<Users>()
				.WithMany()
				.HasForeignKey(receipt => receipt.UserId);
		});
	}
}