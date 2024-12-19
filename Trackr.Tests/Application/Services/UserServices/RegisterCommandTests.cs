using Bogus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Users;
using Trackr.Application.Services;
using Trackr.Domain.Models;

namespace Trackr.Tests.Application.Services.UserServices;

public class RegisterCommandTests
{

    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new Mock<IPasswordHasher>();
    private readonly Mock<IJwtManager> _jwtManagerMock = new Mock<IJwtManager>();
    private readonly Mock<IEmailSender> _emailSenderMock = new Mock<IEmailSender>();


    [Fact]
    public async Task Register_ReturnsCorrectType_WhenPassedCorrectPayload()
    {
        // Arrange
        var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtManagerMock.Object, _emailSenderMock.Object);
        var model = new Faker<UserRequestModel>()
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.ConfirmPassword, (f, u) => u.Password)
            .Generate();
        _userRepositoryMock.Setup(repo => repo.Register(model, It.IsAny<string>())).ReturnsAsync(model);

        // Act
        var response = await userService.Register(model, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsType<UserResponseModel>(response);
    }

    [Fact]
    public async Task Register_ThrowsAnError_WhenUserExists()
    {
        // Arrange
        var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _jwtManagerMock.Object, _emailSenderMock.Object);
        var registerModel = new Faker<UserRequestModel>()
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.ConfirmPassword, (f, u) => u.Password)
            .Generate();
        var model = new Faker<User>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => registerModel.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Balance, new Random().Next(3, 100))
            .RuleFor(u => u.CostLimit, new Random().Next(4, 100))
            .RuleFor(u => u.CreatedAt, DateTime.Now)
            .Generate();
        _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);

        // Act && Assert
        await Assert.ThrowsAsync<ConflictException>(async () => await userService.Register(registerModel, It.IsAny<CancellationToken>()));
    }
}
