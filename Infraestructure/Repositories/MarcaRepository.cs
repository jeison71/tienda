using Core.Entities;
using Core.Interfaces;

using Infraestructure.Data;

namespace Infraestructure.Repositories
{
    public class MarcaRepository : GenericRepository<Marca>, IMarcaRepository
    {
        public MarcaRepository(TiendaContext context) : base(context)
        {
        }
    }

}
