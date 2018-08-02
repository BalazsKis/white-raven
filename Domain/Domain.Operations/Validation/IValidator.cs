namespace WhiteRaven.Domain.Operations.Validation
{
    public interface IValidator<in T>
    {
        void Validate(T item);
    }
}