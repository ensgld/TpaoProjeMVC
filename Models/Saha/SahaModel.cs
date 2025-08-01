using System.ComponentModel.DataAnnotations;

namespace tpaoProjeMvc.Models;


public class SahaModel
{
    [Display(Name = "SahaId")]
    public int SahaId { get; set; }
    [Required(ErrorMessage = "Saha Adı Girmelisiniz.")]
    [Display(Name = "Saha Adı")]
    [StringLength(20, ErrorMessage = "Saha Adı 3-20 Karakter Olmalı", MinimumLength = 3)] public string sahaAdı { get; set; }
    [Display(Name = "Enlem")]
    [Required(ErrorMessage = "Enlem Girmeniz Zorunludur.")]

    public double Latitude { get; set; }
    [Display(Name = "Boylam")]
    [Required(ErrorMessage = "Boylam Girmeniz Zorunludur.")]
    public double Longitude { get; set; }
    [Required(ErrorMessage = "Şehir Girmeniz Zorunludur.")]
    [Display(Name = "Şehir")]
    [StringLength(20, ErrorMessage = "Şehir Bilgis 3-20 Karakter Olmalı", MinimumLength = 3)]
    public string City { get; set; }

}