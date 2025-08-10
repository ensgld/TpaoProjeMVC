using System.Data;
using FluentValidation;
namespace tpaoProjeMvc.Models;

public class WellboreCreateModelValidator : AbstractValidator<WellboreCreateModel>
{
    public WellboreCreateModelValidator()
    {
        RuleFor(wellbore => wellbore.wellboreName).NotEmpty().WithMessage("Wellbore Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Wellbore Adı 3-20 Karakter Olmalı");

        RuleFor(wellbore => wellbore.KuyuId).NotEmpty().WithMessage("Kuyu Seçimi Yapmak Zorundasınız!");


    }
}
public class WellboreEditModelValidator : AbstractValidator<WellboreEditModel>
{
    public WellboreEditModelValidator()
    {
        RuleFor(wellbore => wellbore.wellboreName).NotEmpty().WithMessage("Wellbore Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Wellbore Adı 3-20 Karakter Olmalı");

        RuleFor(wellbore => wellbore.KuyuId).NotEmpty().WithMessage("Kuyu Seçimi Yapmak Zorundasınız!");
    }
}
