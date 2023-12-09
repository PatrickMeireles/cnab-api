namespace Cnab.Domain.Builder
{
    public abstract class Builder
    {
        public ICollection<string> Errors { get; protected set; } = new List<string>();

        public bool IsValid() =>
            !Errors.Any();

        protected void AddError(string message) =>
            Errors.Add(message);

        protected void AddDefaultPropertyError(string property)
        {
            Errors.Add($"The property {property} is invalid.");
        }
    }
}
