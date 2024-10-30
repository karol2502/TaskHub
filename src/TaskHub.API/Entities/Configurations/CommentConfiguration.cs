using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskHub.Entities.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Content).IsRequired().HasMaxLength(100);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired();
        builder.HasOne(x => x.Task).WithMany(x => x.Comments).HasForeignKey(x => x.TaskId).IsRequired();
    }
}
