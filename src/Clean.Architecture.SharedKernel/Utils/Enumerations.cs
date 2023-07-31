using System.ComponentModel;

namespace Clean.Architecture.SharedKernel.Utils;

public enum RecordStatus
{
  [Description("New")]
  New,
  [Description("Approved")]
  Approved,
  [Description("Edited")]
  Edited,
  [Description("Rejected")]
  Rejected,
}

public enum ProjectStatus
{
  [Description("InProgress")]
  InProgress,
  [Description("Complete")]
  Complete
}

public enum DbConnectionContext
{
  [Description("Audit")]
  Audit,
  [Description("Auth")]
  Auth,
  [Description("LIVE")]
  LIVE
}

public enum RunningEnvironment
{
  [Description("Visual Studio Debug")]
  VisualStudioDebug,
  [Description("Visual Studio Debug SQL Server")]
  VisualStudioDebugSQLServer,
  [Description("Windows Server IIS")]
  WindowsServerIIS,
  [Description("Docker Container")]
  DockerContainer
}
