using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class EditedPhoto : Service
    {
        public Guid EditedPhotoId { get; private set; }
        public EditedPhoto()
            : base(default, default, default)
        {

        }

        protected override void When(dynamic @event) => When(@event);

        public static void When(EditedPhotoUpdated editedPhotoUpdated)
        {
            throw new NotSupportedException();
        }

        protected override void EnsureValidState()
        {
            throw new NotSupportedException();
        }

        public void Update()
        {
            throw new NotSupportedException();
        }
    }
}
