using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;

namespace DblDip.Core.Models
{
    public class Brand : AggregateRoot
    {
        public Guid BrandId { get; private set; }
        public Guid LogoDigitalAssetId { get; private set; }
        public DateTime? Deleted { get; private set; }
        protected override void When(dynamic @event) => When(@event);

        public Brand()
        {
            Apply(new BrandCreated(Guid.NewGuid()));
        }

        public void When(BrandCreated brandCreated)
        {
            BrandId = brandCreated.BrandId;
        }

        public void When(BrandRemoved brandRemoved)
        {
            Deleted = brandRemoved.Deleted;
        }

        public static void When(BrandUpdated brandUpdated)
        {
            throw new NotSupportedException();
        }

        protected override void EnsureValidState()
        {

        }

        public void Remove(DateTime deleted)
        {
            Apply(new BrandRemoved(deleted));
        }

        public void Update()
        {
            Apply(new BrandUpdated());
        }
    }
}
