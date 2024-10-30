using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskHub.Entities.Configurations;

public sealed class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasOne(x => x.User).WithMany(x => x.UserProjects).HasForeignKey(x => x.UserId).IsRequired(); ;
        builder.HasOne(x => x.Project).WithMany(x => x.UserProjects).HasForeignKey(x => x.ProjectId).IsRequired();
        builder.Property(x => x.Role).IsRequired();
    }
}
