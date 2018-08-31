using Dominio.Models;
using Framework.Data.Nhibernate.Mapeamento;

namespace Mapeamento.Models
{
    public class PaisMap : EntidadeIdMap<int, Pais>
    {
        public PaisMap()
            : base()
        {
            Table("TBGERPAIS");
            MapIndex(c => c.Nome).Not.Nullable();
        }
    }
}