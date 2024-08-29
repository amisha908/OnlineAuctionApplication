using AutoMapper;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineAuction.BLL.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore()) // Ignore if not setting manually
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Ignore if not mapping manually
                .ForMember(dest => dest.AuctionDuration, opt => opt.MapFrom(src => src.AuctionDuration));

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.AuctionDuration, opt => opt.MapFrom(src => src.AuctionDuration))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)); // Map Guid correctly

            CreateMap<BidDto, Bid>()
                 .ForMember(dest => dest.BidId, opt => opt.MapFrom(src => src.BidId))
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                 .ForMember(dest => dest.BidderId, opt => opt.MapFrom(src => src.BidderId))
                 .ForMember(dest => dest.BidAmount, opt => opt.MapFrom(src => src.BidAmount))
                 .ForMember(dest => dest.BidTime, opt => opt.MapFrom(src => src.BidTime));

            CreateMap<Bid, BidDto>()
                .ForMember(dest => dest.BidId, opt => opt.MapFrom(src => src.BidId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.BidderId, opt => opt.MapFrom(src => src.BidderId))
                .ForMember(dest => dest.BidAmount, opt => opt.MapFrom(src => src.BidAmount))
                .ForMember(dest => dest.BidTime, opt => opt.MapFrom(src => src.BidTime));

        }
    }
}
