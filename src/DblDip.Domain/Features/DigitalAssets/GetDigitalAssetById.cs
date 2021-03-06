using BuildingBlocks.Core;
using DblDip.Core.Data;
using MediatR;
using DblDip.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class GetDigitalAssetById
    {
        public class Request : IRequest<Response>
        {
            public Guid DigitalAssetId { get; init; }
        }

        public class Response: ResponseBase
        {
            public DigitalAssetDto DigitalAsset { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IDblDipDbContext _context { get; init; }

            public Handler(IDblDipDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
                await System.Threading.Tasks.Task.Delay(2000, cancellationToken);
                return new Response
                {
                    DigitalAsset = _context.Set<DigitalAsset>().FirstOrDefault(x => x.DigitalAssetId == request.DigitalAssetId).ToDto()
                };
            }
        }
    }
}
