using DblDip.Core.DomainEvents;
using DblDip.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace DblDip.Core.Models
{
    public class Photographer : Profile
    {
        public Guid PhotographerId { get; private set; }
        public Guid CompanyId { get; private set; }
        public ICollection<ServiceReference> Services { get; private set; }
        public Location PrimaryLocation { get; private set; }
        protected Photographer()
        {

        }

        protected override void When(dynamic @event) => When(@event);

        public Photographer(string name, Email email)
            : base(name, email, typeof(Photographer))
        {
            Apply(new PhotographerCreated(base.ProfileId));
        }

        public void When(PhotographerCreated photographerCreated)
        {
            PhotographerId = photographerCreated.PhotographerId;
        }

        public void When(PhotographerCompanyAdded photographerCompanyAdded)
        {
            CompanyId = photographerCompanyAdded.CompanyId;
        }

        public void When(PhotographerUpdated photographerUpdated)
        {
            throw new NotSupportedException();
        }

        protected override void EnsureValidState()
        {
            throw new NotSupportedException();
        }

        public void AddCompany(Guid companyId)
        {
            Apply(new PhotographerCompanyAdded(companyId));
        }

        public void Update()
        {
            Apply(new PhotographerUpdated());
        }


    }
}
