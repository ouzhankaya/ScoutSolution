using Scout.BusinessLayer;
using Scout.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Scout.Web.Models
{
    public class CacheHelper
    {
        public static List<Country> GetCountriesFromCache()
        {
            var result = WebCache.Get("country-cache");

            if(result == null)
            {
                CountryManager countryManager = new CountryManager();
                result = countryManager.List();
                WebCache.Set("category-cache", result, 20 ,true);
            }
            return result;
        }
        public static List<Province> GetProvincesFromCache()
        {
            var result = WebCache.Get("province-cache");

            if (result == null)
            {
                ProvinceManager provinceManager = new ProvinceManager();
                result = provinceManager.List();
                WebCache.Set("province-cache", result, 20, true);
            }
            return result;
        }
        public static List<Foot> GetFootsFromCache()
        {
            var result = WebCache.Get("foot-cache");

            if (result == null)
            {
                FootManager footManager = new FootManager();
                result = footManager.List();
                WebCache.Set("foot-cache", result, 20, true);
            }
            return result;
        }
        public static List<Position> GetPositionsFromCache()
        {
            var result = WebCache.Get("position-cache");

            if (result == null)
            {
                PositionManager positionManager = new PositionManager();
                result = positionManager.List();
                WebCache.Set("position-cache", result, 20, true);
            }
            return result;
        }
        public static List<OtherPosition> GetOtherPositionsFromCache()
        {
            var result = WebCache.Get("otherposition-cache");

            if (result == null)
            {
                OtherPositionManager otherPositionManager = new OtherPositionManager();
                result = otherPositionManager.List();
                WebCache.Set("otherposition-cache", result, 20, true);
            }
            return result;
        }

        public static void RemoveCountriesFromCache()
        {
            Remove("country-cache");
            Remove("porvince-cache");
            Remove("foot-cache");
            Remove("position-cache");
            Remove("otherposition-cache");
        }
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}