using Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario> {
        Task<Usuario> GetByUserNameAsync(string userName);
        Task<Usuario> GetByRefreshTokenAsync(string refreshToken);
    }

}
