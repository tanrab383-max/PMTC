using TNK.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Services.Configuration
{
    public interface IConfigService
    {
        List<TNK.Core.Domain.Config> GetConfigList();
        string GetConfigByCode(string code);

    }
}
