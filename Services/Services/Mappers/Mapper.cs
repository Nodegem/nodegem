using Mapster;
using Nodester.Services.Data.Mappers;

namespace Nodester.Services.Mappers
{
    public class Mapper<TModel, TDto> : IMapper<TModel, TDto>
    {
        public TDto ToDto(TModel poco)
        {
            return poco.Adapt<TDto>();
        }

        public TModel ToModel(TDto dto)
        {
            return dto.Adapt<TModel>();
        }
    }
}