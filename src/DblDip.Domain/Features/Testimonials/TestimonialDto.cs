using System;

namespace DblDip.Domain.Features
{
    public class TestimonialDto
    {
        public Guid TestimonialId { get; init; }
        public Guid PhotographerId { get; init; }
        public Guid ClientId { get; init; }
        public string Description { get; init; }
    }
}
