using AutoMapper;

namespace TaskManager
{
    public class MapperBootstrapper
    {
        public static IMapper Mapper { get; set; }


        public static void CreateMapper()
        {
            Mapper = SetupConfig().CreateMapper();
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
