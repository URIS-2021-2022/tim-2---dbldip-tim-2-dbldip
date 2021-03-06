using BuildingBlocks.Core;
using DblDip.Core.Data;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class GetEquipment
    {
        public class Request : IRequest<Response> { }

        public class Response: ResponseBase
        {
            public List<EquipmentDto> Equipment { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDblDipDbContext _context;

            public Handler(IDblDipDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await System.Threading.Tasks.Task.Delay(2000, cancellationToken);
                return new Response()
                {
                    
                    Equipment = _context.Set<DblDip.Core.Models.Equipment>().Select(x => x.ToDto()).ToList()
                };
            }
        }
    }
}
