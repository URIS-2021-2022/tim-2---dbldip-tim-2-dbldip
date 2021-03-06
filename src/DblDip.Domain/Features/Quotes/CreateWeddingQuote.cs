using BuildingBlocks.Core;
using BuildingBlocks.EventStore;
using FluentValidation;
using MediatR;
using DblDip.Core.Models;
using DblDip.Core.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using static DblDip.Core.Constants.Rates;
using DblDip.Domain.IntegrationEvents;
using DblDip.Domain.Features;

namespace DblDip.Domain.Features
{
    public class CreateWeddingQuote
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

            }
        }

        public class Request : IRequest<Response>
        {
            public Guid WeddingId { get; init; }
            public string Email { get; init; }
        }

        public class Response: ResponseBase
        {
            public WeddingQuoteDto Quote { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IEventStore _store;
            private readonly IMediator _mediator;

            public Handler(IEventStore store, IMediator mediator)
            {
                _store = store;
                _mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var wedding = await _store.FindAsync<Wedding>(request.WeddingId);

                var rate = await _store.FindAsync<Rate>(PhotographyRate);

                var quote = new WeddingQuote((Email)request.Email, wedding, rate);

                _store.Add(quote);

                await _store.SaveChangesAsync(default);

                await _mediator.Publish(new QuoteCreated(quote), cancellationToken);

                return new Response()
                {
                    Quote = quote.ToDto()
                };
            }
        }
    }
}
