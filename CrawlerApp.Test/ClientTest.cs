using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrawlerApp.Client.Controllers;
using CrawlerApp.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrawlerApp.Test
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public async Task Get_links_success()
        {
            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            
            var context = new CrawlerContext(options);
            context.Add(fakeLink);
            context.SaveChanges();
            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.GetLinks();

            //Assert
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Link>));
        }

        [TestMethod]
        public async Task Get_link_success()
        {
            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            var context = new CrawlerContext(options);
            context.Add(fakeLink);
            context.SaveChanges();
            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.GetLink(1);

            //Assert
            Assert.IsInstanceOfType(actionResult.Value, typeof(Link));
        }

        [TestMethod]
        public async Task Get_link_fail()
        {
            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            var context = new CrawlerContext(options);
            context.Add(fakeLink);
            context.SaveChanges();
            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.GetLink(5);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post_links_success()
        {
            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            var context = new CrawlerContext(options);

            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.PostLink(fakeLink);
            
            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Link>));
        }

        [TestMethod]
        public async Task Post_links_fail()
        {
            //attempt to post existing link

            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            var context = new CrawlerContext(options);
            context.Add(fakeLink);
            context.SaveChanges();

            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.PostLink(fakeLink);

            //Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(ConflictResult));
        }

        [TestMethod]
        public async Task Put_links_success()
        {
            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            var context = new CrawlerContext(options);

            context.Add(fakeLink);
            context.SaveChanges();

            fakeLink.Address = "http://www.test.com";

            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.PutLink((int)fakeLink.ID, fakeLink);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Put_links_fail()
        {
            //attempt to post existing link

            //Arrange
            var fakeLink = new Link
            {
                FoundOn = "http://www.google.com",
                Address = "http://www.test.com",
                Date = DateTime.Now,
                IsCrawled = true,
                Response = "Fake response"
            };
            
            var options = new DbContextOptionsBuilder<CrawlerContext>()
                .UseInMemoryDatabase("Test")
                .Options;
            var context = new CrawlerContext(options);
            context.Add(fakeLink);
            context.SaveChanges();

            //Act
            var linksController = new LinksController(context);
            var actionResult = await linksController.PutLink(100, fakeLink);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

    }
}
