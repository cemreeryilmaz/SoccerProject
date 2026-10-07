using AutoMapper;
using SoccerProject.WebAPI.DTOs.FixtureDtos;
using SoccerProject.WebAPI.DTOs.MatchEventDtos;
using SoccerProject.WebAPI.DTOs.MatchStaticDtos;
using SoccerProject.WebAPI.DTOs.PlayerCareerDtos;
using SoccerProject.WebAPI.DTOs.PlayerDtos;
using SoccerProject.WebAPI.DTOs.StadiumDtos;
using SoccerProject.WebAPI.DTOs.TeamDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Team, CreateTeamDto>().ReverseMap();
            CreateMap<Team, GetByIdTeamDto>().ReverseMap();
            CreateMap<Team, UpdateTeamDto>().ReverseMap();
            CreateMap<Team, ResultTeamDto>().ReverseMap();

           
            CreateMap<Fixture, CreateFixtureDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.MatchStatus))
                .ReverseMap()
                .ForMember(d => d.MatchStatus, o => o.MapFrom(s => s.Status));
            CreateMap<Fixture, UpdateFixtureDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.MatchStatus))
                .ReverseMap()
                .ForMember(d => d.MatchStatus, o => o.MapFrom(s => s.Status));
            CreateMap<Fixture, GetByIdFixtureDto>().ForMember(d => d.HomeTeamName, o => o.MapFrom(s => s.HomeTeam.TeamName))
                                                   .ForMember(d => d.AwayTeamName, o => o.MapFrom(s => s.AwayTeam.TeamName))
                                                   .ForMember(d => d.HomeLogoUrl, o => o.MapFrom(s => s.HomeTeam.LogoUrl))
                                                   .ForMember(d => d.AwayLogoUrl, o => o.MapFrom(s => s.AwayTeam.LogoUrl))
                                                   .ForMember(d => d.Status, o => o.MapFrom(s => s.MatchStatus));

            CreateMap<MatchEvent,CreateMatchEventDto>().ReverseMap();
            CreateMap<MatchEvent,UpdateMatchEventDto>().ReverseMap();
            CreateMap<MatchEvent, MatchEventDto>()
                .ForMember(d => d.EventType, o => o.MapFrom(s => s.MatchEventType))
                .ReverseMap()
                .ForMember(d => d.MatchEventType, o => o.MapFrom(s => s.EventType));

            CreateMap<MatchStatic,CreateMatchStaticDto>().ReverseMap();
            CreateMap<MatchStatic,UpdateMatchStaticDto>().ReverseMap();
            CreateMap<MatchStatic,MatchStaticDto>().ReverseMap();

            // Stadyum
            CreateMap<Stadium, CreateStadiumDto>().ReverseMap();
            CreateMap<Stadium, UpdateStadiumDto>().ReverseMap();
            CreateMap<Stadium, ResultStadiumDto>().ReverseMap();
            CreateMap<Stadium, GetByIdStadiumDto>().ReverseMap();

            // Futbolcu (ResultPlayerDto.TeamName <- Player.Team.TeamName)
            CreateMap<Player, CreatePlayerDto>().ReverseMap();
            CreateMap<Player, UpdatePlayerDto>().ReverseMap();
            CreateMap<Player, GetByIdPlayerDto>()
                .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team.TeamName))
                .ForMember(d => d.Careers, o => o.MapFrom(s => s.Careers))
                .ReverseMap();
            CreateMap<Player, ResultPlayerDto>()
                .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team.TeamName))
                .ReverseMap();

            // Oyuncu kariyeri
            CreateMap<PlayerCareer, CreatePlayerCareerDto>().ReverseMap();
            CreateMap<PlayerCareer, ResultPlayerCareerDto>().ReverseMap();
        }
    }
}
