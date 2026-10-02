using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;

namespace Yashil.Core
{
    public abstract class YashilViewComponent : ViewComponent
    {
        protected readonly ApplicationConfiguration ApplicationConfiguration;
        protected readonly IMapper Mapper;

        public YashilViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper)
        {
            ApplicationConfiguration = applicationConfiguration;
            Mapper = mapper;
        }
    }
}
