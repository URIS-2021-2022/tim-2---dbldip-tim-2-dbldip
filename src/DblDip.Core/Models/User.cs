using BuildingBlocks.EventStore;
using BuildingBlocks.Core;
using DblDip.Core.DomainEvents;
using DblDip.Core.Exceptions;
using DblDip.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DblDip.Core.Models
{
    public class User : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public Email Username { get; private set; }
        public string Password { get; private set; }
        public byte[] Salt { get; private set; }
        public bool PasswordResetRequired { get; private set; }
        public string RefreshToken { get; private set; }
        public ICollection<RoleReference> Roles { get; private set; } = new HashSet<RoleReference>();
        public DateTime? Deleted { get; private set; }
        protected User()
        {

        }
        public User(IEnumerable<IEvent> events)
            : base(events) { }

        public User(string username, string password = null, IUsernameAvailabilityCheck usernameAvailabilityCheck = null, IPasswordHasher passwordHasher = null)
        {
            var passwordResetRequired = string.IsNullOrEmpty(password);

            password ??= BuildingBlocks.Core.Password.Generate(8, 1);

            passwordHasher = new PasswordHasher();

            if (usernameAvailabilityCheck != null && !usernameAvailabilityCheck.IsAvailable(username) )
                throw new DomainException("Email not available");

            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var transformedPassword = passwordHasher.HashPassword(salt, password);

            Apply(new UserCreated(Guid.NewGuid(), username, transformedPassword, salt, passwordResetRequired));
        }

        protected override void When(dynamic @event) => When(@event);

        protected void When(UserCreated userCreated)
        {
            UserId = userCreated.UserId;
            Username = (Email)userCreated.Username;
            Password = userCreated.Password;
            Salt = userCreated.Salt;
            Roles = new HashSet<RoleReference>();
            PasswordResetRequired = userCreated.PasswordResetRequired;
        }

        protected void When(UserPasswordChanged userPasswordChanged)
        {
            Password = userPasswordChanged.Password;
        }

        protected void When(RoleReferenceAdded roleReferenceAdded)
        {
            Roles.Add(new RoleReference(roleReferenceAdded.RoleId));
        }

        protected void When(RoleReferenceRemoved roleReferenceRemoved)
        {
            Roles.Remove(new RoleReference(roleReferenceRemoved.RoleId));
        }

        public void When(UserRefreshTokenAdded userRefreshTokenAdded)
        {
            RefreshToken = userRefreshTokenAdded.RefreshToken;
        }

        protected override void EnsureValidState()
        {

        }

        public User ChangePassword(string password)
        {
            Apply(new UserPasswordChanged(password));

            return this;
        }

        public User AddRole(Guid roleId, string name)
        {
            Apply(new RoleReferenceAdded(roleId, name));

            return this;
        }

        public User RemoveRole(Guid roleId, string value)
        {
            Apply(new RoleReferenceRemoved(roleId, value));

            return this;
        }

        public User AddRefreshToken(string refreshToken)
        {
            Apply(new UserRefreshTokenAdded(refreshToken));

            return this;
        }
    }
}
