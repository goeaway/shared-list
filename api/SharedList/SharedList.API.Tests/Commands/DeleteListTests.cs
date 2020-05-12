using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.DeleteList;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Models.Entities;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - DeleteList")]
    public class DeleteListTests
    {
        [TestMethod]
        public async Task DoesNothingIfNoListFoundWithId()
        {
            const string ID = "id";
            using (var context = Setup.CreateContext())
            {
                var request = new DeleteListRequest(ID);
                var handler = new DeleteListHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                // no assertion, as long as no exception we're fine
            }
        }

        [TestMethod]
        public async Task RemovesExistingListWithId()
        {
            const string ID = "id";
            using (var context = Setup.CreateContext())
            {
                // setup existing list
                context.Lists.Add(new List
                {
                    Id = "id"
                });
                context.SaveChanges();

                var request = new DeleteListRequest(ID);
                var handler = new DeleteListHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, context.Lists.Count());
            }
        }

        [TestMethod]
        public async Task RemovesExistingListContributorsForList()
        {
            const string ID = "id";
            const string USER = "user";
            using (var context = Setup.CreateContext())
            {
                context.Lists.Add(new List
                {
                    Id = "id"
                });

                context.ListContributors.Add(new ListContributor
                {
                    ListId = ID,
                    UserIdent = USER
                });

                context.SaveChanges();

                var request = new DeleteListRequest(ID);
                var handler = new DeleteListHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, context.ListContributors.Count());
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyUnit()
        {
            using (var context = Setup.CreateContext())
            {
                var request = new DeleteListRequest("n/a");
                var handler = new DeleteListHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(Unit.Value, result);
            }
        }
    }
}
