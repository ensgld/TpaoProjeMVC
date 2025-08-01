using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Dynamic.Core;
using tpaoProjeMvc.Models;
namespace tpaoProjeMvc.Controllers;

public class SahaController : Controller
{
    private readonly DataContext _dataContext;
    public SahaController(DataContext context)
    {
        _dataContext = context;
    }

    public ActionResult Index()
    {

        var randomSahaIds = _dataContext.Sahalar
               .OrderBy(s => Guid.NewGuid())  // rnd.Next() yerine daha iyi
               .Select(s => s.SahaId)
               .Take(12)
               .ToList();
        var randomSahaList = _dataContext.Sahalar
            .AsNoTracking()
            .Where(s => randomSahaIds.Contains(s.SahaId))
            .Include(s => s.kuyuList)
            .ToList();

        ViewData["RandomSahaList"] = randomSahaList;
        return View();
    }
    public ActionResult List(string arananKelime)
    {
        var query = _dataContext.Sahalar.AsQueryable();
        if (!string.IsNullOrEmpty(arananKelime))
        {
            query = query.Where(i => i.sahaAdı.ToLower().Contains(arananKelime.ToLower()));
        }
        ViewData["arananKelime"] = arananKelime;

        return View(query.ToList());

    }
    public async Task<ActionResult> Details(int id)
    {
        var saha = await _dataContext.Sahalar
         .AsNoTracking()
         .Include(s => s.kuyuList)
         .FirstOrDefaultAsync(s => s.SahaId == id);

        if (saha == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(saha);
    }
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(SahaCreateModel model)
    {
        if (ModelState.IsValid)
        {

            var entity = new Saha
            {
                sahaAdı = model.sahaAdı,
                City = model.City,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            _dataContext.Sahalar.Add(entity);
            TempData["Mesaj"] = $"{entity.sahaAdı} Sahası Veritabanına Eklendi";
            _dataContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
    public JsonResult Ara(string arananSaha)
    {
        var results = _dataContext.Sahalar
            .Where(s => s.sahaAdı.Contains(arananSaha))
            .Select(s => new
            {
                id = s.SahaId,
                text = s.sahaAdı
            }).ToList();

        return Json(new { results });
    }
    public ActionResult Edit(int id)
    {
        var entity = _dataContext.Sahalar.Select(i => new SahaEditModel
        {
            SahaId = i.SahaId,
            sahaAdı = i.sahaAdı,
            Latitude = i.Latitude,
            Longitude = i.Longitude,
            City = i.City

        }).FirstOrDefault(s => s.SahaId == id);



        return View(entity);
    }
    [HttpPost]
    public ActionResult Edit(int SahaId, SahaEditModel model)
    {

        if (SahaId != model.SahaId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {

            var entity = _dataContext.Sahalar.FirstOrDefault(s => s.SahaId == model.SahaId);

            if (entity != null)
            {
                entity.sahaAdı = model.sahaAdı;
                entity.Latitude = model.Latitude;
                entity.Longitude = model.Longitude;
                entity.City = model.City;
                _dataContext.SaveChanges();

                TempData["Mesaj"] = $"{entity.sahaAdı} Sahası Güncellendi";
                return RedirectToAction("Index", "Home");
            }
        }
        return View(model);
    }
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            TempData["Mesaj"] = "Saha Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Sahalar.FirstOrDefault(s => s.SahaId == id);
        if (entity != null)
        {
            return View(entity);

        }
        return View();
    }
    [HttpPost]
    public ActionResult DeleteConfirm(int id)
    {
        if (id == null)
        {
            TempData["Mesaj"] = "Saha Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Sahalar.FirstOrDefault(s => s.SahaId == id);
        if (entity != null)
        {
            _dataContext.Sahalar.Remove(entity);
            _dataContext.SaveChanges();
            TempData["Mesaj"] = $"{entity.sahaAdı} Sahası Silindi";

        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public ActionResult GetSahalarForGrid()
    {
        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
        var sortColumnName = Request.Form[$"columns[{sortColumnIndex}][data]"].FirstOrDefault();
        var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();


        int pageSize = length != null ? Convert.ToInt32(length) : 10;
        int skip = start != null ? Convert.ToInt32(start) : 0;

        var query = _dataContext.Sahalar
    .Include(s => s.kuyuList)
    .Select(s => new
    {
        sahaId = s.SahaId,
        sahaAdı = s.sahaAdı,
        latitude = s.Latitude,
        longitude = s.Longitude,
        city = s.City,
        kuyuCount = s.kuyuList.Count,

        // Kuyular için açılır kapanır liste
        kuyuListHtml = $@"
            <div id='kuyuList_{s.SahaId}' class='kuyu-list d-none'>
                <ul class='list-unstyled mb-0'>
                    {string.Join("", s.kuyuList.Select(k => $@"
                        <li>
                            <a class='btn btn-sm btn-outline-secondary mb-1' href='/Kuyu/Details/{k.KuyuId}'>
                                <i class='fa fa-search me-1'></i> {System.Net.WebUtility.HtmlEncode(k.kuyuAdı)}
                            </a>
                        </li>
                    "))}
                </ul>
            </div>
            <button class='btn btn-sm btn-outline-primary toggle-kuyular-btn' data-target='#kuyuList_{s.SahaId}'>
                Göster
            </button>
        ",

        actionButtons = $@"
            <a href='/Saha/Edit/{s.SahaId}' class='btn btn-primary btn-sm'>
                <i class='fa fa-edit'></i>
            </a>
            <a href='/Saha/Delete/{s.SahaId}' class='btn btn-danger btn-sm'>
                <i class='fa fa-trash'></i>
            </a>",

        detailsButton = $@"
            <a href='/Saha/Details/{s.SahaId}' class='btn btn-sm btn-outline-danger'>
                Detay
            </a>"
    });




        //Arama için
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(s => s.sahaId.ToString().Contains(searchValue) || s.sahaAdı.ToLower().Contains(searchValue.ToLower()) ||
            s.city.ToLower().Contains(searchValue.ToLower()) || s.longitude.ToString().Contains(searchValue) || s.latitude.ToString().Contains(searchValue)//toStringler performans açısından kötü olabilir dene eğer kötü olursa çıkar

            );
        }
        // Sıralama için 
        if (!string.IsNullOrEmpty(sortColumnName) && !string.IsNullOrEmpty(sortDirection))
        { //sortDirection"asc" ise normal default olduğu gibi artan soralama yap değilse OrderByDescending ile sırala
          // query = sortDirection == "asc" // buradaki dataTable'dan aldığımız sortDirection bilgisi datatabledan gelir bizim seçimimizle asc veya desc olur
          //     ? query.OrderBy(sortColumnName)
          //     : query.OrderByDescending(sortColumnName);
            query = query.OrderBy($"{sortColumnName} {sortDirection}");
        }

        int recordsFiltered = query.Count();
        int recordsTotal = _dataContext.Sahalar.Count(); // Toplam saha sayısı, filtrelenmemiş hali

        var data = query.Skip(skip).Take(pageSize).ToList();

        return Json(new
        {
            draw = draw,
            recordsFiltered = recordsFiltered,
            recordsTotal = recordsTotal,
            data = data
        });



    }
}
