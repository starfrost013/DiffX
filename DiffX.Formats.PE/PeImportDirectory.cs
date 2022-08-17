using System.Buffers.Binary;

namespace DiffX.Formats.PE;

public class PeImportDirectory : IPeDirectory<PeImportDirectory>
{
    public int OriginalFirstThunk { get; }
    public int TimeDateStamp { get; }
    public int ForwarderChain { get; }
    public int Name { get; }
    public int FirstThunk { get; }

    public PeImportDirectory(PortableExecutable pe, ReadOnlySpan<byte> bytes)
    {
        OriginalFirstThunk = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        TimeDateStamp = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
        ForwarderChain = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
        Name = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        FirstThunk = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
    }

    static PeImportDirectory IPeDirectory<PeImportDirectory>.Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes)
        => new(pe, bytes);
}