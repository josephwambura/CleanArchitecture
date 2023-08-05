using AutoMapper;

using Clean.Architecture.Application.DTO;
using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.SharedKernel.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Clean.Architecture.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected AppDbContext _dbContext;
  protected ApplicationDbContext _authDbContext;
  private IMapper _mapper;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var authOptions = CreateNewAuthContextOptions();
    var mockEventDispatcher = new Mock<IDomainEventDispatcher>();
    var mockAuthEventDispatcher = new Mock<IAuthDomainEventDispatcher>();
    var mockAutoMapper = new MapperConfiguration(mc =>
    {
      //mc.AddProfile(new MappingProfile());
      mc.AddMaps(typeof(DefaultApplicationDTOModule).Assembly);
    }).CreateMapper().ConfigurationProvider;

    _dbContext = new AppDbContext(options, mockEventDispatcher.Object);
    _authDbContext = new ApplicationDbContext(authOptions, mockAuthEventDispatcher.Object);
    _mapper = new Mapper(mockAutoMapper);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("clean.architecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected static DbContextOptions<ApplicationDbContext> CreateNewAuthContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    builder.UseInMemoryDatabase("clean.architecture.auth")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EfRepository<Project> GetRepository()
  {
    return new EfRepository<Project>(_dbContext, _mapper);
  }

  protected AuthEfRepository<ApplicationUser> GetAuthRepository()
  {
    return new AuthEfRepository<ApplicationUser>(_authDbContext, _mapper);
  }
}
