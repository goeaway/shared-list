using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.API.Application.Commands.CreateList;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Abstractions;
using SharedList.Core.Implementations;
using SharedList.Core.Models.DTOs;
using SharedList.Persistence;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - CreateList")]
    public class CreateListTests
    {
        private (SharedListContext, INowProvider, IRandomisedWordProvider) CreateDeps(DateTime? dateTime = null, Random random = null)
        {
            return (
                Setup.CreateContext(), 
                new TestNowProvider(dateTime),
                new RandomisedWordProvider(random ?? new Random()));
        } 

        [TestMethod]
        public async Task ReturnsAnIdForAList()
        {
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO();
                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.IsFalse(string.IsNullOrWhiteSpace(result));
                Assert.AreEqual(result, context.Lists.First().Id);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOName()
        {
            const string EXPECTED_NAME = "test name";

            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Name = EXPECTED_NAME
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.Lists.Count());
                Assert.AreEqual(EXPECTED_NAME, context.Lists.First().Name);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItem()
        {
            const string ITEM_ID = "1";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.Lists.Count());
                Assert.AreEqual(1, context.ListItems.Count());
                Assert.AreEqual(context.Lists.First(), context.ListItems.First().ParentList);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItemWithId()
        {
            const string EXPECTED_ITEM_ID = "item id";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = EXPECTED_ITEM_ID
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_ITEM_ID, context.ListItems.First().Id);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItemWithValue()
        {
            const string EXPECTED_ITEM_VALUE = "item value";
            const string ITEM_ID = "1";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Value = EXPECTED_ITEM_VALUE
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_ITEM_VALUE, context.ListItems.First().Value);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItemWithNotes()
        {
            const string EXPECTED_ITEM_NOTES = "item notes";
            const string ITEM_ID = "1";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Notes = EXPECTED_ITEM_NOTES
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_ITEM_NOTES, context.ListItems.First().Notes);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItemWithCompleted()
        {
            const bool EXPECTED_ITEM_COMPLETED = true;
            const string ITEM_ID = "1";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps();

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID,
                            Completed = EXPECTED_ITEM_COMPLETED
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_ITEM_COMPLETED, context.ListItems.First().Completed);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithDTOListItemWithCreatedNow()
        {
            var EXPECTED_DATE = new DateTime(2020, 1, 1);
            const string ITEM_ID = "1";
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps(EXPECTED_DATE);

            using (context)
            {
                var dto = new ListDTO
                {
                    Items = new List<ListItemDTO>
                    {
                        new ListItemDTO
                        {
                            Id = ITEM_ID
                        }
                    }
                };

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_DATE, context.ListItems.First().Created);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithCreatedNow()
        {
            var EXPECTED_DATE = new DateTime(2020, 2, 1);
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps(EXPECTED_DATE);

            using (context)
            {
                var dto = new ListDTO();

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_DATE, context.Lists.First().Created);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithUpdatedNow()
        {
            var EXPECTED_DATE = new DateTime(2020, 2, 1);
            var (context, nowProvider, randomisedWordsProvider) = CreateDeps(EXPECTED_DATE);

            using (context)
            {
                var dto = new ListDTO();

                var request = new CreateListRequest(dto);
                var handler = new CreateListHandler(context, nowProvider, randomisedWordsProvider);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(EXPECTED_DATE, context.Lists.First().Updated);
            }
        }
    }
}
