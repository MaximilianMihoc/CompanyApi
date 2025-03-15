namespace Company.Tests.Data.Builders
{
    internal abstract class BuilderBase<T>
    {
        public abstract T Build();

        public static implicit operator T(BuilderBase<T> builder)
            => builder.Build();
    }
}
