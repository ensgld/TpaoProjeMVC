using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tpaoProjeMvc.Models;

public class KuyuModel
{
    public string kuyuAdi { get; set; }
    public int? SahaId { get; set; }//foreign key
}