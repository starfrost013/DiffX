namespace DiffX.Formats.PE;

public interface IPeDirectory<T> where T : IPeDirectory<T>
{
    static abstract T Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes);
}