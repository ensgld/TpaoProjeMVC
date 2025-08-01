namespace tpaoProjeMvc.Models;

public class GenelAramaModel
{
    public string Sorgu { get; set; }
    public List<Saha> Sahalar { get; set; }
    public List<Kuyu> Kuyular { get; set; }
    public List<Wellbore> Wellbores { get; set; }
}