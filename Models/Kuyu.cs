using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tpaoProjeMvc.Models;

public class Kuyu
{
    public int KuyuId { get; set; }
    public string kuyuAdı { get; set; }
    public List<Wellbore> wellbores { get; set; } = new();
    //şimdi burada ilişikiyi kurmamız gerek Saha ile Kuyu arasında one-to-many ilişkisi var yani bir sahanın birden fazla kuyusu olabilir ama kuyunun bir sahası olur bu yüzden mesela Saha classına bir liste olarak sahip olduğu kuyuların listesini vereceğiz Kuyuya ise ait olduğu Sahanın Id'sini ve objesini database'den çekeceğiz 
    public Saha Saha { get; set; }//navigationProperty
    public int? SahaId { get; set; }//foreign key

}
