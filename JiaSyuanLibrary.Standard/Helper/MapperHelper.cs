using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace JiaSyuanLibrary.Standard.Helper
{
    public class MapperHelper
    {
        /// <summary>
        /// Mapper All Properties IgnoreAllNonExisting
        /// </summary>
        /// <typeparam name="TInPut"></typeparam>
        /// <typeparam name="TOutPut"></typeparam>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static TOutPut MapperProperties<TInPut, TOutPut>(TInPut inPut)
        {
            TOutPut outPut = default;
            if (inPut != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TInPut, TOutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                outPut = mapper.Map<TOutPut>(inPut);
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
        public static IEnumerable<TOutPut> MapperProperties<TInPut, TOutPut>(IEnumerable<TInPut> inPut)
        {
            IEnumerable<TOutPut> outPut = default;
            if (inPut != null && inPut.Any())
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TInPut, TOutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                outPut = mapper.Map<IEnumerable<TOutPut>>(inPut);
            }
            return outPut;
        }
    }
}
