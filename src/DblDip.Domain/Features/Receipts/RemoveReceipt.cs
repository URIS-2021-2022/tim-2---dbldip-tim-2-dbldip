using BuildingBlocks.Core;
using BuildingBlocks.EventStore;
using DblDip.Core.Models;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace DblDip.Domain.Features
{
    public class RemoveReceipt
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

            }
        }

        public class Request : IRequest<Unit>
        {
            public Guid ReceiptId { get; init; }
        }

        public class Response: ResponseBase
        {
            public ReceiptDto Receipt { get; init; }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IEventStore _store;
            private readonly IDateTime _dateTime;

            public Handler(IEventStore store, IDateTime dateTime)
            {
                _store = store;
                _dateTime = dateTime;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {

                var receipt = await _store.FindAsync<Receipt>(request.ReceiptId);

                receipt.Remove(_dateTime.UtcNow);

                _store.Add(receipt);

                await _store.SaveChangesAsync(cancellationToken);

                return new()
                {

                };
            }
        }
    }
}
