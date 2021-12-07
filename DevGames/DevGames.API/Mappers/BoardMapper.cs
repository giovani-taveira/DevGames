using AutoMapper;
using DevGames.API.Entities;
using DevGames.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevGames.API.Mappers
{
    public class BoardMapper : Profile
    {
        public BoardMapper()
        {
            CreateMap<AddBoardInputModel, Board>();

        }
    }
}
