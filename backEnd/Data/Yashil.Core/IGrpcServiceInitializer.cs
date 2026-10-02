using Microsoft.AspNetCore.Routing;

namespace Yashil.Core
{
    public interface IGrpcServiceInitializer
    {
        void BindGrpcServices(IEndpointRouteBuilder endpoints);
    }
}
