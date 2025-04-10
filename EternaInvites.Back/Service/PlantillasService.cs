using Database.Tables;
using Repository;

namespace Service;

public interface IPlantillasService : _IBaseService<PlantillasModel, Tab_Plantillas>
{
}

public class PlantillasModel
{
    public int Id { get; set; }
    public Guid Uui { get; set; } = Guid.NewGuid();
    #region properties
    public string Nombre { get; set; } = string.Empty;

    public bool MostrarDemo { get; set; } = false;
    public string Html { get; set; } = string.Empty;

    public List<ComponenteHTML> Componentes { get; set; }

    // Category or type of template
    public string Categoria { get; set; } = string.Empty;

    #endregion
}

public class PlantillasService : _BaseService<PlantillasModel, Tab_Plantillas>, IPlantillasService
{
    public PlantillasService(ITab_PlantillasRepository repository) : base(repository)
    {
    }
}