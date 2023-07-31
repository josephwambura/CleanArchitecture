using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Clean.Architecture.IntegrationTests.Data;

public class EfRepositoryUpdate : BaseEfRepoTestFixture
{
  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // add a project
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var project = new Project(initialName, PriorityStatus.Backlog)
    {
      CreatedBy = "_SYS_"
    };

    await repository.AddAsync(project);

    // detach the item so we get a different instance
    _dbContext.Entry(project).State = EntityState.Detached;

    // fetch the item and update its title
    var newProject = (await repository.ListAsync())
        .FirstOrDefault(project => project.Name == initialName);
    if (newProject == null)
    {
      Assert.NotNull(newProject);
      return;
    }
    Assert.NotSame(project, newProject);
    var newName = Guid.NewGuid().ToString();
    newProject.UpdateName(newName);

    // Update the item
    await repository.UpdateAsync(newProject);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(project => project.Name == newName);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(project.Name, updatedItem?.Name);
    Assert.Equal(project.Priority, updatedItem?.Priority);
    Assert.Equal(newProject.Id, updatedItem?.Id);
  }

  [Fact]
  public async Task UpdatesItemAfterAddingItSqlQuery()
  {
    // add a project
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var project = new Project(initialName, PriorityStatus.Backlog)
    {
      CreatedBy = "_SYS_"
    };

    await repository.AddAsync(project);

    // detach the item so we get a different instance
    _dbContext.Entry(project).State = EntityState.Detached;

    // SQL version of the above LINQ code.
    FormattableString query = $"SELECT * FROM {Utility.DbTableName<Project>()}";

    var data = _dbContext.Database.IsInMemory() ? await repository.ListAsync() : await repository.DatabaseSqlQueryAsync<Project>(query);

    // fetch the item and update its title
    var newProject = data.FirstOrDefault(project => project.Name == initialName);
    if (newProject == null)
    {
      Assert.NotNull(newProject);
      return;
    }
    Assert.NotSame(project, newProject);
    var newName = Guid.NewGuid().ToString();
    newProject.UpdateName(newName);

    // Update the item
    await repository.UpdateAsync(newProject);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(project => project.Name == newName);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(project.Name, updatedItem?.Name);
    Assert.Equal(project.Priority, updatedItem?.Priority);
    Assert.Equal(newProject.Id, updatedItem?.Id);
  }

  [Fact]
  public async Task UpdatesApplicationUserAfterAddingIt()
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

    // detach the item so we get a different instance
    _authDbContext.Entry(applicationUser).State = EntityState.Detached;

    // fetch the item and update its title
    var newApplicationUser = (await repository.ListAsync())
        .FirstOrDefault(project => project.UserName == initialName);
    if (newApplicationUser == null)
    {
      Assert.NotNull(newApplicationUser);
      return;
    }
    Assert.NotSame(applicationUser, newApplicationUser);
    var newName = Guid.NewGuid().ToString();
    newApplicationUser.UserName = newName;
    newApplicationUser.NormalizedUserName = newName.ToUpper();

    // Update the item
    await repository.UpdateAsync(newApplicationUser);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(project => project.UserName == newName);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(applicationUser.UserName, updatedItem?.UserName);
    Assert.Equal(applicationUser.Email, updatedItem?.Email);
    Assert.Equal(newApplicationUser.Id, updatedItem?.Id);
  }
}
