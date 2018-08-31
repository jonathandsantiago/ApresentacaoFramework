using Framework.Validator.Validation;

namespace Dominio.Models
{
    public class PaisValidador : Validation<Pais>
    {
        public PaisValidador()
        {
            base.AddRule(new ValidationRule<Pais>(c => string.IsNullOrEmpty(c.Nome), PaisMensagem.NomeObrigatorio));
        }
    }
}