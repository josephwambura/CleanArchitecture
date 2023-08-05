namespace Clean.Architecture.Domain.ContributorAggregate;

public static class ContributorFactory
{
  public static Contributor CreateContributor(string name)
  {
    var entity = new Contributor(name);

    return entity;
  }
}
