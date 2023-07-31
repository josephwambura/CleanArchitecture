using System.Reflection;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Clean.Architecture.Application.DTO;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.Infrastructure.Services;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Clean.Architecture.Infrastructure;

public class DefaultInfrastructureModule : Module
{
  private readonly bool _isDevelopment = false;
  private readonly List<Assembly> _assemblies = new List<Assembly>();

  public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
  {
    _isDevelopment = isDevelopment;
    var applicationAssembly =
      Assembly.GetAssembly(typeof(Application.Interfaces.IHttpRequestAppService));
    var applicationDTOAssembly =
      Assembly.GetAssembly(typeof(Application.DTO.ProjectAggregate.ProjectDTO)); // TODO: Replace "ProjectDTO" with any type from your Application DTO project
    var coreAssembly =
      Assembly.GetAssembly(typeof(Project)); // TODO: Replace "Project" with any type from your Core project
    var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));
    if (applicationAssembly != null)
    {
      _assemblies.Add(applicationAssembly);
    }
    if (applicationDTOAssembly != null)
    {
      _assemblies.Add(applicationDTOAssembly);
    }
    if (coreAssembly != null)
    {
      _assemblies.Add(coreAssembly);
    }

    if (infrastructureAssembly != null)
    {
      _assemblies.Add(infrastructureAssembly);
    }

    if (callingAssembly != null)
    {
      _assemblies.Add(callingAssembly);
    }
  }

  protected override void Load(ContainerBuilder builder)
  {
    if (_isDevelopment)
    {
      RegisterDevelopmentOnlyDependencies(builder);
    }
    else
    {
      RegisterProductionOnlyDependencies(builder);
    }

    RegisterCommonDependencies(builder);
  }

  private void RegisterCommonDependencies(ContainerBuilder builder)
  {
    builder.RegisterAutoMapper(typeof(DefaultApplicationDTOModule).Assembly);

    builder.RegisterGeneric(typeof(AuthEfRepository<>))
      .As(typeof(IRepository<>))
      .As(typeof(IReadRepository<>))
      .InstancePerLifetimeScope();
    
    builder.RegisterGeneric(typeof(EfRepository<>))
      .As(typeof(IRepository<>))
      .As(typeof(IReadRepository<>))
      .InstancePerLifetimeScope();
    
    builder.RegisterType<UnitOfWork>()
      .As<IUnitOfWork>()
      .InstancePerLifetimeScope();
    
    builder.RegisterType<ChannelService>()
      .As<IChannelService>()
      .InstancePerLifetimeScope();

    builder
      .RegisterType<Mediator>()
      .As<IMediator>()
      .InstancePerLifetimeScope();

    builder
      .RegisterType<DomainEventDispatcher>()
      .As<IDomainEventDispatcher>()
      .InstancePerLifetimeScope();
    
    builder
      .RegisterType<AuthDomainEventDispatcher>()
      .As<IAuthDomainEventDispatcher>()
      .InstancePerLifetimeScope();

    var mediatrOpenTypes = new[]
    {
      typeof(IRequestHandler<,>), 
      typeof(IRequestExceptionHandler<,,>), 
      typeof(IRequestExceptionAction<,>),
      typeof(INotificationHandler<>),
    };

    foreach (var mediatrOpenType in mediatrOpenTypes)
    {
      builder
        .RegisterAssemblyTypes(_assemblies.ToArray())
        .AsClosedTypesOf(mediatrOpenType)
        .AsImplementedInterfaces();
    }
  }

  private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
  {
    // NOTE: Add any development only services here
    builder.RegisterType<FakeEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();
  }

  private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
  {
    // NOTE: Add any production only services here
    builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
      .InstancePerLifetimeScope();
  }
}
