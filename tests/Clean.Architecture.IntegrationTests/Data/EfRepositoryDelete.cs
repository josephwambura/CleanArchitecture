using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // add a project
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var project = new Project(initialName, PriorityStatus.Backlog)
    {
      CreatedBy = "_SYS_"
    };
    await repository.AddAsync(project);

    // delete the item
    await repository.DeleteAsync(project);

    // verify it's no longer there
    Assert.DoesNotContain(await repository.ListAsync(),
        project => project.Name == initialName);
  }

  [Fact]
  public async Task DeletesApplicationUserAfterAddingIt()
  {
    // add a project
    var repository = GetAuthRepository();
    var initialName = Guid.NewGuid().ToString();
    var testApplicationUserEmail = "kelsey@clean.architecture.com";
    var testApplicationUserPhone = "0726877526";
    var applicationUser = new ApplicationUser()
    {
      UserName = initialName,
      NormalizedUserName = initialName.ToUpper(),
      Email = testApplicationUserEmail,
      NormalizedEmail = testApplicationUserEmail.ToUpper(),
      EmailConfirmed = true,
      PhoneNumber = testApplicationUserPhone,
      PhoneNumberConfirmed = true,
      CreatedDate = DateTime.UtcNow,
      CreatedBy = "_SYS_"
    };
    await repository.AddAsync(applicationUser);

    // delete the item
    await repository.DeleteAsync(applicationUser);

    // verify it's no longer there
    Assert.DoesNotContain(await repository.ListAsync(),
        project => project.UserName == initialName);
  }
}
