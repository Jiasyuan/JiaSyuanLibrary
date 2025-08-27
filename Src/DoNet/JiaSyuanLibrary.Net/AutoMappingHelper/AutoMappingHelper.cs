using AutoMapper;

namespace JiaSyuanLibrary.Net.AutoMappingHelper
{
    public class AutoMappingHelper
    {
        private readonly IMapper _mapper;

        public AutoMappingHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 單一物件映射
        /// </summary>
        public TOut? Map<TIn, TOut>(TIn? source)
            where TIn : class
            where TOut : class
        {
            if (source == null) 
                return default;

            return _mapper.Map<TOut>(source);
        }

        /// <summary>
        /// 集合映射
        /// </summary>
        public IEnumerable<TOut> MapCollection<TIn, TOut>(IEnumerable<TIn> source)
            where TIn : class
            where TOut : class
        {
            if (source== null || !source.Any()) 
                return Enumerable.Empty<TOut>();

            return _mapper.Map<IEnumerable<TOut>>(source);
        }

        /// <summary>
        /// 映射到既有物件
        /// </summary>
        public TOut? MapToExisting<TIn, TOut>(TIn? source, TOut target)
            where TIn : class
            where TOut : class
        {
            if (source == null) 
                return target;

            return _mapper.Map(source, target);
        }
    }

}
