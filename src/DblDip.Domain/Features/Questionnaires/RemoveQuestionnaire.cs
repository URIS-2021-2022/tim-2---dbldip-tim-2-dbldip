using BuildingBlocks.Core;
using BuildingBlocks.EventStore;
using DblDip.Core.Models;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace DblDip.Domain.Features
{
    public class RemoveQuestionnaire
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

            }
        }

        public class Request : IRequest<Unit>
        {
            public Guid QuestionnaireId { get; init; }
        }

        public class Response: ResponseBase
        {
            public QuestionnaireDto Questionnaire { get; init; }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IEventStore _store;
            private readonly IDateTime _dateTime;

            public Handler(IEventStore store, IDateTime dateTime)
            {
                _store = store;
                _dateTime = dateTime;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {

                var questionnaire = await _store.FindAsync<Questionnaire>(request.QuestionnaireId);

                questionnaire.Remove(_dateTime.UtcNow);

                _store.Add(questionnaire);

                await _store.SaveChangesAsync(cancellationToken);

                return new()
                {

                };
            }
        }
    }
}
