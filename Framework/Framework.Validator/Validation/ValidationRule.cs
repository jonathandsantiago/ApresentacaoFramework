using System;

namespace Framework.Validator.Validation
{
	public class ValidationRule<TEntity> : IValidationRule<TEntity>
	{
		private readonly Func<TEntity, bool> _specificationRule;
		public string ErrorMessage { get; private set; }

		public ValidationRule(Func<TEntity, bool> specificationRule, string errorMessage)
		{
			_specificationRule = specificationRule;
			ErrorMessage = errorMessage;
		}

		public bool Valid(TEntity entity)
		{
			return _specificationRule(entity);
		}
	}
}