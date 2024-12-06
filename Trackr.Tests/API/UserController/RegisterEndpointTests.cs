using Bogus;
using FluentValidation.TestHelper;
using Moq;
using Trackr.API.Infrastructure.Validators;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Users;
using UserControllerAlias = Trackr.API.Controllers.UserController;

namespace Trackr.Tests.API.UserController;

public class RegisterEndpointTests
{
    private readonly UserControllerAlias _userController;
    private readonly Mock<IUserService> _userService = new Mock<IUserService>();
    private readonly UserRegistrationValidator _userValidator = new UserRegistrationValidator();

    public RegisterEndpointTests()
    {
        _userController = new UserControllerAlias(_userService.Object);
    }

    [Fact]
    public async Task Register_ThrowsErrors_WhenIncorrectPayloadIsPassed()
    {
        // Arrange
        var model = new Faker<UserRequestModel>()
            .RuleFor(u => u.Name, _ => string.Empty)
            .RuleFor(u => u.Email, f => f.Random.String())
            .RuleFor(u => u.Password, f => f.Random.String())
            .RuleFor(u => u.ConfirmPassword, f => f.Random.String())
            .Generate();
        // Act
        var result = await _userValidator.TestValidateAsync(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(u => u.Name);
        result.ShouldHaveValidationErrorFor(u => u.Email);
        result.ShouldHaveValidationErrorFor(u => u.Password);
        result.ShouldHaveValidationErrorFor(u => u.ConfirmPassword);
    }

    [Fact]
    public async Task Register_RegistersUser_WhenCorrectPayloadIsPassed()
    {
        var model = new Faker<UserRequestModel>()
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => "Something163#")
            .RuleFor(u => u.ConfirmPassword, (_, u) => u.Password)
            .Generate();
        var responseModel = new Faker<UserResponseModel>()
            .RuleFor(u => u.Email, _ => model.Email)
            .RuleFor(u => u.Name, _ => model.Name)
            .RuleFor(u => u.CostLimit, 0)
            .RuleFor(u => u.Balance, 0)
            .Generate();
        _userService.Setup(service => service.Register(model, It.IsAny<CancellationToken>())).ReturnsAsync(responseModel);
        // Act
        var validatorResult = await _userValidator.TestValidateAsync(model);
        var controllerResult = await _userController.Register(model, It.IsAny<CancellationToken>());

        // Assert
        validatorResult.ShouldNotHaveAnyValidationErrors();
        Assert.IsType<UserResponseModel>(controllerResult);
        Assert.Equal(responseModel.Name, controllerResult.Name);
        Assert.Equal(responseModel.Email, controllerResult.Email);
        Assert.Equal(responseModel.CostLimit, controllerResult.CostLimit);
        Assert.Equal(responseModel.Balance, controllerResult.Balance);
    }
}
