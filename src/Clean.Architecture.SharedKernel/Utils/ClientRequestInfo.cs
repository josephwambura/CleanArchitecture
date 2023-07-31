namespace Clean.Architecture.SharedKernel.Utils;

public class ClientRequestInfo
{
  public string? SessionID { get; set; }
  public string? UserHostAddress { get; set; }
  public string? UserHostName { get; set; }
  public string? UserAgent { get; set; }
  public BrowserInfo? Browser { get; set; }
  public LocationInfo? Location { get; set; }

  public ClientRequestInfo()
  {

  }

  //public ClientRequestInfo(HttpContextBase httpContextBase, string locationLatitude, string locationLongitude)
  //{
  //    SessionID = httpContextBase.Session.SessionID;
  //    UserHostAddress = httpContextBase.Request.UserHostAddress;
  //    UserHostName = httpContextBase.Request.UserHostName;
  //    UserAgent = httpContextBase.Request.UserAgent;
  //    Browser = new BrowserInfo(httpContextBase);
  //    Location = new LocationInfo(locationLatitude, locationLongitude);

  //    //// https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/compatibility/ms537503(v=vs.85)?redirectedfrom=MSDN
  //    //if (UserAgent.IndexOf("MSIE 6.0") > -1)
  //    //{
  //    //    // The browser is Microsoft Internet Explorer Version 6.0.
  //    //}
  //}
}

public class BrowserInfo
{
  public int MajorVersion { get; set; }
  public double MinorVersion { get; set; }
  public string? Version { get; set; }
  public string? Name { get; set; }
  public string? Platform { get; set; }
  public string? Type { get; set; }
  public bool IsMobileDevice { get; set; }

  public BrowserInfo()
  {
  }

  //public BrowserInfo(HttpContext httpContextBase)
  //{
  //    MajorVersion = httpContextBase.Request.Browser.MajorVersion;
  //    MinorVersion = httpContextBase.Request.Browser.MinorVersion;
  //    Version = httpContextBase.Request.Browser.Version;
  //    Name = httpContextBase.Request.Browser.Browser;
  //    Platform = httpContextBase.Request.Browser.Platform;
  //    Type = httpContextBase.Request.Browser.Type;
  //    IsMobileDevice = httpContextBase.Request.Browser.IsMobileDevice;
  //}

  public override string ToString() => !string.IsNullOrWhiteSpace(Name) ? $"{Platform}~{Name}~{Version}" : string.Empty;
}

public class LocationInfo
{
  public string? Latitude { get; set; }
  public string? Longitude { get; set; }

  public LocationInfo()
  {

  }

  public LocationInfo(string latitude, string longitude)
  {
    Latitude = latitude;
    Longitude = longitude;
  }

  public override string ToString() => !string.IsNullOrWhiteSpace(Latitude) ? $"{Latitude}, {Longitude}" : string.Empty;
}
