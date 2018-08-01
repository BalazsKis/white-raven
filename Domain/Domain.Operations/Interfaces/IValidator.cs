namespace WhiteRaven.Domain.Operations.Interfaces
{
    public interface IValidator<in T>
    {
        void Validate(T item);
    }
}