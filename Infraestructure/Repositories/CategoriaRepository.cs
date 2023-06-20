using Core.Entities;
using Core.Interfaces;

using Infraestructure.Data;

namespace Infraestructure.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(TiendaContext context) : base(context)
        {
        }
    }

}
