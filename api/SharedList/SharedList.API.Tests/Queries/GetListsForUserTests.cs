using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Queries.GetListsForUser;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Models.Entities;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - GetListsForUser")]
    public class GetListsForUserTests
    {
        private (SharedListContext, IMapper) CreateDeps()
        {
            return (
                Setup.CreateContext(),
                new MapperConfiguration(cfg => cfg.AddProfile<ListProfile>()).CreateMapper());
        }

        [TestMethod]
        public async Task ReturnsEmptyListDTOCollectionWhenNoneFoundForUserIdent()
        {
            var (context, mapper) = CreateDeps();
            const string USER = "user";

            using (context)
            {
                var request = new GetListsForUserRequest(USER);
                var handler = new GetListsForUserHandler(context, mapper);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, result.Count());
            }
        }

        [TestMethod]
        public async Task ReturnsCollectionOfListDTOsForUserIdent()
        {
            var (context, mapper) = CreateDeps();
            const string USER = "user", LIST_ID_1 = "id1", LIST_ID_2 = "id2";

            using (context)
            {
                context.Lists.Add(new List
                {
                    Id = LIST_ID_1
                });
                context.Lists.Add(new List
                {
                    Id = LIST_ID_2
                });

                context.ListContributors.Add(new ListContributor
                {
                    ListId = LIST_ID_1,
                    UserIdent = USER
                });

                context.ListContributors.Add(new ListContributor
                {
                    ListId = LIST_ID_2,
                    UserIdent = USER
                });

                context.SaveChanges();

                var request = new GetListsForUserRequest(USER);
                var handler = new GetListsForUserHandler(context, mapper);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(LIST_ID_1, result.First().Id);
                Assert.AreEqual(LIST_ID_2, result.ToList()[1].Id);
            }
        }
    }
}
