using DblDip.Core.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class Search
    {
        public record Request(string Query) : IRequest<Response>;

        public record SearchResult();

        public record Response(ICollection<SearchResult> Results);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDblDipDbContext _context;

            public Handler(IDblDipDbContext context)
            {
                _context = context;
            }

            public IDblDipDbContext GetContext()
            {
                return _context;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await System.Threading.Tasks.Task.Delay(2000, cancellationToken);
                return new Response(null);
            }
        }
    }
}
