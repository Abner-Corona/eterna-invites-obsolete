using Database.Tables;
using System.Data;
using System.Threading.Tasks;
using Domain.Exceptions;
using System.Net;

namespace Repository;

public interface ITab_ClientesRepository : _IBaseRepository<Tab_Clientes>
{
}

public class Tab_ClientesRepository : _BaseRepository<Tab_Clientes>, ITab_ClientesRepository
{
    public Tab_ClientesRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }

}
