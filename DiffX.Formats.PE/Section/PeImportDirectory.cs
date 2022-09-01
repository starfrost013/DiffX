using System.Buffers.Binary;

namespace DiffX.Formats.PE;

/// <summary>
/// Defines a PE import directory for a single file.
/// </summary>
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

    /// <summary>
    /// Dictionary for PE Imports.
    /// Key = module
    /// </summary>
    public Dictionary<string, (string FunctionName, int Ordinal, int Hint, bool Delayed)> Imports { get; set; }
    
    public PeImportDirectory(PortableExecutable pe, ReadOnlySpan<byte> bytes)
    {
        OriginalFirstThunk = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        uint timeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..8]);
        TimeDateStamp = DateTime.UnixEpoch.AddSeconds(timeDateStamp);
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