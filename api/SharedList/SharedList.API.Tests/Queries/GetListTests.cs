using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Exceptions;
using SharedList.API.Application.Queries.GetList;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - GetList")]
    public class GetListTests
    {
        private (SharedListContext, IMapper) CreateDeps()
        {
            return (Setup.CreateContext(), new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ListProfile>();
                cfg.AddProfile<ListItemProfile>();
            }).CreateMapper());
        }

        [TestMethod]
        public async Task ThrowsIfNoListFound()
        {
            const string ID = "id";
            var (context, mapper) = CreateDeps();
            using (context)
            {
                var request = new GetListRequest(ID);
                var handler = new GetListHandler(context, mapper);
                await Assert.ThrowsExceptionAsync<RequestFailedException>(() => handler.Handle(request, CancellationToken.None));
            }
        }

        [TestMethod]
        public async Task ReturnsListDTOForList()
        {
            const string ID = "id";
            var (context, mapper) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID);
                var handler = new GetListHandler(context, mapper);
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
            var (context, mapper) = CreateDeps();
            using (context)
            {
                // seed DB
                context.Lists.Add(new List
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new GetListRequest(ID.ToLower());
                var handler = new GetListHandler(context, mapper);
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

            var (context, mapper) = CreateDeps();
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

                var request = new GetListRequest(ID);
                var handler = new GetListHandler(context, mapper);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(ORDER_ZERO_ID, result.Items.First().Id);
                Assert.AreEqual(ORDER_ONE_ID, result.Items.ToList()[1].Id);
            }
        }
    }
}
