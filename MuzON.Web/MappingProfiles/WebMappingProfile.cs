﻿using AutoMapper;
using MuzON.BLL.DTO;
using MuzON.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzON.Web.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
           
            CreateMap<ArtistDTO, ArtistViewModel>(MemberList.None)
                //.ForMember(f=>f.SelectedBands, 
                //opt=>opt.MapFrom(src=>src.Bands.ToList().ForEach()))
                .ForMember(f=>f.Image,opt=>opt.MapFrom(src=>Convert.ToBase64String(src.Image)))
                .ForMember(f=>f.BirthDate, opt=>opt.MapFrom(src=>DateTime.Parse(src.BirthDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<ArtistDTO, ArtistIndexViewModel>(MemberList.None)
               .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
               .ForMember(f => f.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<ArtistIndexDTO, ArtistIndexViewModel>(MemberList.None)
               .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
               .ForMember(f => f.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<ArtistDTO, ArtistDetailsViewModel>(MemberList.None)
                .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
                .ForMember(f => f.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<ArtistDetailsDTO, ArtistDetailsViewModel>(MemberList.None)
               .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
               .ForMember(f => f.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate.ToString("yyyy-MM-dd")))).ReverseMap();

            
            CreateMap<BandIndexDTO, BandIndexViewModel>(MemberList.None)
                .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
                .ForMember(f => f.CreatedDate, opt => opt.MapFrom(src => DateTime.Parse(src.CreatedDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<BandDTO, BandDetailsViewModel>(MemberList.None)
               .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
               .ForMember(f => f.CreatedDate, opt => opt.MapFrom(src => DateTime.Parse(src.CreatedDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<BandDTO, BandViewModel>(MemberList.None)
               .ForMember(f => f.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
               .ForMember(f => f.CreatedDate, opt => opt.MapFrom(src => DateTime.Parse(src.CreatedDate.ToString("yyyy-MM-dd")))).ReverseMap();

            CreateMap<CountryDTO, CountryViewModel>(MemberList.None).ReverseMap();

            CreateMap<SongDTO, SongViewModel>()
                .ForMember(trg => trg.Artists, opt => opt.MapFrom(x => x.Artists.Where(f => f.Selected)))
                .ForMember(trg => trg.Bands, opt => opt.MapFrom(x => x.Bands.Where(f => f.Selected)))
                .ForMember(trg => trg.SelectedArtists, f => f.Ignore())
                .ForMember(trg => trg.SelectedBands, f => f.Ignore()).ReverseMap();
            
           
            CreateMap<BandSongDTO, BandSongViewModel>(MemberList.None).ReverseMap();

            CreateMap<UserDTO, LoginViewModel>(MemberList.None).ReverseMap();
        }
    }
}