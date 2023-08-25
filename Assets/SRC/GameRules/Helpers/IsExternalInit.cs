namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides a marker class to enable the use of the init modifier on properties.
    /// This class is included in .NET 5 and later, but must be manually defined in earlier versions
    /// to use C# 9 features related to property initialization.
    /// </summary>
    class IsExternalInit { }
}