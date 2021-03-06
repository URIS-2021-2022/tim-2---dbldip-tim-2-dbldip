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
using static DblDip.Api.FunctionalTests.OffersControllerTests.Endpoints;

namespace DblDip.Api.FunctionalTests
{
    public class OffersControllerTests : IClassFixture<ApiTestFixture>
    {
        private readonly ApiTestFixture _fixture;
        public OffersControllerTests(ApiTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async System.Threading.Tasks.Task Should_CreateOffer()
        {
            var context = _fixture.Context;

            var offer = OfferDtoBuilder.WithDefaults();

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { offer }), Encoding.UTF8, "application/json");

            using var client = _fixture.CreateAuthenticatedClient();

            var httpResponseMessage = await client.PostAsync(Endpoints.Post.CreateOffer, stringContent);

            var response = JsonConvert.DeserializeObject<CreateOffer.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            var sut = context.FindAsync<Offer>(response.Offer.OfferId);

            Assert.NotEqual(default, response.Offer.OfferId);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_RemoveOffer()
        {
            var offer = OfferBuilder.WithDefaults();

            var context = _fixture.Context;

            var client = _fixture.CreateAuthenticatedClient();

            context.Add(offer);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await client.DeleteAsync(Delete.By(offer.OfferId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var removedOffer = await context.FindAsync<Offer>(offer.OfferId);

            Assert.NotEqual(default, removedOffer.Deleted);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_UpdateOffer()
        {
            var offer = OfferBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(offer);

            await context.SaveChangesAsync(default);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { offer = offer.ToDto() }), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().PutAsync(Put.Update, stringContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            var sut = await context.FindAsync<Offer>(offer.OfferId);

            Assert.NotNull(sut);

        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetOffers()
        {
            var offer = OfferBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(offer);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.Offers);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetOffers.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.True(response.Offers.Any());
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetOfferById()
        {
            var offer = OfferBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(offer);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.By(offer.OfferId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetOfferById.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.NotNull(response);

        }

        internal static class Endpoints
        {
            public static class Post
            {
                public static string CreateOffer = "api/offers";
            }

            public static class Put
            {
                public static string Update = "api/offers";
            }

            public static class Delete
            {
                public static string By(Guid offerId)
                {
                    return $"api/offers/{offerId}";
                }
            }

            public static class Get
            {
                public static string Offers = "api/offers";
                public static string By(Guid offerId)
                {
                    return $"api/offers/{offerId}";
                }
            }
        }
    }
}
