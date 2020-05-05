using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedList.Core.Models.DTOs;
using SharedList.Core.Models.Mapping;
using SharedList.Persistence.Models.Entities;

namespace SharedList.API.Tests.Mapping
{
    [TestClass]
    [TestCategory("Mapping - List")]
    public class ListMappingTests
    {
        private IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ListProfile>();
                cfg.AddProfile<ListItemProfile>();
            }).CreateMapper();
        }

        [TestMethod]
        public void LISTDTO_MapsListIdToListDTOId()
        {
            const string ID = "id";
            var mapper = CreateMapper();

            var list = new List
            {
                Id = ID
            };

            var dto = mapper.Map<ListDTO>(list);

            Assert.AreEqual(ID, dto.Id);
        }

        [TestMethod]
        public void LISTDTO_MapsListNameToListDTOName()
        {
            const string NAME = "name";
            var mapper = CreateMapper();

            var list = new List
            {
                Name = NAME
            };

            var dto = mapper.Map<ListDTO>(list);

            Assert.AreEqual(NAME, dto.Name);
        }

        [TestMethod]
        public void LISTDTO_MapsListCreatedToListDTOCreated()
        {
            var CREATED = new DateTime(2020, 1, 1);
            var mapper = CreateMapper();

            var list = new List
            {
                Created = CREATED
            };

            var dto = mapper.Map<ListDTO>(list);

            Assert.AreEqual(CREATED, dto.Created);
        }

        [TestMethod]
        public void LISTDTO_MapsListUpdatedToListDTOUpdated()
        {
            var UPDATED = new DateTime(2020, 2, 1);
            var mapper = CreateMapper();

            var list = new List
            {
                Updated = UPDATED
            };

            var dto = mapper.Map<ListDTO>(list);

            Assert.AreEqual(UPDATED, dto.Updated);
        }

        [TestMethod]
        public void LISTDTO_MapsListItemsToListDTOItems()
        {
            var mapper = CreateMapper();

            var list = new List
            {
                Items = new List<ListItem>
                {
                    new ListItem()
                }
            };

            var dto = mapper.Map<ListDTO>(list);

            Assert.AreEqual(1, dto.Items.Count);
        }

        [TestMethod]
        public void LISTNAMEANDIDDTO_MapsListIdToDTOId()
        {
            var mapper = CreateMapper();
            const string ID = "1";

            var list = new List
            {
                Id = ID
            };

            var dto = mapper.Map<ListNameAndIdDTO>(list);

            Assert.AreEqual(ID, dto.Id);
        }

        [TestMethod]
        public void LISTNAMEANDIDDTO_MapsListNameToDTOName()
        {
            var mapper = CreateMapper();
            const string NAME = "1";

            var list = new List
            {
                Name = NAME
            };

            var dto = mapper.Map<ListNameAndIdDTO>(list);

            Assert.AreEqual(NAME, dto.Name);
        }
    }
}
