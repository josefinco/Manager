using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Tests.Configuration
{
    public static class AutoMapperConfiguration
    {

        public static IMapper Configure()
        {
            var autoMapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                .ReverseMap();
            });

            return autoMapperConfiguration.CreateMapper();
        }


    }
}
