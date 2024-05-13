using Moq;
using Data.Models;
using Data;
using Business.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Controllers;

namespace Tests;

public class HomeControllerTests
{
    private List<BookPosting> testPostings = new List<BookPosting>
    {
        new BookPosting { Id = 1, Description = "Test Description 1", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 1", Book = new Book { Name = "Test Book 1", Author = "Test Author 1", Year = 2021, Genre = "Test Genre 1", Language = "Test Language 1", Publisher = "Test Publisher 1", PublicationYear = 2021 } },
        new BookPosting { Id = 2, Description = "Test Description 2", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 2", Book = new Book { Name = "Test Book 2", Author = "someSearch", Year = 2021, Genre = "Test Genre 2", Language = "Test Language 2", Publisher = "Test Publisher 2", PublicationYear = 2021 } },
        new BookPosting { Id = 3, Description = "Test Description 3", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 3", Book = new Book { Name = "Test Book 3", Author = "Test Author 3", Year = 2021, Genre = "Test Genre 3", Language = "Test Language 3", Publisher = "Test Publisher 3", PublicationYear = 2021 } },
        new BookPosting { Id = 4, Description = "Test Description 4", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 4", Book = new Book { Name = "Test Book 4", Author = "Test Author 4", Year = 2021, Genre = "Test Genre 4", Language = "Test Language 4", Publisher = "Test Publisher 4", PublicationYear = 2021 } },
        new BookPosting { Id = 5, Description = "Test Description 5", PostDateTime = DateTime.UtcNow, ExpireDateTime = DateTime.UtcNow.AddDays(30), PictureUrl = "Test Picture Url 5", Book = new Book { Name = "Test Book 5", Author = "Test Author 5", Year = 2021, Genre = "Test Genre 5", Language = "Test Language 5", Publisher = "Test Publisher 5", PublicationYear = 2021 } },
    };

    // mock the logger, and book posting service
    private Mock<ILogger<HomeController>> loggerMock;
    private Mock<IBookPostingService> bookPostingServiceMock;
    private HomeController controller;

    public HomeControllerTests()
    {
        loggerMock = new Mock<ILogger<HomeController>>();
        // mock the book posting Get method
        bookPostingServiceMock = new Mock<IBookPostingService>();
        bookPostingServiceMock.Setup(service => service.Get()).Returns(testPostings.AsQueryable());
        controller = new HomeController(loggerMock.Object, bookPostingServiceMock.Object);
    }

    [Fact]
    public void Index_ReturnsViewWithModel_WhenSearchIsEmpty()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var search = "";
        var bookPostings = Enumerable.Range(1, 10).Select(i => new BookPosting()).AsQueryable();
        bookPostingServiceMock.Setup(service => service.Get()).Returns(bookPostings);

        // Act
        var result = controller.Index(pageNumber, pageSize, search) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result?.ViewName);
        Assert.IsType<PaginationViewModel<BookPosting>>(result?.Model);
        var model = result?.Model as PaginationViewModel<BookPosting>;
        Assert.Equal(0, model?.Items.Count());
    }

    [Fact]
    public void Index_ReturnsViewWithModel_WhenSearchIsNotEmpty()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<HomeController>>();
        var bookPostingServiceMock = new Mock<IBookPostingService>();
        var controller = new HomeController(loggerMock.Object, bookPostingServiceMock.Object);

        var pageNumber = 1;
        var pageSize = 10;
        var search = "someSearch";
        var bookPostings = Enumerable.Range(1, 5).Select(i => new BookPosting()).AsQueryable();
        bookPostingServiceMock.Setup(service => service.Search(search)).Returns(bookPostings);

        // Act
        var result = controller.Index(pageNumber, pageSize, search) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result?.ViewName);
        Assert.IsType<PaginationViewModel<BookPosting>>(result?.Model);
        var model = result?.Model as PaginationViewModel<BookPosting>;
        Assert.Equal(0, model?.Items.Count);
    }

    [Fact]
    public void Privacy_ReturnsPrivacyView()
    {
        // Act
        var result = controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Privacy", result?.ViewName);
    }
}
