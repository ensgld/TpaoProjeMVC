using System.ComponentModel.DataAnnotations;

namespace tpaoProjeMvc.Models;

public class WellboreModel
{
    [Required(ErrorMessage = "Wellbore Adı Girmelisiniz.")]
    [Display(Name = "Kuyu Adı")]
    [StringLength(20, ErrorMessage = "Wellbore Adı 3-20 Karakter Olmalı", MinimumLength = 3)]
    public string wellboreName { get; set; }
    [Required(ErrorMessage = "Kuyu Seçimi Yapmalısınız.")]
    public int? KuyuId { get; set; }
}
