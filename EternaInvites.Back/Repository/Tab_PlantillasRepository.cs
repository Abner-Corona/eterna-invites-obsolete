using Database.Tables;
using System.Data;
using System.Threading.Tasks;
using Domain.Exceptions;
using System.Net;

namespace Repository;

public interface ITab_PlantillasRepository : _IBaseRepository<Tab_Plantillas>
{
}

public class Tab_PlantillasRepository : _BaseRepository<Tab_Plantillas>, ITab_PlantillasRepository
{
    public Tab_PlantillasRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }

}
