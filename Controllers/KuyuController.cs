using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using tpaoProjeMvc.Models;

namespace tpaoProjeMvc.Controllers;

public class KuyuController : Controller
{
    private readonly DataContext _dataContext;
    public KuyuController(DataContext context)
    {
        _dataContext = context;
    }

    public ActionResult Index()
    {
        var randomKuyular = _dataContext.Kuyular.OrderBy(r => Guid.NewGuid()).Take(16).Select(kuyu => new KuyuAramaDto
        {
            KuyuId = kuyu.KuyuId,
            KuyuAdi = kuyu.kuyuAdı,
            SahaAdi = kuyu.Saha.sahaAdı,
        }).ToList();


        var model = new KuyuListViewModel
        {
            RandomKuyular = randomKuyular,
        };
        return View(model);
    }
    public ActionResult List(string arananKelime)
    {
        var query = _dataContext.Kuyular.Include(k => k.Saha).Select(kuyu => new KuyuAramaDto
        {
            KuyuId = kuyu.KuyuId,
            KuyuAdi = kuyu.kuyuAdı,
            SahaAdi = kuyu.Saha.sahaAdı

        });
        if (!string.IsNullOrEmpty(arananKelime))
        {
            query = query.Where(kuyu => kuyu.KuyuAdi.ToLower().Contains(arananKelime.ToLower()));
        }
        var model = new KuyuListViewModel
        {
            ArananKelime = arananKelime,
            AramaSonuclari = query.ToList(),
        };

        return View(model);

    }
    public async Task<ActionResult> Details(int id)
    {
        Kuyu kuyu = await _dataContext.Kuyular
        .AsNoTracking()
        .Include(k => k.Saha)
        .Include(k => k.wellbores)
        .FirstOrDefaultAsync(k => k.KuyuId == id);
        if (kuyu == null)
        {
            return RedirectToAction("Index", "Home");
        }



        return View(kuyu);
    }
    public ActionResult Create()
    {

        return View(new KuyuCreateModel());
    }
    [HttpPost]
    public ActionResult Create(KuyuCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var entity = new Kuyu
            {
                kuyuAdı = model.kuyuAdi,
                SahaId = (int)model.SahaId!
            };
            _dataContext.Kuyular.Add(entity);
            TempData["Mesaj"] = $"{entity.kuyuAdı} Kuyusu Veritabanına Eklendi";

            _dataContext.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
        return View(model);
    }
    public JsonResult Ara(string arananKuyu)
    {
        var results = _dataContext.Kuyular
            .Where(k => k.kuyuAdı.Contains(arananKuyu))
            .Select(k => new
            {
                id = k.KuyuId,
                text = k.kuyuAdı
            }).ToList();

        return Json(new { results });
    }
    public ActionResult Edit(int id)
    {

        var entity = _dataContext.Kuyular.Select(i => new KuyuEditModel
        {
            KuyuId = i.KuyuId,
            kuyuAdi = i.kuyuAdı,
            SahaId = (int)i.SahaId!
        }).FirstOrDefault(k => k.KuyuId == id);



        return View(entity);

    }
    [HttpPost]
    public ActionResult Edit(int KuyuId, KuyuEditModel model)
    {
        if (KuyuId != model.KuyuId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            var entity = _dataContext.Kuyular.FirstOrDefault(s => s.KuyuId == model.KuyuId);

            if (entity != null)
            {
                entity.kuyuAdı = model.kuyuAdi;
                entity.SahaId = model.SahaId;
                _dataContext.SaveChanges();
                TempData["Mesaj"] = $"{entity.kuyuAdı} Kuyusu Güncellendi";

                return RedirectToAction("Index", "Home");
            }

        }
        return View(model);
    }
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            TempData["Mesaj"] = "Kuyu Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Kuyular.FirstOrDefault(k => k.KuyuId == id);
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
            TempData["Mesaj"] = "Kuyu Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Kuyular.FirstOrDefault(k => k.KuyuId == id);
        if (entity != null)
        {
            _dataContext.Kuyular.Remove(entity);
            _dataContext.SaveChanges();
            TempData["Mesaj"] = $"{entity.kuyuAdı} Kuyusu Silindi";

        }
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public ActionResult GetKuyularForGrid()
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

        var query = _dataContext.Kuyular
    .Include(k => k.Saha)
    .Include(k => k.wellbores)
    .Select(k => new
    {
        kuyuId = k.KuyuId,
        kuyuAdı = k.kuyuAdı,
        latitude = k.Saha.Latitude,
        longitude = k.Saha.Longitude,
        city = k.Saha.City,
        wellboreCount = k.wellbores.Count,

        wellboreListHtml = $@"
            <div id='wellboreList_{k.KuyuId}' class='wellbore-list d-none'>
                <ul class='list-unstyled mb-0'>
                    {string.Join("", k.wellbores.Select(w => $@"
                        <li>
                            <a class='btn btn-sm btn-outline-secondary mb-1' href='/Wellbore/Details/{w.WellboreId}'>
                                <i class='fa fa-search me-1'></i> {System.Net.WebUtility.HtmlEncode(w.wellboreName)}
                            </a>
                        </li>
                    "))}
                </ul>
            </div>
            <button class='btn btn-sm btn-outline-primary toggle-wellbore-btn' data-target='#wellboreList_{k.KuyuId}'>
                Göster
            </button>
        ",

        actionButtons = $"<a href='/Kuyu/Edit/{k.KuyuId}' class='btn btn-primary btn-sm'><i class='fa fa-edit'></i></a> " +
                        $"<a href='/Kuyu/Delete/{k.KuyuId}' class='btn btn-danger btn-sm'><i class='fa fa-trash'></i></a>",
        detailsButton = $"<a href='/Kuyu/Details/{k.KuyuId}' class='btn btn-sm btn-outline-danger'>Detay</a>"
    });


        //Arama için
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(k => k.kuyuId.ToString().Contains(searchValue) || k.kuyuAdı.ToLower().Contains(searchValue.ToLower())//toStringler performans açısından kötü olabilir dene eğer kötü olursa çıkar

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
        int recordsTotal = _dataContext.Kuyular.Count(); // Toplam saha sayısı, filtrelenmemiş hali

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
