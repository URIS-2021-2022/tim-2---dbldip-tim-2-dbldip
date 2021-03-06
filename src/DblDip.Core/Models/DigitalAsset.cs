using BuildingBlocks.EventStore;
using BuildingBlocks.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using DblDip.Core.DomainEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DblDip.Core.Data;

namespace DblDip.Core.Models
{
    public class DigitalAsset : AggregateRoot
    {
        public Guid DigitalAssetId { get; private set; }
        public string Name { get; private set; }
        public byte[] Bytes { get; private set; }
        public string ContentType { get; private set; }
        public DigitalAsset(string name, byte[] bytes, string contentType)
        {
            Apply(new DigitalAssetCreated(Guid.NewGuid(), name, bytes, contentType));
        }
        protected override void When(dynamic @event) => When(@event);

        public void When(DigitalAssetCreated digitalAssetCreated)
        {
            DigitalAssetId = digitalAssetCreated.DigitalAssetId;
            Name = digitalAssetCreated.Name;
            Bytes = digitalAssetCreated.Bytes;
            ContentType = digitalAssetCreated.ContentType;
        }

        protected override void EnsureValidState()
        {

        }

        protected DigitalAsset()
        {

        }



        public static async System.Threading.Tasks.Task<ICollection<DigitalAsset>> Upload(IHttpContextAccessor httpContextAccessor, IDblDipDbContext context, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var defaultFormOptions = new FormOptions();
            var digitalAssets = new List<DigitalAsset>();

            if (!MultipartRequestHelper.IsMultipartContentType(httpContext.Request.ContentType))
            {
                ArgumentNullException argumentNullException = new ArgumentNullException($"Expected a multipart request, but got {httpContext.Request.ContentType}");
                throw argumentNullException;
            }

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

                        context.Add(digitalAsset);

                        digitalAssets.Add(digitalAsset);
                    }

                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            return digitalAssets;
        }
    }
}
