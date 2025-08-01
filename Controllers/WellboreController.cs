using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tpaoProjeMvc.Models;
using System.Linq.Dynamic.Core;
namespace tpaoProjeMvc.Controllers;

public class WellboreController : Controller
{
    private readonly DataContext _dataContext;
    public WellboreController(DataContext context)
    {
        _dataContext = context;
    }

    public ActionResult Index()
    {
        List<Wellbore> wellboreList = _dataContext.Wellbores.Include(k => k.Kuyu).ThenInclude(k => k.Saha).ToList();
        var randomWellboreIds = _dataContext.Wellbores
          .OrderBy(w => Guid.NewGuid())
          .Select(w => w.WellboreId)
          .Take(12)
          .ToList();

        var randomWellboreList = _dataContext.Wellbores
            .AsNoTracking()
            .Where(w => randomWellboreIds.Contains(w.WellboreId))
            .Include(w => w.Kuyu)
                .ThenInclude(k => k.Saha)
            .ToList();

        ViewData["RandomWellboreList"] = randomWellboreList;
        return View(randomWellboreList);


    }
    public ActionResult List(string arananKelime)
    {
        var query = _dataContext.Wellbores.Include(w => w.Kuyu).ThenInclude(w => w.Saha).AsQueryable();
        if (!string.IsNullOrEmpty(arananKelime))
        {
            query = query.Where(i => i.wellboreName.ToLower().Contains(arananKelime.ToLower()));
        }
        ViewData["arananKelime"] = arananKelime;

        return View(query.ToList());

    }

    public async Task<ActionResult> Details(int id)
    {
        var wellbore = await _dataContext.Wellbores
           .AsNoTracking()
           .Include(w => w.Kuyu)
               .ThenInclude(k => k.Saha)
           .FirstOrDefaultAsync(w => w.WellboreId == id);

        if (wellbore == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(wellbore);
    }
    public ActionResult Create()
    {

        return View(new WellboreCreateModel());
    }
    [HttpPost]
    public ActionResult Create(WellboreCreateModel model)
    {
        if (ModelState.IsValid)
        {

            var entity = new Wellbore
            {
                wellboreName = model.wellboreName,
                KuyuId = (int)model.KuyuId!
            };
            _dataContext.Wellbores.Add(entity);
            _dataContext.SaveChanges();
            TempData["Mesaj"] = $"{entity.wellboreName} Wellbore'u Veritabanına Eklendi";

            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
    public ActionResult Edit(int id)
    {
        var entity = _dataContext.Wellbores.Select(i => new WellboreEditModel
        {
            WellboreId = i.WellboreId,
            wellboreName = i.wellboreName,
            KuyuId = i.KuyuId
        }).FirstOrDefault(w => w.WellboreId == id);



        return View(entity); ;
    }
    [HttpPost]
    public ActionResult Edit(int WellboreId, WellboreEditModel model)
    {
        if (WellboreId != model.WellboreId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {

            var entity = _dataContext.Wellbores.FirstOrDefault(s => s.WellboreId == model.WellboreId);

            if (entity != null)
            {
                entity.wellboreName = model.wellboreName;
                entity.KuyuId = (int)model.KuyuId!;
                _dataContext.SaveChanges();
                TempData["Mesaj"] = $"{entity.wellboreName} Wellbore'u Güncellendi";

                return RedirectToAction("Index", "Home");
            }
        }
        return View(model);
    }
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            TempData["Mesaj"] = "Wellbore Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Wellbores.FirstOrDefault(w => w.WellboreId == id);
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
            TempData["Mesaj"] = "Wellbore Bulunamadı";
            return RedirectToAction("Index");
        }
        var entity = _dataContext.Wellbores.FirstOrDefault(w => w.WellboreId == id);
        if (entity != null)
        {
            _dataContext.Wellbores.Remove(entity);
            _dataContext.SaveChanges();
            TempData["Mesaj"] = $"{entity.wellboreName} Sahası Silindi";

        }
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    public ActionResult GetWellboresForGrid()
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

        var query = _dataContext.Wellbores.Include(w => w.Kuyu)
        .Select(w => new
        { //Burası tablodaki gördüğümüz kısımları tek tek şelilde seçip onları ele aldık
            wellboreId = w.WellboreId,
            wellboreName = w.wellboreName,
            latitude = w.Kuyu.Saha.Latitude,
            longitude = w.Kuyu.Saha.Longitude,
            city = w.Kuyu.Saha.City,
            ownKuyuName = w.Kuyu.kuyuAdı,
            ownSahaName = w.Kuyu.Saha.sahaAdı,

            actionButtons = $"<a href='/Wellbore/Edit/{w.WellboreId}' class='btn btn-primary btn-sm'><i class='fa fa-edit'></i></a> " +
                            $"<a href='/Wellbore/Delete/{w.WellboreId}' class='btn btn-danger btn-sm'><i class='fa fa-trash'></i></a>",
            detailsButton = $"<a href='/Wellbore/Details/{w.WellboreId}' class='btn btn-sm btn-outline-danger'>Detay</a>"
        });


        //Arama için
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(w => w.wellboreId.ToString().Contains(searchValue) || w.wellboreName.ToLower().Contains(searchValue.ToLower()) ||
            w.city.ToLower().Contains(searchValue.ToLower()) || w.longitude.ToString().Contains(searchValue) || w.latitude.ToString().Contains(searchValue) ||
            w.ownKuyuName.ToLower().Contains(searchValue.ToLower()) || w.ownSahaName.ToLower().Contains(searchValue.ToLower())
            //toStringler performans açısından kötü olabilir dene eğer kötü olursa çıkar

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
        int recordsTotal = _dataContext.Wellbores.Count(); // Toplam saha sayısı, filtrelenmemiş hali

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
