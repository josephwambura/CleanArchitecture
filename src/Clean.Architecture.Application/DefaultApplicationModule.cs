using Autofac;

using Clean.Architecture.Application.Core.Interfaces.AdministrationModule;
using Clean.Architecture.Application.Core.Interfaces.MessagingModule;
using Clean.Architecture.Application.Core.Services.AdministrationModule;
using Clean.Architecture.Application.Core.Services.MessagingModule;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Application.Interfaces.AdministrationModule;
using Clean.Architecture.Application.Interfaces.InventoryModule;
using Clean.Architecture.Application.Interfaces.UserManagementModule;
using Clean.Architecture.Application.Services;
using Clean.Architecture.Application.Services.AdministrationModule;
using Clean.Architecture.Application.Services.InventoryModule;
using Clean.Architecture.Application.Services.UserManagementModule;

namespace Clean.Architecture.Application;
public class DefaultApplicationModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemService>()
        .As<IToDoItemService>().InstancePerLifetimeScope();

    builder.RegisterType<ProjectService>()
        .As<IProjectService>().InstancePerLifetimeScope();

    builder.RegisterType<ContributorService>()
        .As<IContributorService>().InstancePerLifetimeScope();
    
    builder.RegisterType<WeatherForecastService>()
        .As<IWeatherForecastService>().InstancePerLifetimeScope();

    #region AdministrationModule

    builder.RegisterType<CompanyAppService>()
        .As<ICompanyAppService>().InstancePerLifetimeScope();

    builder.RegisterType<EnumerationAppService>()
        .As<IEnumerationAppService>().InstancePerLifetimeScope();

    builder.RegisterType<StaticSettingAppService>()
        .As<IStaticSettingAppService>().InstancePerLifetimeScope();

    #endregion

    #region InventoryModule

    builder.RegisterType<ProductAppService>()
        .As<IProductAppService>().InstancePerLifetimeScope();

    #endregion

    #region MessagingModule

    builder.RegisterType<EmailAlertAppService>()
        .As<IEmailAlertAppService>().InstancePerLifetimeScope();

    builder.RegisterType<TextAlertAppService>()
        .As<ITextAlertAppService>().InstancePerLifetimeScope();

    #endregion

    #region UserManagementModule

    builder.RegisterType<ApplicationUserService>()
        .As<IApplicationUserService>().InstancePerLifetimeScope();

    #endregion

    builder.RegisterType<SqlCommandAppService>()
        .As<ISqlCommandAppService>().InstancePerLifetimeScope();

    builder.RegisterType<BrokerServiceAppService>()
        .As<IBrokerServiceAppService>().InstancePerLifetimeScope();
  }
}
