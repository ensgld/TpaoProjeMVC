using System.Data;
using FluentValidation;
namespace tpaoProjeMvc.Models;

public class KuyuCreateModelValidator : AbstractValidator<KuyuCreateModel>
{
    public KuyuCreateModelValidator()
    {
        RuleFor(kuyu => kuyu.kuyuAdi).NotEmpty().WithMessage("Kuyu Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Kuyu Adı 3-20 Karakter Olmalı");

        RuleFor(kuyu => kuyu.SahaId).NotEmpty().WithMessage("Saha Seçimi Yapmak Zorundasınız!");


    }
}
public class KuyuEditModelValidator : AbstractValidator<KuyuEditModel>
{
    public KuyuEditModelValidator()
    {
        RuleFor(kuyu => kuyu.kuyuAdi).NotEmpty().WithMessage("Kuyu Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Kuyu Adı 3-20 Karakter Olmalı");

        RuleFor(kuyu => kuyu.SahaId).NotEmpty().WithMessage("Saha Seçimi Yapmak Zorundasınız!");
    }
}
