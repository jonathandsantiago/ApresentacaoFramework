namespace Framework.Validator.Validation
{
	public interface IValidation<in TEntity>
    {
        ValidationResult Valid(TEntity entity);
    }
}