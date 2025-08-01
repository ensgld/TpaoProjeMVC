using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tpaoProjeMvc.Models;

public class KuyuModel
{


    [Required(ErrorMessage = "Kuyu Adı Girmelisiniz.")]
    [Display(Name = "Kuyu Adı")]
    [StringLength(20, ErrorMessage = "Kuyu Adı 3-20 Karakter Olmalı", MinimumLength = 3)]
    public string kuyuAdi { get; set; }
    [Required(ErrorMessage = "Saha Seçimi Yapmalısınız.")]
    public int? SahaId { get; set; }//foreign key
}