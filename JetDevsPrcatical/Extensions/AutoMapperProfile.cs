using AutoMapper;
using JetDevsPrcatical.Data.Entity;
using JetDevsPrcatical.Data.Request;

namespace JetDevsPrcatical.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region ViewToModel

            CreateMap<UserRegisterRequest, Users>();

            #endregion
        }
    }
}