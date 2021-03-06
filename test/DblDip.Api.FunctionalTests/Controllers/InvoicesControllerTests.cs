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
using static DblDip.Api.FunctionalTests.InvoicesControllerTests.Endpoints;

namespace DblDip.Api.FunctionalTests
{
    public class InvoicesControllerTests : IClassFixture<ApiTestFixture>
    {
        private readonly ApiTestFixture _fixture;
        public InvoicesControllerTests(ApiTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async System.Threading.Tasks.Task Should_CreateInvoice()
        {
            var context = _fixture.Context;

            var invoice = InvoiceDtoBuilder.WithDefaults();

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { invoice }), Encoding.UTF8, "application/json");

            using var client = _fixture.CreateAuthenticatedClient();

            var httpResponseMessage = await client.PostAsync(Endpoints.Post.CreateInvoice, stringContent);

            var response = JsonConvert.DeserializeObject<CreateInvoice.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            var sut = context.FindAsync<Invoice>(response.Invoice.InvoiceId);

            Assert.NotEqual(default, response.Invoice.InvoiceId);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_RemoveInvoice()
        {
            var invoice = InvoiceBuilder.WithDefaults();

            var context = _fixture.Context;

            var client = _fixture.CreateAuthenticatedClient();

            context.Add(invoice);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await client.DeleteAsync(Delete.By(invoice.InvoiceId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var removedInvoice = await context.FindAsync<Invoice>(invoice.InvoiceId);

            Assert.NotEqual(default, removedInvoice.Deleted);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_UpdateInvoice()
        {
            var invoice = InvoiceBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(invoice);

            await context.SaveChangesAsync(default);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { invoice = invoice.ToDto() }), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().PutAsync(Put.Update, stringContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            var sut = await context.FindAsync<Invoice>(invoice.InvoiceId);

            Assert.NotNull(sut);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetInvoices()
        {
            var invoice = InvoiceBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(invoice);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.Invoices);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetInvoices.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.True(response.Invoices.Any());
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetInvoiceById()
        {
            var invoice = InvoiceBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(invoice);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.By(invoice.InvoiceId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetInvoiceById.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.NotNull(response);

        }

        internal static class Endpoints
        {
            public static class Post
            {
                public static string CreateInvoice = "api/invoices";
            }

            public static class Put
            {
                public static string Update = "api/invoices";
            }

            public static class Delete
            {
                public static string By(Guid invoiceId)
                {
                    return $"api/invoices/{invoiceId}";
                }
            }

            public static class Get
            {
                public static string Invoices = "api/invoices";
                public static string By(Guid invoiceId)
                {
                    return $"api/invoices/{invoiceId}";
                }
            }
        }
    }
}
