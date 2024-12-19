using Bogus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Services;
using Trackr.Domain.Models;

namespace Trackr.Tests.Application.Services.UserServices;

public class UserControllerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new Mock<IPasswordHasher>();
    private readonly Mock<IJwtManager> _jwtManagerMock = new Mock<IJwtManager>();
    private readonly Mock<IEmailSender> _emailSenderMock = new Mock<IEmailSender>();

    [Fact]
    public async Task Login_ReturnsJwt_WhenUserCredentialsAreCorrect()
    {
        // Arrange
        var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtManagerMock.Object, _emailSenderMock.Object);
        var loginModel = new Faker<UserLoginRequestModel>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .Generate();
        var model = new Faker<User>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => loginModel.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Balance, new Random().Next(3, 100))
            .RuleFor(u => u.CostLimit, new Random().Next(4, 100))
            .RuleFor(u => u.CreatedAt, DateTime.Now)
            .Generate();
        _userRepositoryMock.Setup(repo => repo.GetByEmail(loginModel.Email, It.IsAny<CancellationToken>())).ReturnsAsync(model);
        _passwordHasherMock.Setup(hasher => hasher.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jwtManagerMock.Setup(manager => manager.CreateJwtForUser(model)).ReturnsAsync("SomePassword143**");

        // Act
        var response = await userService.Login(loginModel, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<string>(response);
    }

    [Fact]
    public async Task Login_ThrowsException_WhenUserDoesNotExist()
    {
        // Arrange
        var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtManagerMock.Object, _emailSenderMock.Object);
        var loginModel = new Faker<UserLoginRequestModel>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .Generate();
        var model = new Faker<User>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => loginModel.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Balance, new Random().Next(3, 100))
            .RuleFor(u => u.CostLimit, new Random().Next(4, 100))
            .RuleFor(u => u.CreatedAt, DateTime.Now)
            .Generate();
        _userRepositoryMock.Setup(repo => repo.GetByEmail(loginModel.Email, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
        _passwordHasherMock.Setup(hasher => hasher.Verify(string.Empty, string.Empty)).Returns(false);

        // Act && Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await userService.Login(loginModel, It.IsAny<CancellationToken>()));
    }
}