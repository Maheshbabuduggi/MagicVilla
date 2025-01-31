using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    
    public class VillaNumberController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaService,IMapper mapper,IVillaService villaService1)
        {
            _mapper = mapper;
            _villaNumberService = villaService;
            _villaService = villaService1;
        }
        


        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list=new List<VillaNumberDTO>();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if(response!=null && response.IsSuccess)
            {
                list=JsonConvert.DeserializeObject<List<VillaNumberDTO>>(
                    Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(response.Result)).Select(i=> new SelectListItem
                    {
                        Text = i.Name,
                        Value=i.Id.ToString()
                    });
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.CreateDTO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {

                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var res = await _villaService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model=JsonConvert.DeserializeObject<VillaNumberDTO>(
                    Convert.ToString(response.Result));
               villaNumberVM.VillaNumber=_mapper.Map<VillaNumberUpdateDTO>(model);
            }

          response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {

                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var res = await _villaService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(
                    Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(
                    Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }
            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            
                var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNO);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            
            return View(model);
        }

    }
}
