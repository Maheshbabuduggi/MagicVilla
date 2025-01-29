using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService,IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaService;
        }
        


        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list=new List<VillaDTO>();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if(response!=null && response.IsSuccess)
            {
                list=JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(response.Result));
            }
            return View(list);
        }

        // GET: VillaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VillaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VillaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VillaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VillaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VillaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VillaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
