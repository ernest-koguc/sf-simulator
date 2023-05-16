namespace SFSimulator.Core
{
    public class TypeMap
    {
        public Type Implementation { get; }
        public Type Interface { get; }
        public TypeMapOption TypeMapOption { get; }

        public TypeMap(Type implementation, Type typeInterface, TypeMapOption typeMapOption = TypeMapOption.None)
        {
            Implementation = implementation;
            Interface = typeInterface;
            TypeMapOption = typeMapOption;
        }

    }
}
