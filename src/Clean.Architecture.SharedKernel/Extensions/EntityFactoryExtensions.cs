using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class EntityFactoryExtensions
{
  public static void BaseEntityDetails<T>(T entity, ServiceHeader serviceHeader, T? original = null) where T : EntityBase
  {
    if (original == null)
    {
      entity.CreatedBy = string.Format("{0}", serviceHeader.ApplicationUserName).Length == 0 ? "___SYS___" : serviceHeader.ApplicationUserName!;
      entity.CreatedDate = DateTime.UtcNow;
    }
    else
    {
      entity.ModifiedBy = string.Format("{0}", serviceHeader.ApplicationUserName).Length == 0 ? "___SYS___" : serviceHeader.ApplicationUserName;
      entity.ModifiedDate = DateTime.UtcNow;
    }

    entity.Id = entity.IsTransient() ? IdentityGenerator.NewSequentialGuid() : entity.Id;

    entity.RecordStatus = (byte)RecordStatus.Approved;
  }
}
