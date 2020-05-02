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
    [TestCategory("Mapping - ListItem")]
    public class ListItemMappingTests
    {
        private IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ListItemProfile>();
            }).CreateMapper();
        }

        [TestMethod]
        public void MapsListItemIdToListItemDTOId()
        {
            const int ID = 1;
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Id = ID
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(ID, dto.Id);
        }

        [TestMethod]
        public void MapsListItemValueToListItemDTOValue()
        {
            const string VALUE = "value";
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Value = VALUE
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(VALUE, dto.Value);
        }

        [TestMethod]
        public void MapsListItemNotesToListItemDTONotes()
        {
            const string NOTES = "notes";
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Notes = NOTES
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(NOTES, dto.Notes);
        }

        [TestMethod]
        public void MapsListItemCompletedToListItemDTOCompleted()
        {
            const bool COMPLETED = true;
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Completed = COMPLETED
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(COMPLETED, dto.Completed);
        }

        [TestMethod]
        public void MapsListItemCreatedToListItemDTOCreated()
        {
            var CREATED = new DateTime(2020, 1, 1);
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Created = CREATED
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(CREATED, dto.Created);
        }

        [TestMethod]
        public void MapsListItemUpdatedToListItemDTOUpdated()
        {
            var UPDATED = new DateTime(2020, 1, 1);
            var mapper = CreateMapper();

            var listItem = new ListItem
            {
                Updated = UPDATED
            };

            var dto = mapper.Map<ListItemDTO>(listItem);

            Assert.AreEqual(UPDATED, dto.Updated);
        }
    }
}
