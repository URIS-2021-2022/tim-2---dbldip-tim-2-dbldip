using BuildingBlocks.Core;
using DblDip.Core.Data;
using DblDip.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class GetMeetings
    {
        public class Request : IRequest<Response> { }

        public class Response: ResponseBase
        {
            public List<MeetingDto> Meetings { get; init; }
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
                    Meetings = _context.Set<Meeting>().Select(x => x.ToDto()).ToList()
                };
            }
        }
    }
}
