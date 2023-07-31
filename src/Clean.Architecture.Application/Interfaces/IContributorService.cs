using Ardalis.Result;

namespace Clean.Architecture.Application.Interfaces;

public interface IContributorService
{
  public Task<Result> DeleteContributor(Guid contributorId);
}
