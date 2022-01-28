using FluentValidation;

namespace Orders.Application.Commands.OrderCreate
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateCommond>
    {
        public OrderCreateValidator()
        {
            RuleFor(x => x.SellerUserName)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }
}
