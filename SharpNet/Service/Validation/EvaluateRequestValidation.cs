using FluentValidation;
using SharpNet.Service.Request;

namespace SharpNet.Service.Validation
{
    public class EvaluateRequestValidation
        : AbstractValidator<EvaluateRequest>
    {
        public EvaluateRequestValidation()
        {
            RuleFor(
                request => request.Code
                )
                .NotEmpty()
                .NotNull();
        } 
    }
}