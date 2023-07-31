namespace Clean.Architecture.Web.Pages;

public abstract class BasePageModelBase : PageModelBase
{
  public List<SelectListItem> RecordStatusesSelectListItem(string? selectedValue)
  {
    return Enum.GetValues(typeof(RecordStatus)).Cast<RecordStatus>().Select(t => new SelectListItem
    {
      Text = t.GetDescription(),
      Value = $"{(int)t}",
      Selected = $"{(int)t}" == selectedValue
    }).ToList();
  }

  public List<SelectListItem> RunningEnvironmentsSelectListItem(string? selectedValue)
  {
    return Enum.GetValues(typeof(RunningEnvironment)).Cast<RunningEnvironment>().Select(t => new SelectListItem
    {
      Text = t.GetDescription(),
      Value = $"{(int)t}",
      Selected = $"{(int)t}" == selectedValue
    }).ToList();
  }
}
