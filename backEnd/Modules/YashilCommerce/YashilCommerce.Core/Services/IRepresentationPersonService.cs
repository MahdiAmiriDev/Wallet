using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IRepresentationPersonService : IGenericService<RepresentationPerson,int>
    {
        IQueryable<RepresentationPerson> GetByRepresentationId(int representationId);
        IQueryable<RepresentationPerson> GetByCustomFilter(int? representationId, int? personId, int? postId);
    }
}      
 