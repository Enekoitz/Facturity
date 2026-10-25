using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record GetCustomersRequest(
    string? Alias, 
    string? TaxIdType, 
    string? TaxIdValue, 
    string? TaxIdCountryCode, 
    string? TaxName,
    string? Address,
    bool? Active,
    int Page = 1,
    int Size = 20);

internal static class GetCustomersRequestMapperExtension
{
    internal static Result<GetCustomersQuery> MapToQuery(this GetCustomersRequest request)
    {
        TaxIdType? taxIdType;
        if (string.IsNullOrWhiteSpace(request.TaxIdType))
            taxIdType = null;
        else if (Enum.TryParse<TaxIdType>(request.TaxIdType, ignoreCase: true, out var e)
                 && Enum.IsDefined(e))
            taxIdType = e;
        else
            return Result.Fail<GetCustomersQuery>(CustomerErrors.TaxIdTypeInvalid);

        int page =  request.Page <= 0 ? 1 : request.Page;
        int size = request.Size switch
        {
            <= 0 => 10,
            > 100 => 100,
            _ => request.Size
        };
            
        
        var query = new GetCustomersQuery(
            request.Alias,
            taxIdType,
            request.TaxIdValue,
            request.TaxIdCountryCode,
            request.TaxName,
            request.Address,
            request.Active,
            page,
            size);

        return Result.Ok(query);
    }
}