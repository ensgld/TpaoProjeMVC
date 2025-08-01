using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tpaoProjeMvc.Models;
using System;

namespace tpaoProjeMvc.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _dataContext;
    public HomeController(DataContext context)
    {
        _dataContext = context;
    }
    public ActionResult Index()
    {
        var locations = _dataContext.Sahalar
       .Select(s => s.City)
       .Distinct()
       .ToList();

        var model = new MapViewModel
        {
            Locations = locations,
            Sahalar = _dataContext.Sahalar.Include(s => s.kuyuList).ToList(),
            Kuyular = _dataContext.Kuyular.Include(k => k.wellbores).ToList(),
            Wellbores = _dataContext.Wellbores.ToList()
        };

        return View(model);
    }

    public JsonResult GetSahalarByLocation(string locationName)
    {
        var query = _dataContext.Sahalar.AsQueryable();

        if (!string.IsNullOrEmpty(locationName))
        {
            query = query.Where(s => s.City == locationName);
        }

        var location = query.Select(s => new
        {
            s.SahaId,
            s.sahaAdı,
            s.Latitude,
            s.Longitude,
            s.City
        }).ToList();

        return Json(location);
    }

    public ActionResult List(string arananKelime)
    {

        var sahaList = _dataContext.Sahalar
        .Where(s => s.sahaAdı.ToLower().Contains(arananKelime))
        .ToList();

        var kuyuList = _dataContext.Kuyular.Include(k => k.Saha)
            .Where(k => k.kuyuAdı.ToLower().Contains(arananKelime))
            .ToList();

        var wellboreList = _dataContext.Wellbores.Include(w => w.Kuyu).ThenInclude(w => w.Saha)
            .Where(w => w.wellboreName.ToLower().Contains(arananKelime))
            .ToList();
        var model = new GenelAramaModel
        {
            Sahalar = sahaList,
            Kuyular = kuyuList,
            Wellbores = wellboreList,
            Sorgu = arananKelime
        };
        ViewData["ArananKelime"] = arananKelime;


        return View(model);
    }


}


