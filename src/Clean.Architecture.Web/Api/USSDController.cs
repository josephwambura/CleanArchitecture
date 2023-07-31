using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;

using Clean.Architecture.Application.DTO;

using LazyCache;

using Stateless;

namespace Clean.Architecture.Web.Api;

public class USSDController : BaseApiController
{
  private readonly ILogger<USSDController> _logger;
  private readonly IAppCache _appCache;
  private static IChannelService? _channelService;

  public USSDController(ILogger<USSDController> logger,
      IAppCache appCache, IChannelService channelService)
  {
    _logger = logger;
    _appCache = appCache;
    _channelService = channelService;
  }

  [HttpGet]
  public ActionResult<string> Get()
  {
    var assembly = typeof(Program).Assembly;

    var creationDate = System.IO.File.GetCreationTime(assembly.Location);
    var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

    var company = FileVersionInfo.GetVersionInfo(assembly.Location).CompanyName;
    var product = FileVersionInfo.GetVersionInfo(assembly.Location).ProductName;
    var copyright = FileVersionInfo.GetVersionInfo(assembly.Location).LegalCopyright;
    var trademarks = FileVersionInfo.GetVersionInfo(assembly.Location).LegalTrademarks;
    var description = FileVersionInfo.GetVersionInfo(assembly.Location).FileDescription;

    return Ok($"Company: {company}\nProduct: {product}\nCopyright: {copyright}\nTrademark: {trademarks}\nVersion: {version}\nDescription: {description}\nLast Updated: {creationDate}");
  }

