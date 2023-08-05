using System.Text.Encodings.Web;

using Ardalis.GuardClauses;

using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Pages.UserManagement.ApplicationUsers;

public class CreateModel : AuthorizedPageModelBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IEmailSender _emailSender;
  private readonly IStringLocalizer<CreateModel> _localizer;
  private readonly ILogger<CreateModel> _logger;

  public CreateModel(UserManager<ApplicationUser> userManager,
    IChannelService channelService,
    IUnitOfWork unitOfWork,
    IEmailSender emailSender,
    IStringLocalizer<CreateModel> localizer,
    ILogger<CreateModel> logger)
  {
    _userManager = Guard.Against.Null(userManager, nameof(userManager));
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    _emailSender = Guard.Against.Null(emailSender, nameof(emailSender));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _localizer = Guard.Against.Null(localizer, nameof(localizer));
  }

  public IFormFile? ProfilePictureFile { get; set; }

  public List<SelectListItem>? ApplicationUserCollectionAccounts { get; set; } = default!;

  public IActionResult OnGet()
  {
    //var roles = await _applicationDbContext.Roles.ToListAsync();

    //ApplicationUserBindingModel = new ApplicationUserBindingModel
    //{
    //    ApplicationRoleSelectListItems = roles?.Select(r => new ApplicationRoleSelectListItem { Display = r.Name, Id = r.Id })?.ToArray(),
    //};

    return Page();
  }

  [BindProperty]
  public ApplicationUserBindingModel ApplicationUserBindingModel { get; set; } = default!;

  // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
  public async Task<IActionResult> OnPostAsync()
  {
    if (!ModelState.IsValid || ApplicationUserBindingModel == null)
    {
      return Page();
    }

    var filelist = HttpContext.Request.Form.Files;
    if (filelist.Count > 0)
    {
      foreach (var file in filelist)
      {
        var uploads = @"C:\\Clean.Architecture\Uploads\ApplicationUsers\ProfilePictures";
        string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string SavePath = Path.Combine(uploads, FileName);

        using (var fileStream = new FileStream(SavePath, FileMode.Create))
        {
          await file.CopyToAsync(fileStream);
        }

        ApplicationUserBindingModel.ProfilePicture = FileName;
      }
    }

    var result = await _channelService.CreateApplicationUserAsync(_unitOfWork.MapTo<ApplicationUserDTO>(ApplicationUserBindingModel), this.GetServiceHeader(User), CancellationToken.None);

    if (result.Value.Item1.Succeeded)
    {
      _logger.LogInformation("a new user account created  with password.");

      //if (ApplicationUserBindingModel!.ApplicationRoleSelectListItems != null && ApplicationUserBindingModel!.ApplicationRoleSelectListItems.Any())
      //{
      //  foreach (var role in ApplicationUserBindingModel!.ApplicationRoleSelectListItems!.Where(a => a.Selected).Select(a => a.Display).ToList())
      //  {
      //    ApplicationUserBindingModel.ApplicationRoles?.Add(role!);
      //  }
      //}

      //if (ApplicationUserBindingModel.ApplicationRoles != null && ApplicationUserBindingModel.ApplicationRoles.Count != 0)
      //{
      //  result = await _userManager.AddToRolesAsync(user, ApplicationUserBindingModel.ApplicationRoles);

      //  _logger.LogInformation($"User added to {string.Join(Environment.NewLine, ApplicationUserBindingModel.ApplicationRoles)} roles.");
      //}

      var callbackUrl = Url.Page(
          "/Account/ConfirmEmail",
          pageHandler: null,
          values: new { area = "Identity", userId = result.Value.UserId, code = result.Value.Code, /*returnUrl = returnUrl*/ },
          protocol: Request.Scheme);

      _logger.LogInformation($"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.", "Confirm your email");

      await _emailSender.SendEmailAsync(ApplicationUserBindingModel.Email!, "Confirm your email",
          $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

      //var staticSettingDTO = await _channelService.GetStaticSettingByKeyAsync(DefaultSettings.Instance.EmailMessageFrom!, this.GetServiceHeader(User!));

      //var emailAlertBindingModel = new EmailAlertBindingModel
      //{
      //  From = staticSettingDTO?.Value?.Value,
      //  EmailMessageTo = ApplicationUserBindingModel.Email,
      //  EmailMessageSubject = "Confirm your email",
      //  EmailMessageBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.",
      //  EmailMessageIsBodyHtml = true,
      //  EmailMessageSecurityCritical = true,
      //};

      //await _channelService.AddEmailAlertAsync(_unitOfWork.MapTo<EmailAlertDTO>(emailAlertBindingModel));

      if (_userManager.Options.SignIn.RequireConfirmedAccount)
      {
        //return RedirectToPage("RegisterConfirmation", new { email = request.Email, /*returnUrl = returnUrl */ });

        return RedirectToPage("./Index");
      }
    }

    return RedirectToPage("./Index");
  }
}
