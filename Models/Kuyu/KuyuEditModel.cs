using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tpaoProjeMvc.Models;

public class KuyuEditModel : KuyuModel
{
    public int KuyuId { get; set; }

}