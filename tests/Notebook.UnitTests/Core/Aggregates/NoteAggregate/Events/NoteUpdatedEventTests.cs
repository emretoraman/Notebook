using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Events;
using Notebook.SharedKernel.Interfaces;
using Moq;
using System;
using Xunit;
using static Notebook.Core.Aggregates.NoteAggregate.Events.NoteUpdatedEvent;

namespace Notebook.UnitTests.Core.Aggregates.NoteAggregate.Events
{
    public class NoteUpdatedEventTests
    {
        public class EmailNotificationHandlerTests
        {
            [Fact]
            public void Handle_ByEvent_Handles()
            {
                Note note = new("updated");
                string oldText = "test";
                DateTime dateOccurred = DateTime.UtcNow;
                NoteUpdatedEvent domainEvent = new(note, oldText, dateOccurred);

                Mock<IEmailSender> mockEmailSender = new();
                EmailNotificationHandler handler = new(mockEmailSender.Object);

                handler.Handle(domainEvent);

                mockEmailSender.Verify(
                    s => s.SendEmailAsync(
                        "from@test.com",
                        "to@test.com",
                        It.Is<string>(subject => subject.Contains(note.Id.ToString())),
                        It.Is<string>(
                            body => body.Contains(oldText) 
                                && body.Contains(note.Text)
                                && body.Contains(dateOccurred.ToString())
                        ),
                        default
                    ),
                    Times.Once
                );
            }
        }
    }
}
