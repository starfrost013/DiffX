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

public abstract class PeResourceDirectoryEntry
{
    protected const uint IMAGE_RESOURCE_NAME_IS_STRING = 0x80000000;
    protected const uint IMAGE_RESOURCE_DATA_IS_DIRECTORY = 0x80000000;

    protected uint id, offset;

    public bool NameIsString => (id & IMAGE_RESOURCE_NAME_IS_STRING) > 0;

    public bool DataIsDirectory => (offset & IMAGE_RESOURCE_DATA_IS_DIRECTORY) > 0;
    public int OffsetToData => (int)(offset & ~IMAGE_RESOURCE_DATA_IS_DIRECTORY);

    public PeResourceDirectory? Directory { get; }
    public PeResourceDataEntry? DataEntry { get; }

    public PeResourceDirectoryEntry(ReadOnlySpan<byte> bytes, int entryOffset)
    {
        id = BinaryPrimitives.ReadUInt32LittleEndian(bytes[entryOffset..(entryOffset+4)]);
        offset = BinaryPrimitives.ReadUInt32LittleEndian(bytes[(entryOffset+4)..(entryOffset+8)]);

        if (DataIsDirectory)
        {
            Directory = new PeResourceDirectory(bytes, OffsetToData);
        }
        else
        {
            DataEntry = new PeResourceDataEntry(bytes, OffsetToData);
        }
    }
}

public class PeResourceDirectoryNamedEntry : PeResourceDirectoryEntry
{
    public int NameOffset => (int)(id & ~IMAGE_RESOURCE_NAME_IS_STRING);
    public string Name { get; }

    public PeResourceDirectoryNamedEntry(ReadOnlySpan<byte> bytes, int entryOffset) : base(bytes, entryOffset)
    {
        Debug.Assert(NameIsString);

        var length = BinaryPrimitives.ReadUInt16LittleEndian(bytes[NameOffset..]) * 2;
        Name = Encoding.Unicode.GetString(bytes[(NameOffset+2)..(NameOffset+length+2)]);
    }
}

public class PeResourceDirectoryIdEntry : PeResourceDirectoryEntry
{
    public short Id => (short)id;

    public PeResourceDirectoryIdEntry(ReadOnlySpan<byte> bytes, int entryOffset) : base(bytes, entryOffset)
    {
        Debug.Assert(!NameIsString);
    }
}

public class PeResourceDataEntry
{
    public int OffsetToData { get; }
    public int Size { get; }
    public int CodePage { get; }
    public int Reserved { get; }

    public PeResourceDataEntry(ReadOnlySpan<byte> bytes, int offset)
    {
        OffsetToData = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        Size = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        CodePage = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        Reserved = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
    }
}