using FluentAssertions;
using LedgerManager.Application.Contracts.Residents;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Application.Services;
using LedgerManager.Domain.Entities;
using Moq;

namespace LedgerManager.Tests.Services;

public class ResidentServiceTests
{
    private readonly Mock<IResidentRepository> residentRepoMock = new();
    private readonly ResidentService service;

    public ResidentServiceTests()
    {
        service = new ResidentService(residentRepoMock.Object);
    }
    
    private static Resident CreateResident(string first = "Ivan", string last = "Ivanov", string middle = "Ivanovich")
    {
        return new Resident
        {
            Id = Guid.NewGuid(),
            FirstName = first,
            LastName = last,
            MiddleName = middle,
            BirthDate = new DateTime(1990, 1, 1),
            AccountId = Guid.NewGuid()
        };
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnResident_WhenExists()
    {
        var resident = CreateResident();
        residentRepoMock.Setup(r => r.GetByIdAsync(resident.Id)).ReturnsAsync(resident);

        var result = await service.GetByIdAsync(resident.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be("Ivan");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenNotFound()
    {
        residentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Resident?)null);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllResidents()
    {
        var residents = new List<Resident>
        {
            CreateResident("Ivan", "Ivanov", "Ivanovich"),
            CreateResident("Petr", "Petrov", "Petrovich")
        };
        residentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(residents);

        var result = await service.GetAllAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Select(r => r.FirstName).Should().Contain(new[] { "Ivan", "Petr" });
    }

    [Fact]
    public async Task CreateAsync_ShouldAddResident()
    {
        var request = new CreateResidentRequest("Ivan", "Ivanov", "Ivanovich", new DateTime(1990, 1, 1), Guid.NewGuid());

        Resident? savedResident = null;
        residentRepoMock.Setup(r => r.AddAsync(It.IsAny<Resident>()))
            .Callback<Resident>(r => savedResident = r)
            .Returns(Task.CompletedTask);

        var result = await service.CreateAsync(request);

        result.IsSuccess.Should().BeTrue();
        savedResident.Should().NotBeNull();
        savedResident!.FirstName.Should().Be("Ivan");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        var resident = CreateResident("Old", "Name", "Middle");
        residentRepoMock.Setup(r => r.GetByIdAsync(resident.Id)).ReturnsAsync(resident);

        var request = new UpdateResidentRequest("New", "Surname", "Other", new DateTime(2000, 1, 1), Guid.NewGuid());

        var result = await service.UpdateAsync(resident.Id, request);

        result.IsSuccess.Should().BeTrue();
        resident.FirstName.Should().Be("New");
        resident.LastName.Should().Be("Surname");
    }

    [Fact]
    public async Task UpdateAsync_ShouldFail_WhenNotFound()
    {
        residentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Resident?)null);

        var request = new UpdateResidentRequest("New", "Surname", "Other", new DateTime(2000, 1, 1), Guid.NewGuid());

        var result = await service.UpdateAsync(Guid.NewGuid(), request);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExists()
    {
        var resident = CreateResident();
        residentRepoMock.Setup(r => r.GetByIdAsync(resident.Id)).ReturnsAsync(resident);

        var result = await service.DeleteAsync(resident.Id);

        result.IsSuccess.Should().BeTrue();
        residentRepoMock.Verify(r => r.DeleteAsync(resident), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenNotFound()
    {
        residentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Resident?)null);

        var result = await service.DeleteAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
    }
}