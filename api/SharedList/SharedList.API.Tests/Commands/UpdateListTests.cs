using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - Update List")]
    public class UpdateListTests
    {
        private (SharedListContext, INowProvider) CreateDeps(string databaseName = null, DateTime? dateTime = null)
        {
            return (Setup.CreateContext(databaseName), new TestNowProvider(dateTime));
        }

        [TestMethod]
        public async Task DoesNothingIfNoExistingFoundForId()
        {
            const string ID = "id";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                var dto = new ListDTO
                {
                    Id = ID
                };
                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                // assert nothing an exception means failure
            }
        }

        [TestMethod]
        public async Task UpdatesNameOfExistingListWithDTOName()
        {
            const string ID = "id";
            const string OLD_NAME = "old name";
            const string NEW_NAME = "new name";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID,
                    Name = OLD_NAME
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                    Name = NEW_NAME
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(NEW_NAME, context.Lists.First().Name);
            }
        }

        [TestMethod]
        public async Task UpdatesUpdatedDateOfExistingListWithNow()
        {
            const string ID = "id";
            var updatedDate = new DateTime(2020, 1, 1);
            var (context, nowProvider) = CreateDeps(dateTime: updatedDate);
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(updatedDate, context.Lists.First().Updated);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemValues()
        {
            const string ID = "id";
            const string ITEM_ID = "1";
            const string ITEM_VALUES = "value";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Value = ITEM_VALUES
                        }
                    }
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(ITEM_VALUES, context.ListItems.First().Value);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemNotes()
        {
            const string ID = "id";
            const string ITEM_ID = "1";
            const string ITEM_NOTES = "notes";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Notes = ITEM_NOTES
                        }
                    }
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(ITEM_NOTES, context.ListItems.First().Notes);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemCompleted()
        {
            const string ID = "id";
            const string ITEM_ID = "1";
            const bool COMPLETED = true;
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Completed = COMPLETED
                        }
                    }
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(COMPLETED, context.ListItems.First().Completed);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemCreated()
        {
            const string ID = "id";
            const string ITEM_ID = "1";
            var created = new DateTime(2020, 2, 1);
            var (context, nowProvider) = CreateDeps(dateTime: created);
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID
                        }
                    }
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(created, context.ListItems.First().Created);
            }
        }

        [TestMethod]
        public async Task RemovesOldListItemsForList()
        {
            const string ID = "id";
            const string ITEM_ID = "1";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID,
                    Items = new List<ListItem>
                    {
                        new ListItem()
                        {
                            Id = ITEM_ID
                        }
                    }
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, context.ListItems.Count());
            }
        }

        [TestMethod]
        public async Task DoesNotUpdateCreatedDate()
        {
            const string ID = "id";
            var created = new DateTime(2020, 1, 1);
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID,
                    Created = created,
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(created, context.Lists.First().Created);
            }
        }

        [TestMethod]
        public async Task ReturnsUnitValue()
        {
            const string ID = "id";
            var (context, nowProvider) = CreateDeps();
            using (context)
            {
                var dto = new ListDTO
                {
                    Id = ID,
                };

                var request = new UpdateListRequest(dto);
                var handler = new UpdateListHandler(context, nowProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(Unit.Value, result);
            }
        }
    }
}
