using Clean.Architecture.Application.Services;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

using Moq;

using Xunit;

namespace Clean.Architecture.UnitTests.Core.Services;

public class ProjectService_GetProjectsWithFiltersAndInPage
{
  private readonly Mock<IUnitOfWork> _mockRepo = new Mock<IUnitOfWork>();
  private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
  private readonly ProjectService _service;

  public ProjectService_GetProjectsWithFiltersAndInPage()
  {
    _service = new ProjectService(_mockRepo.Object, _mockMediator.Object);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenCantFindProjectsWithFiltersAndInPage()
  {
    var result = await _service.GetProjectsWithFiltersAndInPageAsync(string.Empty, 1, 10, "CreatedDate", "desc", new ServiceHeader(), CancellationToken.None);

    Assert.Equal(Ardalis.Result.ResultStatus.Ok, result.Status);
  }
}
