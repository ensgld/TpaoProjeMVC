using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tpaoProjeMvc.Models;

public class Saha
{
    public int SahaId { get; set; }

    public string sahaAdı { get; set; }
    public List<Kuyu> kuyuList { get; set; } = new();//newliyoruz çünkü bu sefer EF görmüyor

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string City { get; set; }


    // public Saha() { }
    // public Saha(string sahaAdı)//bu Constructor Entity Framework için bunu yaptık çünkü EF wellcount veya perWell'i bilmesine gerek yok onların database ile alakası yok
    // {
    //     this.sahaAdı = sahaAdı;
    // }

}
