namespace tpaoProjeMvc.Models;

public class WellboreListViewModel
{
    public string ArananKelime { get; set; }
    public List<WellboreAramaDto> RandomWellbores { get; set; }
    public List<WellboreAramaDto> AramaSonuclari { get; set; }

}