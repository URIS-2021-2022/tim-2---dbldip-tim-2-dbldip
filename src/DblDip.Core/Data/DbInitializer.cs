using BuildingBlocks.EventStore;
using DblDip.Core;
using DblDip.Core.Data;
using DblDip.Core.Models;
using DblDip.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using static DblDip.Core.Constants.Rates;

namespace DblDip.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IDblDipDbContext context, IEventStore store, IConfiguration configuration)
        {
            RoleConfiguration.Seed(store);
            SystemLocationConfiguration.Seed(store, configuration);
            CardConfiguration.Seed(store, context);
            UserConfiguration.Seed(context, store);
            DashboardConfiguration.Seed(context, store);
        }

        internal static class RoleConfiguration
        {
            public static void Seed(IEventStore store)
            {
                AddRoleIfDoesntExists(store, Constants.Roles.Lead, nameof(Constants.Roles.Lead));

                AddRoleIfDoesntExists(store, Constants.Roles.Client, nameof(Constants.Roles.Client));

                AddRoleIfDoesntExists(store, Constants.Roles.Photographer, nameof(Constants.Roles.Photographer));

                AddRoleIfDoesntExists(store, Constants.Roles.ProjectManager, nameof(Constants.Roles.ProjectManager));

                AddRoleIfDoesntExists(store, Constants.Roles.SystemAdministrator, nameof(Constants.Roles.SystemAdministrator));

                void AddRoleIfDoesntExists(IEventStore store, Guid roleId, string name)
                {

                    var role = store.FindAsync<Role>(roleId).GetAwaiter().GetResult();

                    role ??= new Role(roleId, name);

                    if (role.DomainEvents.Any())
                    {
                        store.Add(role);
                        if (store.SaveChangesAsync(default).GetAwaiter().IsCompleted)
                            store.SaveChangesAsync(default).GetAwaiter().GetResult();
                    }
                }
            }
        }

        internal static class CardConfiguration
        {
            public static void Seed(IEventStore store, IDblDipDbContext context)
            {

                var card = context.Cards.SingleOrDefault(x => x.Name == "Leads");

                if (card == null)
                {
                    card = new Card("Leads", "");

                    store.Add(card);

                    store.SaveChangesAsync(default).GetAwaiter().GetResult();
                }
            }
        }

        internal static class UserConfiguration
        {
            public static void Seed(IDblDipDbContext context, IEventStore store)
            {
                var username = (Email)"quinntynebrown@gmail.com";

                var user = context.Users.FirstOrDefault(x => x.Username == username);

                if (user == null)
                {
                    user = new User(username, "dbldip");

                    user.AddRole(Constants.Roles.SystemAdministrator, nameof(Constants.Roles.SystemAdministrator));

                    var sysAdmin = new SystemAdministrator("Quinntyne", user.Username);

                    var account = new Account(sysAdmin.ProfileId, sysAdmin.Name, user.UserId);

                    sysAdmin.UpdateAccountId(account.AccountId);

                    store.Add(user);

                    store.Add(sysAdmin);

                    store.Add(account);

                    store.SaveChangesAsync(default).GetAwaiter().GetResult();
                }
            }
        }

        internal static class DashboardConfiguration
        {
            public static void Seed(IDblDipDbContext context, IEventStore store)
            {

                var profile = context.Profiles.Single(x => x.Name == "Quinntyne");

                var dashboard = context.Dashboards.FirstOrDefault(x => x.Name == "Default" && x.ProfileId == profile.ProfileId);

                if (dashboard == null)
                {
                    dashboard = new Dashboard("Default", profile.ProfileId);

                    store.Add(dashboard);

                    store.SaveChangesAsync(default).GetAwaiter().GetResult();
                }
            }
        }

        internal static class SystemLocationConfiguration
        {
            public static void Seed(IEventStore store, IConfiguration configuration)
            {
                throw new NotSupportedException();
            }
        }
    }
}
