using Ardalis.Specification;
using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Specifications;

public class EmailAlertsByDLRStatusAndWithFiltersAndInPageSpec : Specification<EmailAlert>, ISingleResultSpecification
{
  public EmailAlertsByDLRStatusAndWithFiltersAndInPageSpec(byte[] dlrStatuses, string? searchString, int pageSize, string sortColumn, string sortDirection)
  {
    Query
      .Where(emailAlert => dlrStatuses.Contains(emailAlert.DLRStatus));
    
    //Query
    //  .Where(emailAlert => emailAlert.DLRStatus == dlrStatuses[0] || emailAlert.DLRStatus == dlrStatuses[1]);

    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(emailAlert => emailAlert.EmailMessage.Body.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    Query.Take(pageSize);
  }
}
