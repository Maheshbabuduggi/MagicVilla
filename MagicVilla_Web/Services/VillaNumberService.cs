﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService:IVillaNumberService
    {
        private readonly IBaseService _baseService;
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaNumberService(IHttpClientFactory httpClientFactory,IConfiguration configuration,IBaseService baseService)
        {
            _baseService = baseService;
            _clientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url=villaUrl +$"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/"+id
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/"+id
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + $"/api/{SD.CurrentAPIVersion}/VillaNumberAPI/"+dto.VillaNO
            });
        }
    }
}
