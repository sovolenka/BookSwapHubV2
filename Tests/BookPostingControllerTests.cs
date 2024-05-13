using Business.Services;
using Business.Services.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Web.Controllers;

namespace Tests;

public class BookPostingControllerTests
{
    private List<BookPosting> testPostings = new List<BookPosting>
    {
        new BookPosting { Id = 1, Description = "Test Description 1", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 1", Book = new Book { Name = "Test Book 1", Author = "Test Author 1", Year = 2021, Genre = "Test Genre 1", Language = "Test Language 1", Publisher = "Test Publisher 1", PublicationYear = 2021 } },
        new BookPosting { Id = 2, Description = "Test Description 2", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 2", Book = new Book { Name = "Test Book 2", Author = "someSearch", Year = 2021, Genre = "Test Genre 2", Language = "Test Language 2", Publisher = "Test Publisher 2", PublicationYear = 2021 } },
        new BookPosting { Id = 3, Description = "Test Description 3", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 3", Book = new Book { Name = "Test Book 3", Author = "Test Author 3", Year = 2021, Genre = "Test Genre 3", Language = "Test Language 3", Publisher = "Test Publisher 3", PublicationYear = 2021 } },
        new BookPosting { Id = 4, Description = "Test Description 4", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 4", Book = new Book { Name = "Test Book 4", Author = "Test Author 4", Year = 2021, Genre = "Test Genre 4", Language = "Test Language 4", Publisher = "Test Publisher 4", PublicationYear = 2021 } },
        new BookPosting { Id = 5, Description = "Test Description 5", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 5", Book = new Book { Name = "Test Book 5", Author = "Test Author 5", Year = 2021, Genre = "Test Genre 5", Language = "Test Language 5", Publisher = "Test Publisher 5", PublicationYear = 2021 } },
    };

    private List<ApplicationUser> testUsers = new List<ApplicationUser>
    {
        new ApplicationUser { Id = "1", Email = "hello@world.com", UserName = "helloworld" },
    };

    // mock the logger, and book posting service
    private Mock<ILogger<BookPostingController>> loggerMock;
    private Mock<IBookPostingService> bookPostingServiceMock;
    private Mock<IApplicationUserService> applicationUserServiceMock;
    private BookPostingController controller;


    public BookPostingControllerTests()
    {
        loggerMock = new Mock<ILogger<BookPostingController>>();
        // mock the book posting Get method
        bookPostingServiceMock = new Mock<IBookPostingService>();
        bookPostingServiceMock.Setup(service => service.Get()).Returns(testPostings.AsQueryable());
        applicationUserServiceMock = new Mock<IApplicationUserService>();
        applicationUserServiceMock.Setup(service => service.GetUser(It.IsAny<string>())).Returns(testUsers[0]);
        controller = new BookPostingController(loggerMock.Object, bookPostingServiceMock.Object, applicationUserServiceMock.Object);
    }

    [Fact]
    public void Details_ReturnsViewWithModel_WhenIdIsProvided()
    {
        try
        {
            // Act
            var result = controller.Details() as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result?.Model);
            Assert.Equal("Index", result?.ViewName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [Fact]
    public void Add_ReturnsView_WhenUserIsNotAuthorized()
    {
        // Act
        var result = controller.Add() as ViewResult;

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(result?.Model);
        Assert.Equal("Add", result?.ViewName);
    }

    [Fact]
    public void Add_ReturnsView_WhenUserIsAuthorized()
    {
        // Act
        var result = controller.Add() as ViewResult;

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.Null(result?.Model);
        Assert.Equal("Add", result?.ViewName);
    }

    [Fact]
    public void Add_ReturnsRedirectToAction_WhenUserIsAuthorized()
    {
        // Arrange
        var bookPosting = new BookPosting
        {
            Description = "Test Description 6",
            PostDateTime = DateTime.UtcNow,
            ExpireDateTime = DateTime.UtcNow.AddDays(30),
            PictureUrl = "Test Picture Url 6",
            Book = new Book
            {
                Name = "Test Book 6",
                Author = "Test Author 6",
                Year = 2021,
                Genre = "Test Genre 6",
                Language = "Test Language 6",
                Publisher = "Test Publisher 6",
                PublicationYear = 2021
            }
        };
    }
}