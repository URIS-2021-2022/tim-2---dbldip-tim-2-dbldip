using BuildingBlocks.EventStore;
using DblDip.Core.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DblDip.Core.Models
{
    public class Conversation : AggregateRoot
    {
        private List<Guid> _profileIds;

        private List<Guid> _messageIds;
        public Guid ConversationId { get; private set; }
        public List<Guid> MessageIds => _messageIds.ToList();
        public DateTime? Deleted { get; private set; }
        public Conversation()
        {
            Apply(new ConversationCreated(Guid.NewGuid()));
        }

        public List<Guid> ProfileIds()
        {
            return _profileIds.ToList();
        }

        protected override void When(dynamic @event) => When(@event);

        public static void When(ConversationUpdated conversationUpdated)
        {
            throw new NotSupportedException();
        }

        public void When(ConversationCreated conversationCreated)
        {
            ConversationId = conversationCreated.ConversationId;
            _profileIds = new List<Guid>();
            _messageIds = new List<Guid>();
        }

        public void When(ConversationRemoved conversationRemoved)
        {
            Deleted = conversationRemoved.Deleted;
        }

        protected override void EnsureValidState()
        {

        }

        public void Update()
        {
            Apply(new ConversationUpdated());
        }

        public void Remove(DateTime deleted)
        {
            Apply(new ConversationRemoved(deleted));
        }
    }
}
