using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Validations.FluentValidations.CategoryValidation;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator(IBaseRepository<Category> categoryRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .MustAsync(async (name, cancellation) =>
            {
                var exists = await categoryRepository.GetAll()
                    .AnyAsync(c => c.Name.ToLower() == name.ToLower(), 
                        cancellationToken: cancellation);
                return !exists;
            })
            .WithMessage("Категория с таким именем уже существует");
    }
}