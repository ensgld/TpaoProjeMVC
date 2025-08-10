namespace tpaoProjeMvc.Models;

public class SahaAramaDto
{
    public int SahaId { get; set; }
    public string SahaAdi { get; set; }
    public string City { get; set; }


}

public class KuyuAramaDto
{
    public int KuyuId { get; set; }
    public string KuyuAdi { get; set; }
    public string SahaAdi { get; set; }
}

public class WellboreAramaDto
{
    public int WellboreId { get; set; }
    public string WellboreName { get; set; }
    public string KuyuAdi { get; set; }
    public string SahaAdi { get; set; }
}