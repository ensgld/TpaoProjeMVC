using System.Data;
using FluentValidation;
namespace tpaoProjeMvc.Models;

public class SahaCreateModelValidator : AbstractValidator<SahaCreateModel>
{
    public SahaCreateModelValidator()
    {
        RuleFor(saha => saha.sahaAdı).NotEmpty().WithMessage("Saha Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Saha Adı 3-20 Karakter Olmalı");

        RuleFor(saha => saha.Latitude).NotEmpty().WithMessage("Enlem Değeri Girmeniz Zorunludur");

        RuleFor(saha => saha.Longitude).NotEmpty().WithMessage("Boylam Değeri Girmeniz Zorunludur");

        RuleFor(saha => saha.City).NotEmpty().WithMessage("Şehir Bilgisi Girmeniz zorunludur").Length(3, 20).WithMessage("Şehir Bilgisi 3-20 Karakter Olmalı");
    }
}
public class SahaEditModelValidator : AbstractValidator<SahaEditModel>
{
    public SahaEditModelValidator()
    {
        RuleFor(saha => saha.sahaAdı).NotEmpty().WithMessage("Saha Adı Boş Bırakılamaz").Length(3, 20).WithMessage("Saha Adı 3-20 Karakter Olmalı");

        RuleFor(saha => saha.Latitude).NotEmpty().WithMessage("Enlem Değeri Girmeniz Zorunludur");

        RuleFor(saha => saha.Longitude).NotEmpty().WithMessage("Boylam Değeri Girmeniz Zorunludur");

        RuleFor(saha => saha.City).NotEmpty().WithMessage("Şehir Bilgisi Girmeniz zorunludur").Length(3, 20).WithMessage("Şehir Bilgisi 3-20 Karakter Olmalı");
    }
}
