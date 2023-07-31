using Ardalis.SmartEnum;

namespace Clean.Architecture.SharedKernel.Utils;

public class PriorityStatus : SmartEnum<PriorityStatus>
{
  public static readonly PriorityStatus Backlog = new(nameof(Backlog), 0);
  public static readonly PriorityStatus Critical = new(nameof(Critical), 1);

  protected PriorityStatus(string name, int value) : base(name, value) { }
}

public class WellKnownRoles : SmartEnum<WellKnownRoles>
{
  public static readonly WellKnownRoles SuperAdministrator = new(nameof(SuperAdministrator), 0);
  public static readonly WellKnownRoles Administrator = new(nameof(Administrator), 1);
  public static readonly WellKnownRoles Developer = new(nameof(Developer), 2);
  public static readonly WellKnownRoles Support = new(nameof(Support), 3);
  public static readonly WellKnownRoles Secretary = new(nameof(Secretary), 4);
  public static readonly WellKnownRoles Editor = new(nameof(Editor), 5);
  public static readonly WellKnownRoles Viewer = new(nameof(Viewer), 6);
  public static readonly WellKnownRoles Subscriber = new(nameof(Subscriber), 7);
  public static readonly WellKnownRoles Manager = new(nameof(Manager), 8);
  public static readonly WellKnownRoles Owner = new(nameof(Owner), 9);
  public static readonly WellKnownRoles Buyer = new(nameof(Buyer), 10);
  public static readonly WellKnownRoles Business = new(nameof(Business), 11);
  public static readonly WellKnownRoles Seller = new(nameof(Seller), 12);
  public static readonly WellKnownRoles Customer = new(nameof(Customer), 13);
  public static readonly WellKnownRoles Tester = new(nameof(Tester), 14);

  protected WellKnownRoles(string name, int value) : base(name, value) { }
}

public class DLRStatus : SmartEnum<DLRStatus>
{
  public static readonly DLRStatus Pending = new(nameof(Pending), 0);
  public static readonly DLRStatus Queued = new(nameof(Queued), 1);
  public static readonly DLRStatus Failed = new(nameof(Failed), 2);
  public static readonly DLRStatus Delivered = new(nameof(Delivered), 4);
  public static readonly DLRStatus NotApplicable = new("Not Applicable", 8);
  public static readonly DLRStatus UnKnown = new("Awaiting Delivery", 999);

  protected DLRStatus(string name, int value) : base(name, value) { }
}
