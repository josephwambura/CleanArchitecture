using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsProjectAndSetsId()
  {
    var testProjectName = "testProject";
    var testProjectStatus = PriorityStatus.Backlog;
    var repository = GetRepository();
    var project = new Project(testProjectName, testProjectStatus)
    {
      CreatedBy = "_SYS_"
    };

    await repository.AddAsync(project);

    var newProject = (await repository.ListAsync()).FirstOrDefault();

    Assert.Equal(testProjectName, newProject?.Name);
    Assert.Equal(testProjectStatus, newProject?.Priority);
    Assert.True(newProject?.Id != Guid.Empty);
  }

  [Fact]
  public async Task AddsApplicationUserAndSetsId()
  {
    var testApplicationUserUserName = "kelsey";
    var testApplicationUserEmail = "kelsey@clean.architecture.com";
    var testApplicationUserPhone = "0726877526";
    var repository = GetAuthRepository();
    var applicationUser = new ApplicationUser()
    {
      UserName = testApplicationUserUserName,
      NormalizedUserName = testApplicationUserUserName.ToUpper(),
      Email = testApplicationUserEmail,
      NormalizedEmail = testApplicationUserEmail.ToUpper(),
      EmailConfirmed = true,
      PhoneNumber = testApplicationUserPhone,
      PhoneNumberConfirmed = true,
      CreatedDate = DateTime.UtcNow,
      CreatedBy = "_SYS_"
    };

    await repository.AddAsync(applicationUser);

    var newProject = (await repository.ListAsync()).FirstOrDefault();

    Assert.Equal(testApplicationUserUserName, newProject?.UserName);
    Assert.Equal(testApplicationUserEmail, newProject?.Email);
    Assert.Equal(testApplicationUserPhone, newProject?.PhoneNumber);
    Assert.True(newProject?.Id != string.Empty);
  }
}
