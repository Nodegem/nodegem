namespace Nodester.Services.Data.Mappers
{
    public interface IMapper<TPoco, TDto>
    {
        TPoco ToModel(TDto dto);
        TDto ToDto(TPoco poco);
    }
}