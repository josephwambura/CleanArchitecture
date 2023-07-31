using System.Text.Encodings.Web;

using Ardalis.GuardClauses;

using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel.Models;

using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class ApplicationUsersController : BaseApiController
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IStringLocalizer<ApplicationUsersController> _localizer;
  private readonly ILogger<ApplicationUsersController> _logger;

  public ApplicationUsersController(IChannelService channelService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ApplicationUsersController> localizer,
    ILogger<ApplicationUsersController> logger)
  {
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _localizer = Guard.Against.Null(localizer, nameof(localizer));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
  }

  // GET: api/ApplicationUsers
  [HttpGet]
  public async Task<IActionResult> List(CancellationToken cancellationToken)
  {
    var result = await _channelService.FindApplicationUsersAsync(this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers/DatatablesList
  [HttpPost("DatatablesList")]
  public async Task<IActionResult> DatatablesList([FromBody] JQueryDataTablesFormRequest request, CancellationToken cancellationToken)
  {
    if (request == null)
    {
      return BadRequest();
    }

    var response = this.DataTablesJson(request, new PageCollectionInfo<ApplicationUserDTO>());

    // Validate the request model
    if (ModelState.IsValid)
    {
      var result = await _channelService.GetApplicationUsersWithFiltersAndInPageAsync(request?.Search?.Value, (int)request?.Start!, (int)request?.Length!, request?.Columns?.ElementAt((int)request?.Order?.FirstOrDefault()?.Column!)?.Name!, request?.Order?.FirstOrDefault()?.Dir!, HtmlHelpers.GetServiceHeader(this.HttpContext, User), cancellationToken);

      if (result.Status == ResultStatus.Ok)
      {
        response = this.DataTablesJson(request!, result.Value);
      }
      else if (result.Status == ResultStatus.Invalid)
      {
        return BadRequest(new
        {
          Title = _localizer["InvalidResponseMessage"],
          Detail = string.Join(',', result.ValidationErrors)
        });
      }
      else if (result.Status == ResultStatus.Error)
      {
        return BadRequest(new
        {
          Title = _localizer["ErrorResponseMessage"],
          Detail = string.Join(',', result.Errors)
        });
      }
      else if (result.Status == ResultStatus.NotFound)
      {
        return NotFound();
      }
    }

    return Ok(response);
  }

  // GET: api/ApplicationUsers
  [HttpGet("{id:Guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.FindApplicationUserAsync(id, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers
  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ApplicationUserBindingModel request, CancellationToken cancellationToken)
  {
    var result = await _channelService.AddApplicationUserAsync(_unitOfWork.MapTo<ApplicationUserDTO>(request), this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers/Create
  [HttpPost("Create")]
  public async Task<IActionResult> CreatePost([FromBody] ApplicationUserBindingModel request, CancellationToken cancellationToken)
  {
    var result = await _channelService.CreateApplicationUserAsync(_unitOfWork.MapTo<ApplicationUserDTO>(request), this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      if (!string.IsNullOrWhiteSpace(result.Value.UserId))
      {
        var staticSettingDTO = await _channelService.FindStaticSettingByKeyAsync(DefaultSettings.Instance.EmailMessageFrom!, this.GetServiceHeader(User), cancellationToken).ConfigureAwait(false);

        if (staticSettingDTO != null)
        {
          var callbackUrl = Url.Page(
          "/Account/ConfirmEmail",
          pageHandler: null,
          values: new { area = "Identity", userId = result.Value.UserId, code = result.Value.Code, /*returnUrl = returnUrl*/ },
          protocol: Request.Scheme);

          var emailAlertBindingModel = new EmailAlertBindingModel
          {
            From = staticSettingDTO?.Value?.Value,
            EmailMessageTo = request.Email,
            EmailMessageSubject = "Confirm your email",
            EmailMessageBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.",
            EmailMessageIsBodyHtml = true,
            EmailMessageSecurityCritical = true,
          };

          await _channelService.AddEmailAlertAsync(_unitOfWork.MapTo<EmailAlertDTO>(emailAlertBindingModel), this.GetServiceHeader(User), cancellationToken).ConfigureAwait(false);
        }
      }

      //return Ok(new { result.Value.UserId, result.Value.Code });
      return Ok(new APIResult(true, new { result.Value.UserId, result.Value.Code }));
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers/{userId}/ConfirmEmail/{code}
  [HttpPatch("{userId:Guid}/ConfirmEmail/{code}")]
  public async Task<IActionResult> ConfirmEmail(string userId, string code, CancellationToken cancellationToken)
  {
    var result = await _channelService.ConfirmApplicationUserEmailAsync(userId, code, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value.Succeeded);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers/Login
  [HttpPost("Login")]
  public async Task<IActionResult> Login([FromBody] AccountLoginBindingModel request, CancellationToken cancellationToken)
  {
    var result = await _channelService.LoginApplicationUserAsync(request, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value.Succeeded);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/ApplicationUsers/Bulk
  [HttpPost("Bulk")]
  public async Task<IActionResult> BulkPost([FromBody] List<ApplicationUserBindingModel> request, CancellationToken cancellationToken)
  {
    var result = await _channelService.BulkInsertApplicationUsersAsync(_unitOfWork.MapListTo<ApplicationUserDTO>(request), this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // PATCH: api/ApplicationUsers/delete/{id}
  [HttpDelete("delete/{id:Guid}")]
  public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.DeleteApplicationUserAsync(id, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok();
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }
}
