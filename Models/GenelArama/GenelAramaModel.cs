namespace tpaoProjeMvc.Models;

public class GenelAramaModel
{
    public string Sorgu { get; set; }
    public List<SahaAramaDto> Sahalar { get; set; }
    public List<KuyuAramaDto> Kuyular { get; set; }
    public List<WellboreAramaDto> Wellbores { get; set; }
}