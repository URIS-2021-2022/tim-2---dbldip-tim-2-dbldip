using BuildingBlocks.AspNetCore;
using DblDip.Core;
using DblDip.Core.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class GetCurrentAccountProfiles
    {
        public record Request : IRequest<Response>;

        public class Response
        {
            public Response(List<ProfileDto> profiles)
            {
                Profiles = profiles;
            }

            public List<ProfileDto> Profiles { get; set; }
        }


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDblDipDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IDblDipDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {

                var accountId = new Guid(_httpContextAccessor.HttpContext.User.FindFirst(Constants.ClaimTypes.AccountId).Value);

                var profiles = await (from account in _context.Accounts
                                      join profileReference in _context.Accounts.SelectMany(x => x.Profiles) on true equals true
                                      join profile in _context.Profiles on profileReference.ProfileId equals profile.ProfileId
                                      where account.AccountId == accountId
                                      select profile.ToDto()).ToListAsync(cancellationToken: cancellationToken);

                return new (profiles);
            }
        }
    }
}
