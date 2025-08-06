using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JiaSyuanLibrary.Helper.AutoMapping
{
    public static class AutoMappingHelper
    {
        public static TOut Map<TIn, TOut>(TIn source, string profileName = "default")
            where TIn : class
            where TOut : class
        {
            if (source == null) return default;

            Action<IMapperConfigurationExpression> configAction = delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<TIn, TOut>(MemberList.None);
                var profile = MappingProfileRegistry.Get(profileName);
                if (profile != null) profile(cfg);
            };

            var mapper = MapperFactory.GetMapper(configAction, profileName);
            return mapper.Map<TOut>(source);
        }

        public static IEnumerable<TOut> MapCollection<TIn, TOut>(IEnumerable<TIn> source, string profileName = "default")
            where TIn : class
            where TOut : class
        {
            if (source == null || !source.Any()) return Enumerable.Empty<TOut>();

            Action<IMapperConfigurationExpression> configAction = delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<TIn, TOut>(MemberList.None);
                var profile = MappingProfileRegistry.Get(profileName);
                if (profile != null) profile(cfg);
            };

            var mapper = MapperFactory.GetMapper(configAction, profileName);
            return mapper.Map<IEnumerable<TOut>>(source);
        }

        public static TOut MapToExisting<TIn, TOut>(TIn source, TOut target, string profileName = "default")
            where TIn : class
            where TOut : class
        {
            if (source == null || target == null) return target;

            Action<IMapperConfigurationExpression> configAction = delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<TIn, TOut>(MemberList.None);
                var profile = MappingProfileRegistry.Get(profileName);
                if (profile != null) profile(cfg);
            };

            var mapper = MapperFactory.GetMapper(configAction, profileName);
            return mapper.Map(source, target);
        }

        public static TOut MapFromTwoSources<TIn1, TIn2, TOut>(
            TIn1 source1, TIn2 source2,
            string profile1 = "default", string profile2 = "default")
            where TIn1 : class
            where TIn2 : class
            where TOut : class, new()
        {
            if (source1 == null || source2 == null) return default;

            var target = new TOut();
            target = MapToExisting(source1, target, profile1);
            target = MapToExisting(source2, target, profile2);
            return target;
        }
    }


}


