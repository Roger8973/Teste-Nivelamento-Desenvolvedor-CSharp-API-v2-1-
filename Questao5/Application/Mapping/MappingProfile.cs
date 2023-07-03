using AutoMapper;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Dtos;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMovementAccountRequest, MovementAccount>();
            CreateMap<CurrentAccount, CurrentAccountDto>()
                     .ForMember(x => x.AccountNumber, s => s.MapFrom(x => x.AccountNumber))
                     .ForMember(x => x.Name, s => s.MapFrom(x => x.Name))
                     .ForMember(x => x.DateHour, s => s.MapFrom(x => DateTime.Now.ToString("dd/MM/yyyy")))
                     .ForMember(x => x.Amount, s => s.MapFrom(x => x.Amount));
                  
                     
        }
    }
}
