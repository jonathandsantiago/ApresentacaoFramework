using Framework.Dominio.Base;

namespace Dominio.Models
{
    public class Pais : EntidadeId<int>
    {
        public virtual string Nome { get; set; }
    }
}
