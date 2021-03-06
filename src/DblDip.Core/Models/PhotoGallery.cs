using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;
using System.Collections.Generic;

namespace DblDip.Core.Models
{
    public class PhotoGallery : AggregateRoot
    {
        public Guid PhotoGalleryId { get; private set; }
        public Guid PhotographerId { get; private set; }
        public string Name { get; private set; }
        public ICollection<Photo> Photos { get; private set; }
        public Guid CoverPhotoDigitalAssetId { get; private set; }
        public DateTime? Deleted { get; set; }
        public DateTime? Published { get; private set; }

        protected PhotoGallery()
        {

        }

        public PhotoGallery(string name)
        {
            Apply(new PhotoGalleryCreated(Guid.NewGuid(), name));
        }

        protected override void When(dynamic @event) => When(@event);

        public void When(PhotoGalleryCreated photoGalleryCreated)
        {
            PhotoGalleryId = photoGalleryCreated.PhotoGalleryId;
            Name = photoGalleryCreated.Name;
            Photos = new HashSet<Photo>();
        }

        public void When(PhotoGalleryUpdated photoGalleryUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(PhotoGalleryRemoved photoGalleryRemoved)
        {
            Deleted = photoGalleryRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Update()
        {
            Apply(new PhotoGalleryUpdated());
        }

        public void Remove(DateTime deleted)
        {
            Apply(new PhotoGalleryRemoved(deleted));
        }


    }
}