  // POST: api/USSD
  [HttpPost]
  public ActionResult<string> Post([FromBody] USSDRequestDTO uSSDRequestDTO)
  {
    try
    {
      string? result = string.Empty;

      string[]? stringValues = null;

      if (uSSDRequestDTO.text != null)
      {
        if (uSSDRequestDTO.text.Contains('*'))
        {
          stringValues = uSSDRequestDTO.text.Split('*');

          result = stringValues[^1];
        }
        else
        {
          stringValues = new[] { uSSDRequestDTO.text };

          result = uSSDRequestDTO.text;
        }
      }

      var mobileNumber = string.Empty;

      mobileNumber = uSSDRequestDTO.phoneNumber.StartsWith("+") ? uSSDRequestDTO.phoneNumber.TrimStart('+') : uSSDRequestDTO.phoneNumber;

      var ussdResult = _appCache.GetOrAdd(uSSDRequestDTO.sessionId, () => new USSD(mobileNumber), DateTimeOffset.Now.AddMinutes(1));

      //var ussdResult2 = _memoryCache.GetOrCreate(uSSDRequestDTO.sessionId, () => new USSD(mobileNumber));

      //var ussdResult = await _distributedCache.GetAsync<Func<USSD>>(uSSDRequestDTO.sessionId);

      //if (ussdResult == null)
      //{
      //    await _distributedCache.SetAsync<Func<USSD>>(uSSDRequestDTO.sessionId, () => new USSD(mobileNumber), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1) });
      //}

      ussdResult?.Transition(result, stringValues!);

      return Ok($"{ussdResult}");
    }
    catch (Exception ex)
    {
      _logger.LogError("USSD error > {0}", ex.Message);

      var response = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StringContent("END Sorry, a technical error has occurred! Please try again later."),
      };

      response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

      return Ok("END Sorry, a technical error has occurred! Please try again later.");
    }
  }

  public class USSD
  {
    public string? MSISDN { get; set; }

    public string? Input { get; set; }

    //public ProjectDTO? Project { get; set; }

    public string? ProjectName { get; set; }

    public Guid? ProjectId { get; set; }

    public string? OtherAccountNumber { get; set; }

    public string? PrimaryAccountNumber { get; set; }

    public string? PIN { get; set; }

    public string? NewPIN { get; set; }

    public int PINAttemptCount { get; set; } = -1;

    public string? Amount { get; set; }

    public string? IDNumber { get; set; }

    public string? AcceptMessage { get; set; }

    public string? ProjectMobileNumber { get; set; }

    public byte ChannelType { get; set; }

    public enum Triggers
    {
      [Description("Menu")] Menu,
      [Description("Top Up")] TopUp,
      [Description("Withdrawal")] Withdrawal,
      [Description("Change Pin")] ChangePIN,
      [Description("Forgot Pin")] ForgotPIN,
      [Description("Balance")] Balance,
      [Description("Pin")] Pin,
      [Description("List Projects")] ListProjects,
      [Description("Add Project")] AddProject,
      [Description("Remove Project")] RemoveProject,
      [Description("Exit")] Exit = 99,
      [Description("Decline")] Decline = 97,
      [Description("Accept")] Accept = 98
    }

    public enum States
    {
      [Description("Menu")] Menu,
      [Description("Top Up")] TopUp,
      [Description("Withdrawal")] Withdrawal,
      [Description("Change Pin")] ChangePIN,
      [Description("Forgot Pin")] ForgotPIN,
      [Description("Balance")] Balance,
      [Description("Pin")] Pin,
      [Description("List Projects")] ListProjects,
      [Description("Add Project")] AddProject,
      [Description("Remove Project")] RemoveProject,
      [Description("Exit")] Exit,
      [Description("Decline")] Decline,
      [Description("Accept")] Accept
    }

    public States CurrentState { get; set; }

    public StateMachine<States, Triggers> Machine { get; set; }

    public StateMachine<States, Triggers>.TriggerWithParameters<int> parameterizedAcceptTrigger;

    public USSD()
    {
      Machine = new StateMachine<States, Triggers>(() => CurrentState, s => CurrentState = s);

      parameterizedAcceptTrigger = Machine.SetTriggerParameters<int>(Triggers.Accept);

      Machine.Configure(States.Menu)
          .Permit(Triggers.TopUp, States.TopUp)
          .PermitReentry(Triggers.Menu)
          .Permit(Triggers.Withdrawal, States.Withdrawal)
          .Permit(Triggers.ForgotPIN, States.ForgotPIN)
          .Permit(Triggers.ChangePIN, States.ChangePIN)
          .Permit(Triggers.ListProjects, States.ListProjects)
          .Permit(Triggers.Balance, States.Balance)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Decline, States.Decline)
          .Permit(Triggers.Exit, States.Exit);

      Machine.Configure(States.Pin)
          .Permit(Triggers.Menu, States.Menu)
          .PermitReentry(Triggers.Menu)
          .Permit(Triggers.ChangePIN, States.ChangePIN)
          .Permit(Triggers.ListProjects, States.ListProjects)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Decline, States.Decline)
          .Permit(Triggers.Exit, States.Exit)
          .PermitReentry(Triggers.Exit);

      Machine.Configure(States.TopUp)
          .SubstateOf(States.Menu)
          .PermitReentry(Triggers.Menu)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline);

      Machine.Configure(States.Withdrawal)
          .SubstateOf(States.Menu)
          .PermitReentry(Triggers.Menu)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline);

      Machine.Configure(States.Balance)
          .SubstateOf(States.Menu)
          .PermitReentry(Triggers.Balance)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline);

      Machine.Configure(States.ListProjects)
          .SubstateOf(States.Menu)
          .PermitReentry(Triggers.ListProjects)
          .Permit(Triggers.AddProject, States.AddProject)
          .Permit(Triggers.RemoveProject, States.RemoveProject)
          .Permit(Triggers.Accept, States.Accept)
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline);

      Machine.Configure(States.ChangePIN)
          .SubstateOf(States.Menu)
          .Permit(Triggers.Menu, States.Menu)
          .PermitIf(Triggers.Accept, States.Accept, () => !string.IsNullOrWhiteSpace(PIN) && !string.IsNullOrWhiteSpace(NewPIN))
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline)
          .PermitReentry(Triggers.Exit);

      Machine.Configure(States.ForgotPIN)
          .Permit(Triggers.Menu, States.Menu)
          .SubstateOf(States.Menu)
          .PermitReentry(Triggers.ChangePIN)
          .PermitIf(Triggers.Accept, States.Accept, () => !string.IsNullOrWhiteSpace(IDNumber) && !string.IsNullOrWhiteSpace(MSISDN))
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline)
          .PermitReentry(Triggers.Exit);

      Machine.Configure(States.Decline)
          .Permit(Triggers.Menu, States.Menu)
          .Permit(Triggers.Exit, States.Exit);

      Machine.Configure(States.Accept)
          .Permit(Triggers.Menu, States.Menu)
          .Permit(Triggers.Exit, States.Exit)
          .Permit(Triggers.Decline, States.Decline)
          .OnEntryFrom(parameterizedAcceptTrigger, (currentState) =>
          {
            switch ((States)currentState)
            {
              case States.ChangePIN:

                #region ChangePIN

                //Project.Pin = PasswordHash.CreateHash(NewPIN);

                //Project.LastPinChange = DateTime.UtcNow;

                //var result = _channelService.UpdateProjectAsync(Project).GetAwaiter().GetResult();

                //AcceptMessage = result
                //    ? "Your PIN has been changed!"
                //    : "Sorry but we couldn't update your PIN at the moment. Please try again later.";

                #endregion

                break;
              case States.TopUp:
                {

                  #region Top Up

                  //var debitProjectAccount = _channelService.FindProjectAccountByProjectMobileNumberAsync($"+{MSISDN}").GetAwaiter().GetResult();

                  //var otherProjectAccounts = _channelService.FindProjectAccountsByPANAsync(PrimaryAccountNumber).GetAwaiter().GetResult();

                  //var creditProjectAccount = otherProjectAccounts.FirstOrDefault();

                  //var productTransactionType = _channelService.FindProductTransactionTypeByTransactionTypeChannelAsync(ChannelType).GetAwaiter().GetResult();

                  //var credit = new WalletTopUpDTO()
                  //{
                  //    EventType = (byte)ProjectAccountEventType.Credit,
                  //    ProductTransactionTypeId = productTransactionType.Id,
                  //    ProjectAccountId = creditProjectAccount.Id,
                  //    OtherProjectAccountId = debitProjectAccount.Id, //Debit account
                  //    WalletStatus = (byte)WalletStatus.Pending,
                  //    RecordStatus = (byte)RecordStatus.Approved,
                  //    Amount = decimal.Parse(Amount),
                  //    Reason = "USSD Top up",
                  //    Receipt = "",
                  //    CreatedBy = "USSD"
                  //};

                  //var topUp = _channelService.CreateWalletTopUpAsync(credit).GetAwaiter().GetResult();

                  //if (string.IsNullOrEmpty(topUp.Item2))
                  //{
                  //    AcceptMessage = "Thank you. Top up request has been accepted.";
                  //}
                  //else
                  //{
                  //    AcceptMessage = topUp.Item2;
                  //}

                  AcceptMessage = "Thank you. Top up request has been accepted.";


                  #endregion

                  break;
                }
              case States.Withdrawal:
                {

                  #region Withdrawal

                  //var customer = _channelService.FindProjectByIDNumberAndMobileNumberAsync(IDNumber, ProjectMobileNumber).GetAwaiter().GetResult();

                  //if (customer != null)
                  //{
                  //    AcceptMessage = "The provided details have been verfied.";
                  //}
                  //else
                  //{
                  //    AcceptMessage = "Sorry but we could not verify the details at the moment. Please try again later.";
                  //}

                  #endregion
                  break;
                }
              case States.ForgotPIN:
                {
                  #region ForgotPIN

                  if (!string.IsNullOrWhiteSpace(IDNumber))
                  {
                    //if (Project.IDNumber.Equals(IDNumber))
                    //{
                    //    Project.Pin = string.Empty;

                    //    Project.LastPinChange = DateTime.UtcNow;

                    //    var updateResult = _channelService.ProcessRetailProjectAsync(Project).GetAwaiter().GetResult();

                    //    AcceptMessage = updateResult
                    //                ? "Your PIN has been reset successfully. You will received a text message with your new PIN."
                    //                : "Sorry but we couldn't update your PIN at the moment. Please try again later.";
                    //}

                    //else
                    //{
                    //    AcceptMessage = "Sorry but we couldn't update your PIN at the moment. Please try again later.";
                    //}
                  }

                  #endregion

                  break;
                }
              case States.Balance:
                {
                  //var customerAccounts = _channelService.FindProjectAccountsByPANAsync(PrimaryAccountNumber).GetAwaiter().GetResult();

                  //var customerAccount = customerAccounts.FirstOrDefault();

                  //var expirationTime = Convert.ToInt32(ConfigurationManager.AppSettings["SessionExpirationTime"]);

                  //var accountBalance = _channelService.GetCachedProjectAccountBalanceAsync(customerAccount.Id, expirationTime).GetAwaiter().GetResult();

                  //var sb = new StringBuilder();

                  //if (accountBalance != null)
                  //{
                  //    sb.AppendLine($"The account balance is {accountBalance.Balance}");

                  //    AcceptMessage = sb.ToString();
                  //}
                  //else
                  //{
                  //    AcceptMessage = "Something went wrong. Please try again later.";
                  //}
                  break;
                }
              case States.ListProjects:
                {
                  if (_channelService != null)
                  {
                    var projects = _channelService.FindProjectsAsync(new ServiceHeader(), CancellationToken.None).GetAwaiter().GetResult();

                    var sb = new StringBuilder();

                    if (projects != null && projects.IsSuccess && projects.Value.Any())
                    {
                      sb.AppendLine($"{projects.Value.Count} projects");

                      AcceptMessage = sb.ToString();
                    }
                    else
                    {
                      AcceptMessage = "Something went wrong. Please try again later.";
                    }
                  }
                  break;
                }
              case States.AddProject:
                {
                  if (_channelService != null)
                  {
                    var projects = _channelService.FindProjectsAsync(new ServiceHeader(), CancellationToken.None).GetAwaiter().GetResult();

                    var sb = new StringBuilder();

                    if (projects != null && projects.IsSuccess && projects.Value.Any())
                    {
                      sb.AppendLine($"{projects.Value.Count} projects");

                      AcceptMessage = sb.ToString();
                    }
                    else
                    {
                      AcceptMessage = "Something went wrong. Please try again later.";
                    }
                  }
                  break;
                }
              case States.RemoveProject:
                {
                  if (_channelService != null)
                  {
                    var projects = _channelService.FindProjectsAsync(new ServiceHeader(), CancellationToken.None).GetAwaiter().GetResult();

                    var sb = new StringBuilder();

                    if (projects != null && projects.IsSuccess && projects.Value.Any())
                    {
                      sb.AppendLine($"{projects.Value.Count} projects");

                      AcceptMessage = sb.ToString();
                    }
                    else
                    {
                      AcceptMessage = "Something went wrong. Please try again later.";
                    }
                  }
                  break;
                }
              case States.Exit:
                break;
              case States.Decline:
                break;
              case States.Accept:
                break;
            }
          });
    }

    public USSD(string msisdn) : this()
    {
      Seed(msisdn);
    }

    private void Seed(string msisdn)
    {
      if (!string.IsNullOrWhiteSpace(msisdn))
      {
        if (_channelService != null)
        {
          //var project = _channelService.GetProjectByPhoneNumberAsync($"+{msisdn}").GetAwaiter().GetResult()?.Value;
          //MSISDN = msisdn;

          //if (project != null)
          //{
          //  Project = _channelService.GetProjectByPhoneNumberAsync($"+{msisdn}").GetAwaiter().GetResult()?.Value;
          //}
        }
      }
    }

    public void Transition(string transitInput, string[] originalValue)
    {
      Input = transitInput;

      States currentState = CurrentState;

      Triggers trigger;

      if (Enum.TryParse(transitInput, out trigger))
      {
        if (Enum.IsDefined(typeof(Triggers), trigger))
        {
          if (currentState == States.Menu || currentState == States.Accept || currentState == States.Decline)
          {
            if (Machine.CanFire(trigger))
            {
              switch (trigger)
              {

                case Triggers.Accept:
                  Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
                  break;
                case Triggers.Exit:
                  PrimaryAccountNumber = string.Empty;
                  PIN = string.Empty;
                  NewPIN = string.Empty;
                  Machine.Fire(trigger);
                  break;
                default:

                  CaptureInput(transitInput, originalValue);

                  Machine.Fire(trigger);

                  break;
              }
            }
            else
            {
              CaptureInput(transitInput, originalValue);
            }
          }
          else if (trigger == Triggers.Accept)
          {
            #region Accept

            if (currentState == States.ChangePIN)
            {
              if (!string.IsNullOrWhiteSpace(NewPIN) && !string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }

            else if (currentState == States.TopUp)
            {
              if (!string.IsNullOrWhiteSpace(PrimaryAccountNumber) && !string.IsNullOrWhiteSpace(Amount) && !string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.Withdrawal)
            {
              if (!string.IsNullOrWhiteSpace(ProjectMobileNumber) && !string.IsNullOrWhiteSpace(IDNumber) && !string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.ListProjects)
            {
              if (!string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.AddProject)
            {
              if (!string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.RemoveProject)
            {
              if (!string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.ForgotPIN)
            {
              if (!string.IsNullOrWhiteSpace(IDNumber))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }
            else if (currentState == States.Balance)
            {
              if (!string.IsNullOrWhiteSpace(PrimaryAccountNumber) && !string.IsNullOrWhiteSpace(PIN))
              {
                Machine.Fire(parameterizedAcceptTrigger, (int)CurrentState);
              }
              else
              {
                CaptureInput(transitInput, originalValue);
              }
            }

            #endregion
          }
          else if (trigger == Triggers.Exit)
          {
            CaptureInput(transitInput, originalValue);
          }
          else if (trigger == Triggers.Decline)
          {
            Machine.Fire(trigger);
          }
          else
          {
            CaptureInput(transitInput, originalValue);
          }
        }
        else
        {
          CaptureInput(transitInput, originalValue);
        }
      }
      else CaptureInput(transitInput, originalValue);
    }

    private void CaptureInput(string transitValue, string[] originalValue)
    {
      //this method is used to seed data into the memory during transitions
      switch (CurrentState)
      {
        case States.Pin:

          #region PIN

          if (!string.IsNullOrWhiteSpace(Input))
          {

            //if (originalValue[originalValue.Length - 1].Equals(ConfigurationManager.AppSettings["USSDServiceExtensionCode"]))
            //{
            //    Input = string.Empty;
            //}

            //if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(Input, Project.Pin))
            //{
            //    PIN = originalValue[originalValue.Length - 1];
            //    if (!string.IsNullOrWhiteSpace(Project.Pin) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
            //    {
            //        CurrentState = States.Menu;
            //    }
            //    else
            //    {
            //        PINAttemptCount++;
            //    }
            //}
            //else
            //{
            //    PINAttemptCount++;
            //}
          }


          #endregion

          break;
        case States.Menu:
          break;
        case States.TopUp:

          #region Top Up

          if (string.IsNullOrWhiteSpace(PrimaryAccountNumber))
          {
            PrimaryAccountNumber = originalValue[originalValue.Length - 1];

          }
          else if (string.IsNullOrWhiteSpace(Amount))
          {
            Amount = originalValue[originalValue.Length - 1];

          }
          //else if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //}

          #endregion

          break;
        case States.ListProjects:
          break;
        case States.AddProject:

          #region Top Up

          if (string.IsNullOrWhiteSpace(ProjectName))
          {
            ProjectName = originalValue[originalValue.Length - 1];

          }
          else if (string.IsNullOrWhiteSpace(Amount))
          {
            Amount = originalValue[originalValue.Length - 1];

          }
          //else if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //}

          #endregion

          break;
        case States.RemoveProject:

          #region Top Up

          if (ProjectId == null)
          {
            ProjectId = Guid.Parse(originalValue[originalValue.Length - 1]);

          }
          //else if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //}

          #endregion

          break;
        case States.Withdrawal:

          #region Withdrawal

          if (string.IsNullOrWhiteSpace(IDNumber))
          {
            IDNumber = originalValue[originalValue.Length - 1];

          }
          else if (string.IsNullOrEmpty(ProjectMobileNumber))
          {
            ProjectMobileNumber = originalValue[originalValue.Length - 1];
          }
          //else if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //}

          #endregion

          break;
        case States.ChangePIN:

          #region Change Pin

          //if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //    PINAttemptCount++;
          //}
          //else if (string.IsNullOrWhiteSpace(NewPIN) && Regex.IsMatch(originalValue[originalValue.Length - 1], "^[0-9]{4}$") &&
          //         !string.IsNullOrWhiteSpace(PIN))
          //{
          //    NewPIN = originalValue[originalValue.Length - 1];
          //}

          #endregion

          break;

        case States.ForgotPIN:

          #region Forgot Pin

          if (string.IsNullOrWhiteSpace(IDNumber))
          {
            IDNumber = originalValue[originalValue.Length - 1];

          }
          #endregion
          break;

        case States.Balance:

          #region Balance

          if (string.IsNullOrWhiteSpace(PrimaryAccountNumber))
          {
            PrimaryAccountNumber = originalValue[originalValue.Length - 1];

          }
          //else if (string.IsNullOrWhiteSpace(PIN) && PasswordHash.ValidatePassword(originalValue[originalValue.Length - 1], Project.Pin))
          //{
          //    PIN = originalValue[originalValue.Length - 1];
          //}

          #endregion
          break;

      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();

      switch (CurrentState)
      {
        case States.Pin:

          #region PIN

          if (string.IsNullOrWhiteSpace(Input))
          {
            sb.Append("CON ");
            sb.AppendLine("Welcome to DawaProjects Service");
            sb.AppendLine("Enter your 4 digit PIN:");
          }
          //else if (!string.IsNullOrWhiteSpace(Input) && !PasswordHash.ValidatePassword(Input, Project.Pin))
          //{
          //    sb.Append("CON ");
          //    sb.AppendLine("Invalid PIN!");
          //    sb.AppendLine($"{PINAttemptCount} attempts remaining");

          //    sb.AppendLine("Enter PIN:");
          //}
          else
          {
            sb.Append("CON ");
            sb.AppendLine("Welcome to DawaProjects Service");
            sb.AppendLine("Enter your 4 digit PIN:");
          }

          #endregion

          break;

        case States.ChangePIN:
          {
            if (string.IsNullOrWhiteSpace(PIN))
            {
              sb.Append("CON ");
              sb.AppendLine("Enter Current PIN:");
            }
            else if (string.IsNullOrWhiteSpace(NewPIN))
            {
              sb.Append("CON ");
              sb.AppendLine("Enter New PIN:");
            }
            else
            {
              sb.Append("CON ");
              sb.AppendLine(string.Format("Are you sure you want to change your pin from {0} to {1}?", PIN, NewPIN));
              sb.AppendLine("98. Accept");
              sb.AppendLine("99. Decline");
            }

            break;
          }
        case States.Menu:

          #region Menu

          sb.Append("CON ");
          sb.AppendLine("Welcome to DawaProjects Service.");
          sb.AppendLine("1: Top Up");
          sb.AppendLine("2: Validate customer withdrawal");
          sb.AppendLine("3: Change Pin");
          sb.AppendLine("4: Forgot Pin");
          sb.AppendLine("5: Check Balance");

          #endregion

          break;

        case States.TopUp:

          #region Top Up


          if (string.IsNullOrWhiteSpace(PrimaryAccountNumber))
          {
            sb.Append("CON ");
            sb.AppendLine($"Enter the agent account number:");

          }
          //else if (!string.IsNullOrEmpty(PrimaryAccountNumber) && string.IsNullOrWhiteSpace(Amount))
          //{
          //    var customerAccounts = _channelService.FindProjectAccountsByPANAsync(PrimaryAccountNumber).GetAwaiter().GetResult();

          //    if (customerAccounts == null || !customerAccounts.Any())
          //    {
          //        sb.Append("CON ");
          //        sb.AppendLine(
          //            $"Sorry the account {PrimaryAccountNumber} does not exist. Enter another account number:");

          //        PrimaryAccountNumber = string.Empty;
          //    }
          //    else
          //    {

          //        sb.Append("CON ");
          //        sb.AppendLine("Enter the amount to top up:");
          //    }

          //}
          //else if (!string.IsNullOrEmpty(Amount) && string.IsNullOrEmpty(PIN))
          //{

          //    var debitProjectAccount = _channelService.FindProjectAccountByProjectMobileNumberAsync($"+{MSISDN}").GetAwaiter().GetResult();

          //    var otherProjectAccounts = _channelService.FindProjectAccountsByPANAsync(PrimaryAccountNumber).GetAwaiter().GetResult();

          //    var creditProjectAccount = otherProjectAccounts.FirstOrDefault();

          //    switch ((ProjectAccountType)creditProjectAccount.Type)
          //    {
          //        case ProjectAccountType.NormalAgentAccount:
          //            ChannelType = (byte)TransactionTypeChannel.SuperAgentToAgent;
          //            break;

          //        case ProjectAccountType.SuperAgentAccount:
          //            {

          //                if (debitProjectAccount.Type == (byte)ProjectAccountType.NormalAgentAccount)
          //                {
          //                    ChannelType = (byte)TransactionTypeChannel.AgentToSuperAgent;
          //                }
          //                else
          //                {
          //                    ChannelType = (byte)TransactionTypeChannel.SuperAgentToAgent;
          //                }
          //                break;
          //            }

          //        case ProjectAccountType.Disbursement:
          //        case ProjectAccountType.Collections:
          //            ChannelType = (byte)TransactionTypeChannel.AgentToProject;
          //            break;

          //    }

          //    var productTransactionType = _channelService.FindProductTransactionTypeByTransactionTypeChannelAsync(ChannelType).GetAwaiter().GetResult();

          //    var product = _channelService.FindProductByIdAsync(productTransactionType.ProductId).GetAwaiter().GetResult();

          //    if (decimal.Parse(Amount) > product.MaximumLimit)
          //    {
          //        sb.Append("CON ");
          //        sb.AppendLine(
          //            $"Sorry the amount ({decimal.Parse(Amount)}) has exceeded the maximum limit ({product.MaximumLimit}). Enter amount:");

          //        Amount = string.Empty;
          //    }
          //    else if (decimal.Parse(Amount) < product.MinimumLimit)
          //    {

          //        sb.Append("CON ");
          //        sb.AppendLine(
          //            $"Sorry the amount ({decimal.Parse(Amount)}) is less than minimum limit ({product.MinimumLimit}). Enter amount:");

          //        Amount = string.Empty;
          //    }
          //    else if (decimal.Parse(Amount) > product.DailyLimit)
          //    {

          //        sb.Append("CON ");
          //        sb.AppendLine(
          //            $"Sorry the amount ({decimal.Parse(Amount)}) has exceeded the daily limit ({product.MinimumLimit}). Enter amount:");

          //        Amount = string.Empty;
          //    }
          //    else
          //    {
          //        sb.Append("CON ");
          //        sb.AppendLine("Enter your 4-digit pin:");
          //    }
          //}
          else if (!string.IsNullOrWhiteSpace(PrimaryAccountNumber) && !string.IsNullOrWhiteSpace(Amount) && !string.IsNullOrEmpty(PIN))
          {
            sb.Append("CON ");
            sb.AppendLine($"Top Up KES {Amount} to account {PrimaryAccountNumber}?");
            sb.AppendLine("98. Accept and top up");
            sb.AppendLine("97. Decline");
          }

          #endregion

          break;

        case States.Withdrawal:

          #region Withdrawal

          if (string.IsNullOrWhiteSpace(IDNumber))
          {
            sb.Append("CON ");
            sb.AppendLine($"Enter customer ID number: ");

          }
          else if (!string.IsNullOrWhiteSpace(IDNumber) && string.IsNullOrEmpty(ProjectMobileNumber))
          {
            sb.Append("CON ");
            sb.AppendLine("Enter the customer mobile number: ");
          }
          else if (string.IsNullOrEmpty(PIN))
          {
            sb.Append("CON ");
            sb.AppendLine("Enter your 4-digit pin:");
          }
          else
          {
            sb.Append("CON ");
            sb.AppendLine($"Verify customer with ID Number: {IDNumber} and mobile number: {ProjectMobileNumber}");
            sb.AppendLine("98. Verify");
            sb.AppendLine("97. Decline");
          }
          #endregion

          break;

        case States.ForgotPIN:

          #region ForgotPIN

          if (string.IsNullOrWhiteSpace(IDNumber))
          {
            sb.Append("CON ");
            sb.AppendLine("ID Number: ");
          }
          else
          {
            sb.Append("CON ");
            sb.AppendLine("Confirm PIN Reset:");
            sb.AppendLine($"Reset PIN for account mobile number {MSISDN}, ID number: {IDNumber}");
            sb.AppendLine("98: Accept");
            sb.AppendLine("99: Decline");
          }

          #endregion
          break;

        case States.Balance:

          #region Balance

          if (string.IsNullOrEmpty(PrimaryAccountNumber))
          {
            sb.Append("CON ");
            sb.AppendLine("Enter the account number:");
          }
          else if (string.IsNullOrWhiteSpace(PIN))
          {
            sb.Append("CON ");
            sb.AppendLine("Enter your 4-digit pin:");
          }
          else
          {
            sb.Append("CON ");
            sb.AppendLine($"Check balance for account {PrimaryAccountNumber}?");
            sb.AppendLine("98. Check balance");
            sb.AppendLine("97. Decline");
          }



          #endregion
          break;

        case States.Exit:

          #region Exit

          IDNumber = string.Empty;
          PIN = string.Empty;
          PrimaryAccountNumber = string.Empty;
          Amount = string.Empty;
          ProjectMobileNumber = string.Empty;

          sb.Append("END ");
          sb.AppendLine("Thank you for using DawaProjects Service");

          #endregion

          break;

        case States.Decline:

          #region Decline

          IDNumber = string.Empty;
          PrimaryAccountNumber = string.Empty;
          Amount = string.Empty;
          PIN = string.Empty;
          ProjectMobileNumber = string.Empty;

          sb.Append("CON ");
          sb.AppendLine("Request declined.");
          sb.AppendLine("0. Main Menu");
          sb.AppendLine("99. Exit");

          #endregion

          break;

        case States.Accept:

          #region Accept

          IDNumber = string.Empty;
          PrimaryAccountNumber = string.Empty;
          Amount = string.Empty;
          PIN = string.Empty;

          CurrentState = States.Menu;

          sb.Append("CON ");
          sb.AppendLine(AcceptMessage);


          break;

          #endregion
      }

      return sb.ToString();
    }

  }
}
