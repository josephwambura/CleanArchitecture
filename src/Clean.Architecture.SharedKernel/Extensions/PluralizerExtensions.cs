using Clean.Architecture.SharedKernel.Pluralize;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class PluralizerExtensions
{
  public static string Pluralize(this string word) => new Pluralizer().Pluralize(word);

  public static string Singularize(this string word) => new Pluralizer().Singularize(word);
}
