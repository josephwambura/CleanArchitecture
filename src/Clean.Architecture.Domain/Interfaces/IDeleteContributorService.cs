using Ardalis.Result;

namespace Clean.Architecture.Domain.Interfaces;

public interface IDeleteContributorService
{
  public Task<Result> DeleteContributor(Guid contributorId);
}
