using TNK.Core;
using TNK.Core.Caching;
using TNK.Core.Data;
using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TNK.Services.LichSuThaoTac;
using TNK.Services.Authentication;

namespace TNK.Services.Configuration
{
    public class ConfigService : IConfigService
    {
        IRepository<TNK.Core.Domain.Config> _configRepository;
        ICacheManager _cacheManager;
        public ConfigService(IRepository<TNK.Core.Domain.Config> _configRepository
            , ICacheManager _cacheManager)
        {
            this._configRepository = _configRepository;
            this._cacheManager = _cacheManager;
        }
       
        public string GetConfigByCode(string configCode)
        {
            Config obj = this._configRepository.Table.FirstOrDefault(x => x.ConfigCode == configCode);
            if (obj == null)
                return "";
            return obj.ConfigValue;
        }

        public List<TNK.Core.Domain.Config> GetConfigList()
        {
            return _cacheManager.Get(CacheKey.Category.keyCategoryItem, () => _configRepository.Table.ToList());
        }


    }
}
