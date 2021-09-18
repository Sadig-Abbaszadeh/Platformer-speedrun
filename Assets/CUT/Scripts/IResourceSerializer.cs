public interface IResourceSerializer
{
    /// <summary>
    /// To pass relative property paths, use dot for combining.
    /// For ex: to get int a in SomeClass c in the main serialized object, pass "c.a" as argument, and "c" if the whole class instance is to be serialized
    /// </summary>
    string[] SerializableProperties { get; }
}