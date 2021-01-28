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
        public static TOutPut MapperProperties<TInPut, TOutPut>(TInPut inPut) where TInPut: class where TOutPut : class
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
        /// Mapper Two Class All Properties Into Another Class
        /// </summary>
        /// <typeparam name="TInPut1">TSource 1</typeparam>
        /// <typeparam name="TInPut2">TSource 2</typeparam>
        /// <typeparam name="TOutPut">TDestination</typeparam>
        /// <param name="inPut1">Source 1</param>
        /// <param name="inPut2">Source 2</param>
        /// <returns></returns>
        public static TOutPut MapperTwoClassProperties<TInPut1, TInPut2, TOutPut>(TInPut1 inPut1, TInPut2 inPut2) where TInPut1 : class where TInPut2 : class where TOutPut : class
        {
            TOutPut outPut = default;
            if (inPut1 != null && inPut2 != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TInPut1, TOutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                var tempOutPut = mapper.Map<TOutPut>(inPut1);

                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TInPut2, TOutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                mapper = config.CreateMapper();
                outPut = mapper.Map(inPut2, tempOutPut);
            }
            return outPut;
        }

        /// <summary>
        /// Mapper Source Class Properties Into Destination Class
        /// </summary>
        /// <typeparam name="TInPut">T Source</typeparam>
        /// <typeparam name="TOutPut">T Destination</typeparam>
        /// <param name="inPut">Source</param>
        /// <param name="outPut">Destination</param>
        /// <returns></returns>
        public static TOutPut MapperSourcePropertiesIntoDestination<TInPut, TOutPut>(TInPut inPut, TOutPut outPut) where TInPut : class where TOutPut : class
        {
            if (inPut != null && outPut !=null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TInPut, TOutPut>(MemberList.None);
                });
                config.AssertConfigurationIsValid();//←證驗應對
                var mapper = config.CreateMapper();
                outPut = mapper.Map(inPut, outPut);
            }
            return outPut;
        }

        /// <summary>
        /// Mapper IEnumerable T All Properties IgnoreAllNonExisting
        /// </summary>
        /// <typeparam name="InPut"></typeparam>
        /// <typeparam name="OutPut"></typeparam>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static IEnumerable<TOutPut> MapperProperties<TInPut, TOutPut>(IEnumerable<TInPut> inPut) where TInPut : class where TOutPut : class
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
