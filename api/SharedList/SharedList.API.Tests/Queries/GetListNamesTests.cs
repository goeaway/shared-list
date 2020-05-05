using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Queries.GetListName;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - GetListNames")]
    public class GetListNamesTests
    {
        private (SharedListContext, IMapper) CreateDeps()
        {
            return (
                Setup.CreateContext(), 
                new MapperConfiguration(cfg => cfg.AddProfile<ListProfile>()).CreateMapper());
        }

        [TestMethod]
        public async Task ReturnsListNameAndIdDTOCollectionForLists()
        {
            var (context, mapper) = CreateDeps();
            const string LIST_1_ID = "1", LIST_2_ID = "2";

            using (context)
            {
                context.Lists.Add(new List
                {
                    Id = LIST_1_ID
                });

                context.Lists.Add(new List
                {
                    Id = LIST_2_ID
                });

                context.SaveChanges();

                var request = new GetListNamesRequest(new[] {LIST_1_ID, LIST_2_ID});
                var handler = new GetListNamesHandler(context, mapper);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(LIST_1_ID, result.First().Id);
                Assert.AreEqual(LIST_2_ID, result.ToList()[1].Id);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyIfNothingFound()
        {
            var (context, mapper) = CreateDeps();
            const string LIST_1_ID = "1", LIST_2_ID = "2";

            using (context)
            {
                var request = new GetListNamesRequest(new[] { LIST_1_ID, LIST_2_ID });
                var handler = new GetListNamesHandler(context, mapper);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, result.Count());
            }
        }
    }
}
