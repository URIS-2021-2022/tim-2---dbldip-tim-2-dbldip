using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using DblDip.Core.Interfaces;
using DblDip.Core.ValueObjects;
using System;

namespace DblDip.Core.Models
{
    public abstract class PhotographyProject : AggregateRoot, IScheduledAggregate
    {
        public Guid PhotographyProjectId { get; private set; }
        public abstract DateRange Scheduled { get; }
        public Guid PhotographerId { get; private set; }
        public Guid ParticipantId { get; private set; }
        public Guid AdditionalParticipantIds { get; private set; }
        public Guid? VendorId { get; private set; }
        public DateTime? GallerySent { get; private set; }
        public DateTime? Deleted { get; private set; }
        protected PhotographyProject()
        {

        }

        protected override void When(dynamic @event)
        {
            if (@event is PhotoGallerySent)
            {
                When(@event);
            }
        }
        public static void When(PhotoGallerySent sent)
        {
            throw new NotSupportedException();
        }

        public void When(PhotographyProjectRemoved photographyProjectRemoved)
        {
            Deleted = photographyProjectRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new PhotographyProjectRemoved(deleted));
        }

        public void SendGallery()
        {
            Apply(new PhotoGallerySent(default));
        }

        public static void Update()
        {
            throw new NotSupportedException();
        }
    }
}
