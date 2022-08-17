using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace DiffX.Formats.PE;

public class PeResourceDirectory : IPeDirectory<PeResourceDirectory>
{
    public int Characteristics { get; }
    public int TimeDateStamp { get; }
    public short MajorVersion { get; }
    public short MinorVersion { get; }
    public short NumberOfNamedEntries { get; }
    public short NumberOfIdEntries { get; }

    public Dictionary<string, PeResourceDirectoryNamedEntry> NamedEntries { get; } = new();
    public Dictionary<short, PeResourceDirectoryIdEntry> IdEntries { get; } = new();

    public PeResourceDirectory(ReadOnlySpan<byte> bytes, int offset)
    {
        Characteristics = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        TimeDateStamp = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        MajorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        MinorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        NumberOfNamedEntries = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        NumberOfIdEntries = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);

        for(var i = 0; i < NumberOfNamedEntries; i++)
        {
            var entry = new PeResourceDirectoryNamedEntry(bytes, offset);
            NamedEntries.Add(entry.Name, entry);

            offset += 8;
        }

        for(var i = 0; i < NumberOfIdEntries; i++)
        {
            var entry = new PeResourceDirectoryIdEntry(bytes, offset);
            IdEntries.Add(entry.Id, entry);

            offset += 8;
        }
    }

    static PeResourceDirectory IPeDirectory<PeResourceDirectory>.Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes)
        => new(bytes, 0);
}

