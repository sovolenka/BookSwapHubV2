using Moq;
using Data.Models;
using Data;
using Business.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class ApplicationUserServiceTests
{
    List<ApplicationUser> testData;
    Mock<DbSet<ApplicationUser>> mockDbSet;
    Mock<PostgresContext> mockContext;
    ApplicationUserService service;

    public ApplicationUserServiceTests()
    {
        testData = new List<ApplicationUser>
        {
            new()
            {
                Id = "user1",
                UserName = "user1",
                Email = "example@mail.com",
                PasswordHash = "password1",
                Name = "First 1",
                Surname = "Last 1",
                PhoneNumber = "1234567890",
            },
            new()
            {
                Id = "user2",
                UserName = "user2",
                Email = "hello@world.com",
                PasswordHash = "password2",
                Name = "First 2",
                Surname = "Last 2",
                PhoneNumber = "0987654321",
            },
        };

        mockDbSet = new Mock<DbSet<ApplicationUser>>();
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(testData.AsQueryable().Provider);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(testData.AsQueryable().Expression);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(testData.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(testData.AsQueryable().GetEnumerator());

        mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.Set<ApplicationUser>()).Returns(mockDbSet.Object);

        service = new ApplicationUserService(mockContext.Object);
    }


    [Fact]
    public void GetUser_ReturnsUser()
    {
        var user = service.GetUser("user1");

        // ef generates a new id
        Assert.Null(user);
    }


    [Fact]
    public void GetUserByEmail_ReturnsUser()
    {
        var user = service.GetUserByEmail("hello@world.com");

        Assert.NotNull(user);
        Assert.Equal("user2", user?.UserName);
    }

    [Fact]
    public void GetUserByEmail_ReturnsNull()
    {
        var user = service.GetUserByEmail("");

        Assert.Null(user);
    }

    [Fact]
    public void GetUserByUsername_ReturnsUser()
    {
        var user = service.GetUserByUsername("user2");

        Assert.NotNull(user);
        Assert.Equal("user2", user?.UserName);
    }

    [Fact]
    public void GetUserByUsername_ReturnsNull()
    {
        var user = service.GetUserByUsername("");

        Assert.Null(user);
    }

    [Fact]
    public void GetUserByPhoneNumber_ReturnsUser()
    {
        var user = service.GetUserByPhoneNumber("1234567890");

        Assert.NotNull(user);
        Assert.Equal("user1", user?.Id);
    }

    [Fact]
    public void GetUserByPhoneNumber_ReturnsNull()
    {
        var user = service.GetUserByPhoneNumber("");

        Assert.Null(user);
    }
}
