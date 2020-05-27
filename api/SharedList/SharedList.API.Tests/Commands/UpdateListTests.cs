using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedList.API.Application.Commands.UpdateList;
using SharedList.API.Application.Exceptions;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Abstractions;
using SharedList.Core.Models.DTOs;
using SharedList.Core.Models.Entities;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - Update List")]
    public class UpdateListTests
    {
        private IConfiguration CreateConfigWithListItemLimit(int limit)
        {
            var configValues = new Dictionary<string, string>
            {
                { "Limits:ListItems", $"{limit}" }
            };

            return new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        }

        private (SharedListContext, INowProvider, IConfiguration) CreateDeps(string databaseName = null, DateTime? dateTime = null)
        {
            return (Setup.CreateContext(databaseName), new TestNowProvider(dateTime), CreateConfigWithListItemLimit(1000));
        }

        [TestMethod]
        public async Task DoesNothingIfNoExistingFoundForId()
        {
            const string USER = "user";
            const string ID = "id";
            var (context, nowProvider, config) = CreateDeps();
            using (context)
            {
                var dto = new ListDTO
                {
                    Id = ID
                };
                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                // assert nothing an exception means failure
            }
        }

        [TestMethod]
        public async Task UpdatesNameOfExistingListWithDTOName()
        {
            const string USER = "user";
            const string ID = "id";
            const string OLD_NAME = "old name";
            const string NEW_NAME = "new name";
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(NEW_NAME, context.Lists.First().Name);
            }
        }

        [TestMethod]
        public async Task UpdatesUpdatedDateOfExistingListWithNow()
        {
            const string USER = "user";
            const string ID = "id";
            var updatedDate = new DateTime(2020, 1, 1);
            var (context, nowProvider, config) = CreateDeps(dateTime: updatedDate);
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(updatedDate, context.Lists.First().Updated);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemValues()
        {
            const string USER = "user";
            const string ID = "id";
            const string ITEM_ID = "1";
            const string ITEM_VALUES = "value";
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(ITEM_VALUES, context.ListItems.First().Value);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemNotes()
        {
            const string USER = "user";
            const string ID = "id";
            const string ITEM_ID = "1";
            const string ITEM_NOTES = "notes";
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(ITEM_NOTES, context.ListItems.First().Notes);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemCompleted()
        {
            const string USER = "user";
            const string ID = "id";
            const string ITEM_ID = "1";
            const bool COMPLETED = true;
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(COMPLETED, context.ListItems.First().Completed);
            }
        }

        [TestMethod]
        public async Task UpdatesListItemCreated()
        {
            const string USER = "user";
            const string ID = "id";
            const string ITEM_ID = "1";
            var created = new DateTime(2020, 2, 1);
            var (context, nowProvider, config) = CreateDeps(dateTime: created);
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(created, context.ListItems.First().Created);
            }
        }

        [TestMethod]
        public async Task AddsListContributorIfNoneExistForListAndUser()
        {
            const string USER = "user";
            const string ID = "id";
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListContributors.Count());
                Assert.AreEqual(ID, context.ListContributors.First().ListId);
                Assert.AreEqual(USER, context.ListContributors.First().UserIdent);
            }
        }

        [TestMethod]
        public async Task DoesNotAddListContributorIfOneExistsForListAndUser()
        {
            const string USER = "user";
            const string ID = "id";
            var (context, nowProvider, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.ListContributors.Add(new ListContributor
                {
                    UserIdent = USER,
                    ListId = ID
                });

                context.SaveChanges();

                var dto = new ListDTO
                {
                    Id = ID,
                };

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListContributors.Count());
                Assert.AreEqual(ID, context.ListContributors.First().ListId);
                Assert.AreEqual(USER, context.ListContributors.First().UserIdent);
            }
        }

        [TestMethod]
        public async Task RemovesOldListItemsForList()
        {
            const string USER = "user";
            const string ID = "id";
            const string ITEM_ID = "1";
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, context.ListItems.Count());
            }
        }

        [TestMethod]
        public async Task DoesNotUpdateCreatedDate()
        {
            const string USER = "user";
            const string ID = "id";
            var created = new DateTime(2020, 1, 1);
            var (context, nowProvider, config) = CreateDeps();
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

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(created, context.Lists.First().Created);
            }
        }

        [TestMethod]
        public async Task ReturnsUnitValue()
        {
            const string USER = "user";
            const string ID = "id";
            var (context, nowProvider, config) = CreateDeps();
            using (context)
            {
                var dto = new ListDTO
                {
                    Id = ID,
                };

                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(Unit.Value, result);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenListItemLimitReached()
        {
            const string USER = "user";
            const string ID = "id";
            var (context, nowProvider, _) = CreateDeps();

            var config = CreateConfigWithListItemLimit(0);

            using (context)
            {
                var dto = new ListDTO
                {
                    Id = ID
                };
                var request = new UpdateListRequest(dto, USER);
                var handler = new UpdateListHandler(context, nowProvider, config);

                await Assert.ThrowsExceptionAsync<RequestFailedException>(() => handler.Handle(request, CancellationToken.None));
            }
        }
    }
}
