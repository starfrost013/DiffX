using System.Buffers.Binary;

namespace DiffX.Formats.PE;

public class PeImportDirectory : IPeDirectory<PeImportDirectory>
{
    public int OriginalFirstThunk { get; }

    /// <summary>
    /// -1 if bound
    /// </summary>
    public DateTime TimeDateStamp { get; }
    public int ForwarderChain { get; }
    public int Name { get; }
    public int FirstThunk { get; }

    public PeImportDirectory(PortableExecutable pe, ReadOnlySpan<byte> bytes)
    {
        OriginalFirstThunk = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        uint timeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..8]);
        TimeDateStamp = new DateTime(1970, 1, 1, 1, 1, 1).AddSeconds(timeDateStamp);
        ForwarderChain = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
        Name = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        FirstThunk = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
    }

    static PeImportDirectory IPeDirectory<PeImportDirectory>.Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes)
        => new(pe, bytes);

    public override string ToString()
    {
        return $"Import Timestamp: {TimeDateStamp.ToString("yyyy-MM-dd HH:mm:ss")}";
    }
}