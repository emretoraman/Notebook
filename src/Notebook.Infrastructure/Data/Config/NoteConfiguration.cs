using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notebook.Infrastructure.Data.Config
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(n => n.Text)
                .IsRequired();
        }
    }
}
