using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedList.API.Application.Commands.CreateEmptyList;
using SharedList.API.Application.Exceptions;
using SharedList.API.Tests.TestUtilities;
using SharedList.Core.Abstractions;
using SharedList.Core.Implementations;
using SharedList.Core.Models.DTOs;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence;

namespace SharedList.API.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - CreateEmptyList")]
    public class CreateEmptyListTests
    {
        private IConfiguration CreateConfigWithListLimit(int limit)
        {
            var configValues = new Dictionary<string, string>
            {
                { "Limits:Lists", $"{limit}" }
            };

            return new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        }

        private (SharedListContext, INowProvider, IRandomisedWordProvider, IConfiguration) CreateDeps(DateTime? dateTime = null, Random random = null)
        {
            return (
                Setup.CreateContext(),
                new TestNowProvider(dateTime),
                new RandomisedWordProvider(random ?? new Random()),
                CreateConfigWithListLimit(1000));
        }

        [TestMethod]
        public async Task AddsNewContributorForListAndUser()
        {
            const string USER = "user";
            const string LIST_ID = "list id";
            var idProviderMock = new Mock<IRandomisedWordProvider>();
            idProviderMock
                .Setup(m => m.CreateRandomId())
                .Returns(LIST_ID);

            var (context, nowProvider, _, config) = CreateDeps();

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, idProviderMock.Object, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(1, context.ListContributors.Count());
                Assert.AreEqual(LIST_ID, context.ListContributors.First().ListId);
                Assert.AreEqual(USER, context.ListContributors.First().UserIdent);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithCreatedNow()
        {
            const string USER = "user";
            var CREATED = new DateTime(2020, 01, 01);
            var (context, nowProvider, randomisedWordProvider, config) = CreateDeps(CREATED);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomisedWordProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(CREATED, context.Lists.First().Created);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithUpdatedNow()
        {
            const string USER = "user";
            var UPDATED = new DateTime(2020, 01, 01);
            var (context, nowProvider, randomisedWordProvider, config) = CreateDeps(UPDATED);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomisedWordProvider, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(UPDATED, context.Lists.First().Updated);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithRandomName()
        {
            const string USER = "user";
            const string LIST_ID = "list id";
            const string LIST_NAME = "list name";

            var (context, nowProvider, _, config) = CreateDeps();

            var randomiserMock = new Mock<IRandomisedWordProvider>();
            randomiserMock.Setup(m => m.CreateRandomName()).Returns(LIST_NAME);
            randomiserMock.Setup(m => m.CreateRandomId()).Returns(LIST_ID);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomiserMock.Object, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(LIST_NAME, context.Lists.First().Name);
            }
        }

        [TestMethod]
        public async Task AddsNewListToDBWithRandomId()
        {
            const string USER = "user";
            const string LIST_ID = "list id";

            var (context, nowProvider, _, config) = CreateDeps();

            var randomiserMock = new Mock<IRandomisedWordProvider>();
            randomiserMock.Setup(m => m.CreateRandomId()).Returns(LIST_ID);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomiserMock.Object, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(LIST_ID, context.Lists.First().Id);
            }
        }

        [TestMethod]
        public async Task ReturnsIdForList()
        {
            const string USER = "user";
            const string LIST_ID = "list id";
            var (context, nowProvider, _, config) = CreateDeps();

            var randomiserMock = new Mock<IRandomisedWordProvider>();
            randomiserMock.Setup(m => m.CreateRandomId()).Returns(LIST_ID);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomiserMock.Object, config);
                var result = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(LIST_ID, result);
            }
        }

        [TestMethod]
        public async Task ThrowsExceptionWhenLimitReached()
        {
            const string USER = "USER";
            var (context, nowProvider, randomiser, _) = CreateDeps();

            var config = CreateConfigWithListLimit(0);

            using (context)
            {
                var request = new CreateEmptyListRequest(USER);
                var handler = new CreateEmptyListHandler(context, nowProvider, randomiser, config);

                await Assert.ThrowsExceptionAsync<RequestFailedException>(() => handler.Handle(request, CancellationToken.None));
            }
        }
    }
}
