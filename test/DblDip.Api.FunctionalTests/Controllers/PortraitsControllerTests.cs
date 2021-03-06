using Newtonsoft.Json;
using DblDip.Core.Models;
using DblDip.Domain.Features;
using DblDip.Domain.Features;
using DblDip.Testing;
using DblDip.Testing.Builders;
using DblDip.Testing.Builders;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;
using static DblDip.Api.FunctionalTests.PortraitsControllerTests.Endpoints;

namespace DblDip.Api.FunctionalTests
{
    public class PortraitsControllerTests : IClassFixture<ApiTestFixture>
    {
        private readonly ApiTestFixture _fixture;
        public PortraitsControllerTests(ApiTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async System.Threading.Tasks.Task Should_CreatePortrait()
        {
            var context = _fixture.Context;

            var portrait = PortraitDtoBuilder.WithDefaults();

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { portrait }), Encoding.UTF8, "application/json");

            using var client = _fixture.CreateAuthenticatedClient();

            var httpResponseMessage = await client.PostAsync(Endpoints.Post.CreatePortrait, stringContent);

            var response = JsonConvert.DeserializeObject<CreatePortrait.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            var sut = context.FindAsync<Portrait>(response.Portrait.PortraitId);

            Assert.NotEqual(default, response.Portrait.PortraitId);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_RemovePortrait()
        {
            var portrait = PortraitBuilder.WithDefaults();

            var context = _fixture.Context;

            var client = _fixture.CreateAuthenticatedClient();

            context.Add(portrait);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await client.DeleteAsync(Delete.By(portrait.PortraitId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var removedPortrait = await context.FindAsync<Portrait>(portrait.PortraitId);

            Assert.NotEqual(default, removedPortrait.Deleted);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_UpdatePortrait()
        {
            var portrait = PortraitBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(portrait);

            await context.SaveChangesAsync(default);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { portrait = portrait.ToDto() }), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().PutAsync(Put.Update, stringContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            var sut = await context.FindAsync<Portrait>(portrait.PortraitId);

            Assert.NotNull(sut);

        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetPortraits()
        {
            var portrait = PortraitBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(portrait);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.portraits);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetPortraits.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.True(response.Portraits.Any());
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetPortraitById()
        {
            var portrait = PortraitBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(portrait);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.By(portrait.PortraitId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetPortraitById.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.NotNull(response);

        }

        internal static class Endpoints
        {
            public static class Post
            {
                public static string CreatePortrait = "api/portraits";
            }

            public static class Put
            {
                public static string Update = "api/portraits";
            }

            public static class Delete
            {
                public static string By(Guid portraitId)
                {
                    return $"api/portraits/{portraitId}";
                }
            }

            public static class Get
            {
                public static string portraits = "api/portraits";
                public static string By(Guid portraitId)
                {
                    return $"api/portraits/{portraitId}";
                }
            }
        }
    }
}
