using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.SharedKernel.BaseClasses;
using Notebook.SharedKernel.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Core.Aggregates.NoteAggregate.Events
{
    public class NoteUpdatedEvent : BaseDomainEvent
    {
        public NoteUpdatedEvent(Note note, string oldText)
        {
            Note = note;
            OldText = oldText;
        }

        public NoteUpdatedEvent(Note note, string oldText, DateTime dateOccurred) : this(note, oldText)
        {
            DateOccurred = dateOccurred;
        }

        public Note Note { get; private set; }
        public string OldText { get; private set; }

        public class EmailNotificationHandler : INotificationHandler<NoteUpdatedEvent>
        {
            private readonly IEmailSender _emailSender;

            public EmailNotificationHandler(IEmailSender emailSender)
            {
                _emailSender = emailSender;
            }

            public Task Handle(NoteUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
            {
                return _emailSender.SendEmailAsync(
                    "from@test.com",
                    "to@test.com", 
                    $"Note({domainEvent.Note.Id}) updated", 
                    $"Note's text \"{domainEvent.OldText}\" updated with \"{domainEvent.Note.Text}\" on {domainEvent.DateOccurred}",
                    cancellationToken
                );
            }
        }
    }
}
