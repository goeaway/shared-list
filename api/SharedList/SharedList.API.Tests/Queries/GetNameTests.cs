using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedList.API.Application.Queries.GetName;
using SharedList.Core.Abstractions;
using SharedList.Core.Implementations;
using SharedList.Core.Models.DTOs;

namespace SharedList.API.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - GetName")]
    public class GetNameTests
    {
        [TestMethod]
        public async Task UsesRandomisedWordProviderToGetName()
        {
            const string NAME = "name";
            var randomiserMock = new Mock<IRandomisedWordProvider>();
            randomiserMock
                .Setup(m => m.CreateRandomName())
                .Returns(NAME);
            var request = new GetNameRequest();
            var handler = new GetNameHandler(randomiserMock.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(NAME, result);
        }
    }
}
