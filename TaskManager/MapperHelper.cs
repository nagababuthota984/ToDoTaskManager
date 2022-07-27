using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class MapperHelper
    {
        public static IMapper Mapper { get; set; }


        public static void CreateMapper()
        {
            Mapper =  SetupConfig().CreateMapper();
        }

        public static MapperConfiguration SetupConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.SqlServer.Task, Models.Task>();
                cfg.CreateMap<Models.Task, Data.SqlServer.Task>();
                cfg.CreateMap<Models.Task, Models.Task>();

            });
        }
    }
}
