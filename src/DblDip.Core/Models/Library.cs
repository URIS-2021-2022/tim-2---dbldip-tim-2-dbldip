using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;
using System.Collections.Generic;

namespace DblDip.Core.Models
{
    public class Library : AggregateRoot
    {
        public Guid LibraryId { get; private set; }
        public Guid PhotographerId { get; private set; }
        public ICollection<DigitalAssetReference> MyImages { get; private set; }
        public ICollection<DigitalAssetReference> MyFiles { get; private set; }
        public DateTime? Deleted { get; private set; }
        public Library()
        {
            Apply(new LibraryCreated(Guid.NewGuid()));
        }
        protected override void When(dynamic @event) => When(@event);

        public void When(LibraryCreated libraryCreated)
        {
            LibraryId = libraryCreated.LibraryId;
        }

        public void When(LibraryUpdated libraryUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(LibraryRemoved libraryRemoved)
        {
            Deleted = libraryRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public static void Update()
        {
            throw new NotSupportedException();
        }

        public void Remove(DateTime deleted)
        {
            Apply(new LibraryRemoved(deleted));
        }
    }
}
