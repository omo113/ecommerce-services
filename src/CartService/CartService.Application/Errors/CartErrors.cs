using EcommerceServices.Shared;

namespace CartService.Application.Errors;

public static class CartErrors
{
    private const string ErrorCode = "Cart";
    public static Error FailedToDeleteCart => new(ErrorCode, "failed to delete cart");
    public static Error CartNotExist => new(ErrorCode, "cart does not exist");
    public static Error IdAlreadyExist => new(ErrorCode, "this kind of Id already exists");
}