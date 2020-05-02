using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.DeleteListItem;
using SharedList.API.Tests.TestUtilities;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - DeleteListItem")]
    public class DeleteListItemTests
    {
        [TestMethod]
        public async Task DoesNothingIfNoListItemWithIdFound()
        {
            const int ID = 1;
            using (var context = Setup.CreateContext())
            {
                var request = new DeleteListItemRequest(ID);
                var handler = new DeleteListItemHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                // exception means failure, no need for assertion
            }
        }

        [TestMethod]
        public async Task RemovesItemFromDB()
        {
            const int ID = 1;
            using (var context = Setup.CreateContext())
            {
                context.ListItems.Add(new ListItem
                {
                    Id = ID
                });

                context.SaveChanges();

                var request = new DeleteListItemRequest(ID);
                var handler = new DeleteListItemHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(0, context.ListItems.Count());
            }
        }

        [TestMethod]
        public async Task ReturnsUnitValue()
        {
            const int ID = 1;
            using (var context = Setup.CreateContext())
            {
                var request = new DeleteListItemRequest(ID);
                var handler = new DeleteListItemHandler(context);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(Unit.Value, result);
            }
        }
    }
}
