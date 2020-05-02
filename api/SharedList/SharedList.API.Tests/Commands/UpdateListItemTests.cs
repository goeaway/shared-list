using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.UpdateListItem;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - UpdateListItem")]
    public class UpdateListItemTests
    {
        private (SharedListContext, INowProvider) CreateDeps(DateTime? dateTime = null)
        {
            return (Setup.CreateContext(), new TestNowProvider(dateTime));
        }

        [TestMethod]
        public async Task DoesNothingIfNoItemFoundWithID()
        {
            const int ID = 1;
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                var dto = new ListItemDTO
                {
                    Id = ID
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                // exception would be failure
            }
        }

        [TestMethod]
        public async Task UpdatesValueFromDTO()
        {
            const int ID = 1;
            const string NEW_VALUE = "new value";

            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListItemDTO
                {
                    Id = ID,
                    Value = NEW_VALUE
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(NEW_VALUE, context.ListItems.First().Value);
            }
        }

        [TestMethod]
        public async Task UpdatesNotesFromDTO()
        {
            const int ID = 1;
            const string NEW_NOTES = "new notes";

            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListItemDTO
                {
                    Id = ID,
                    Notes = NEW_NOTES
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(NEW_NOTES, context.ListItems.First().Notes);
            }
        }

        [TestMethod]
        public async Task UpdatesCompletedFromDTO()
        {
            const int ID = 1;
            const bool NEW_VALUE = true;

            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListItemDTO
                {
                    Id = ID,
                    Completed = NEW_VALUE
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(NEW_VALUE, context.ListItems.First().Completed);
            }
        }

        [TestMethod]
        public async Task UpdatesUpdatedFromDTO()
        {
            const int ID = 1;
            var updated = new DateTime(2020, 1, 1);
            var (context, nowProvider) = CreateDeps(updated);
            using (context)
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListItemDTO
                {
                    Id = ID
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(updated, context.ListItems.First().Updated);
            }
        }

        [TestMethod]
        public async Task DoesNotUpdateCreatedDate()
        {
            const int ID = 1;
            var created = new DateTime(2020, 1, 1);
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID,
                    Created = created
                });

                context.SaveChanges();

                var dto = new ListItemDTO
                {
                    Id = ID
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(created, context.ListItems.First().Created);
            }
        }

        [TestMethod]
        public async Task ReturnsUnitValue()
        {
            const int ID = 1;
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                var dto = new ListItemDTO
                {
                    Id = ID
                };
                var request = new UpdateListItemRequest(dto);
                var handler = new UpdateListItemHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(Unit.Value, result);
            }
        }
    }
}
