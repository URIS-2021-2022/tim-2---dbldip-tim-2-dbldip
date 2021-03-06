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
using static DblDip.Api.FunctionalTests.BoardsControllerTests.Endpoints;

namespace DblDip.Api.FunctionalTests
{
    public class BoardsControllerTests : IClassFixture<ApiTestFixture>
    {
        private readonly ApiTestFixture _fixture;
        public BoardsControllerTests(ApiTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async System.Threading.Tasks.Task Should_CreateBoard()
        {
            var context = _fixture.Context;

            var board = BoardDtoBuilder.WithDefaults();

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { board }), Encoding.UTF8, "application/json");

            using var client = _fixture.CreateAuthenticatedClient();

            var httpResponseMessage = await client.PostAsync(Endpoints.Post.CreateBoard, stringContent);

            var response = JsonConvert.DeserializeObject<CreateBoard.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            var sut = context.FindAsync<Board>(response.Board.BoardId);

            Assert.NotEqual(default, response.Board.BoardId);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_RemoveBoard()
        {
            var board = BoardBuilder.WithDefaults();

            var context = _fixture.Context;

            var client = _fixture.CreateAuthenticatedClient();

            context.Add(board);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await client.DeleteAsync(Delete.By(board.BoardId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var removedBoard = await context.FindAsync<Board>(board.BoardId);

            Assert.NotEqual(default, removedBoard.Deleted);
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_UpdateBoard()
        {
            var board = BoardBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(board);

            await context.SaveChangesAsync(default);

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(new { board = board.ToDto() }), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().PutAsync(Put.Update, stringContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            var sut = await context.FindAsync<Board>(board.BoardId);

            Assert.NotNull(sut);

        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetBoards()
        {
            var board = BoardBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(board);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.Boards);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetBoards.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.True(response.Boards.Any());
        }

        [Fact]
        public async System.Threading.Tasks.Task Should_GetBoardById()
        {
            var board = BoardBuilder.WithDefaults();

            var context = _fixture.Context;

            context.Add(board);

            await context.SaveChangesAsync(default);

            var httpResponseMessage = await _fixture.CreateAuthenticatedClient().GetAsync(Get.By(board.BoardId));

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<GetBoardById.Response>(await httpResponseMessage.Content.ReadAsStringAsync());

            Assert.NotNull(response);

        }

        internal static class Endpoints
        {
            public static class Post
            {
                public static string CreateBoard = "api/boards";
            }

            public static class Put
            {
                public static string Update = "api/boards";
            }

            public static class Delete
            {
                public static string By(Guid boardId)
                {
                    return $"api/boards/{boardId}";
                }
            }

            public static class Get
            {
                public static string Boards = "api/boards";
                public static string By(Guid boardId)
                {
                    return $"api/boards/{boardId}";
                }
            }
        }
    }
}
