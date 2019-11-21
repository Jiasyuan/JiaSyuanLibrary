using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace JiaSyuanLibrary.Helper
{
    public class MapperHelper
    {
        /// <summary>
        /// Mapper All Properties IgnoreAllNonExisting
        /// </summary>
        /// <typeparam name="InPut"></typeparam>
        /// <typeparam name="OutPut"></typeparam>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static OutPut MapperProperties<InPut, OutPut>(InPut inPut)
        {
            OutPut outPut = default;
            if (inPut != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<InPut, OutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                outPut = mapper.Map<OutPut>(inPut);
            }
            return outPut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="InPut"></typeparam>
        /// <typeparam name="OutPut"></typeparam>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static IEnumerable<OutPut> MapperProperties<InPut, OutPut>(IEnumerable<InPut> inPut)
        {
            IEnumerable<OutPut> outPut = default;
            if (inPut != null && inPut.Count() > 0)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<InPut, OutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                outPut = mapper.Map<IEnumerable<OutPut>>(inPut);
            }
            return outPut;
        }
    }
}
