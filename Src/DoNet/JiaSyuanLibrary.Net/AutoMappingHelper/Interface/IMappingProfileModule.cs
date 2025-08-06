using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaSyuanLibrary.Net.AutoMappingHelper.Interface
{
    public interface IMappingProfileModule
    {
        void Register(IProfileRegistry registry);
    }
}
