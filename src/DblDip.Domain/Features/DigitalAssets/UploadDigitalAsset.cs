using BuildingBlocks.Core;
using DblDip.Core.Data;
using BuildingBlocks.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using DblDip.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DblDip.Domain.Features
{
    public class UploadDigitalAsset
    {
        public class Request : IRequest<Response> { }

        public class Response : ResponseBase
        {
            public List<Guid> DigitalAssetIds { get; init; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IDblDipDbContext _context { get; init; }
            public IHttpContextAccessor _httpContextAccessor { get; init; }
            public Handler(IDblDipDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var defaultFormOptions = new FormOptions();
                var digitalAssets = new List<DigitalAsset>();

                if (!MultipartRequestHelper.IsMultipartContentType(httpContext.Request.ContentType))
                    throw new ArgumentException($"Expected a multipart request, but got {httpContext.Request.ContentType}");

                var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(httpContext.Request.ContentType);

                var boundary = MultipartRequestHelper.GetBoundary(
                    mediaTypeHeaderValue,
                    defaultFormOptions.MultipartBoundaryLengthLimit);

                var reader = new MultipartReader(boundary, httpContext.Request.Body);

                var section = await reader.ReadNextSectionAsync(cancellationToken);

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out ContentDispositionHeaderValue contentDisposition);

                    if (hasContentDispositionHeader && MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        using (var targetStream = new MemoryStream())
                        {
                            await section.Body.CopyToAsync(targetStream, cancellationToken);
                            var name = $"{contentDisposition.FileName}".Trim(new char[] { '"' }).Replace("&", "and");
                            var bytes = StreamHelper.ReadToEnd(targetStream);
                            var contentType = section.ContentType;

                            var digitalAsset = new DigitalAsset(name, bytes, contentType);

                            _context.Add(digitalAsset);

                            digitalAssets.Add(digitalAsset);
                        }

                    }

                    section = await reader.ReadNextSectionAsync(cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new Response()
                {
                    DigitalAssetIds = digitalAssets.Select(x => x.DigitalAssetId).ToList()
                };
            }
        }
    }
}
