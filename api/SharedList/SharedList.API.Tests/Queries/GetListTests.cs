using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Exceptions;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Models.Entities;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - GetList")]
    public class GetListTests
    {
        private (SharedListContext, IMapper, IConfiguration) CreateDeps(int listLimit = 10)
        {
            var configValues = new Dictionary<string, string>
            {
                { "Limits:Lists", $"{listLimit}" }
            };

            return (Setup.CreateContext(), new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ListProfile>();
                cfg.AddProfile<ListItemProfile>();
            }).CreateMapper(),
            new ConfigurationBuilder().AddInMemoryCollection(configValues).Build());
        }

        [TestMethod]
        public async Task ThrowsIfNoListFound()
        {
            const string ID = "id";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);
                await Assert.ThrowsExceptionAsync<RequestFailedException>(() => handler.Handle(request, CancellationToken.None));
            }
        }

        [TestMethod]
        public async Task ThrowsIfUserReachedMaxBeforeContributing()
        {
            const string ID = "id";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps(0);
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);

                try
                {
                    await handler.Handle(request, CancellationToken.None);
                    Assert.Fail("exception was not thrown");
                }
                catch(RequestFailedException e)
                {
                    Assert.AreEqual(HttpStatusCode.Forbidden, e.Code);
                }
                catch (Exception)
                {
                    Assert.Fail("didn't throw correct exception");
                }
            }
        }

        [TestMethod]
        public async Task DoesNotThrowIfUserReachedMaxAfterContributing()
        {
            const string ID = "id";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps(0);
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.ListContributors.Add(new ListContributor
                {
                    ListId = ID,
                    UserIdent = USER
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);

                var result = await handler.Handle(request, CancellationToken.None);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnsListDTOForList()
        {
            const string ID = "id";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);
                var result = await handler.Handle(request, CancellationToken.None);

                // only check it's the right one, let the mapping tests check the properties
                Assert.IsNotNull(result);
                Assert.AreEqual(ID, result.Id);
            }
        }

        [TestMethod]
        public async Task ReturnsListDTOForListIgnoresIdCase()
        {
            const string ID = "id";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID.ToLower(), USER);
                var handler = new GetListHandler(context, mapper, config);
                var result = await handler.Handle(request, CancellationToken.None);

                // only check it's the right one, let the mapping tests check the properties
                Assert.IsNotNull(result);
                Assert.AreEqual(ID, result.Id);
            }
        }

        [TestMethod]
        public async Task ReturnsListDTOWithOrderedItems()
        {
            const string ID = "id";
            const string ORDER_ONE_ID = "1";
            const string ORDER_ZERO_ID = "0";
            const string USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID,
                    Items = new List<ListItem>
                    {
                        new ListItem
                        {
                            Id = ORDER_ONE_ID,
                            Order = 1
                        },
                        new ListItem
                        {
                            Id = ORDER_ZERO_ID,
                            Order = 0
                        }
                    }
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(ORDER_ZERO_ID, result.Items.First().Id);
                Assert.AreEqual(ORDER_ONE_ID, result.Items.ToList()[1].Id);
            }
        }

        [TestMethod]
        public async Task AddsListContributorRecordForUserAndList()
        {
            const string ID = "id", USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);
                await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListContributors.Count());
                Assert.AreEqual(ID, context.ListContributors.First().ListId);
                Assert.AreEqual(USER, context.ListContributors.First().UserIdent);
            }
        }

        [TestMethod]
        public async Task DoesNotAddListContributorRecordForUserAndListIfAlreadyExists()
        {
            const string ID = "id", USER = "user";
            var (context, mapper, config) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.ListContributors.Add(new ListContributor
                {
                    ListId = ID,
                    UserIdent = USER
                });

                context.SaveChanges();

                var request = new GetListRequest(ID, USER);
                var handler = new GetListHandler(context, mapper, config);
                await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListContributors.Count());
                Assert.AreEqual(ID, context.ListContributors.First().ListId);
                Assert.AreEqual(USER, context.ListContributors.First().UserIdent);
            }
        }
    }
}
