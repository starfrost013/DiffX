using System.Buffers.Binary;
using System.Text;

namespace DiffX.Formats.PE;

/// <summary>
/// ApiSetSection
/// 
/// Defines an API set section.
/// </summary>
public class ApiSetSection
{
    public int Version { get; }
    internal int Size { get; }
    public ApiSetFlags Flags { get; }
    internal int Count { get; }
    internal int EntryOffset { get; }
    internal int HashOffset { get; }
    public int HashFactor { get; }

    public Dictionary<string, List<string>> Values { get; } = new();

    Dictionary<int, int> _hashes = new();

    public ApiSetSection()
    {

    }

    public ApiSetSection(ReadOnlySpan<byte> bytes)
    {
        Version = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        Size = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
        Flags = (ApiSetFlags)BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
        Count = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        EntryOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
        HashOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..24]);
        HashFactor = BinaryPrimitives.ReadInt32LittleEndian(bytes[24..28]);

        for(var i = 0; i < Count; i++)
        {
            var offset = HashOffset + (i * 8);
            var hash = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..]);
            var value = BinaryPrimitives.ReadInt32LittleEndian(bytes[(offset + 4)..]);

            _hashes.Add(hash, value);
        }

        for(var i = 0; i < Count; i++)
        {
            var apiSetNamespace = new ApiSetSchemaNamespace(bytes[(EntryOffset + i * 24)..]);

            var name = Encoding.Unicode.GetString(bytes[apiSetNamespace.NameOffset..(apiSetNamespace.NameOffset+apiSetNamespace.NameLength)]);
            Values.Add(name, new());

            for(var j = 0; j < apiSetNamespace.ValueCount; j++)
            {
                var apiSetValue = new ApiSetSchemaValue(bytes[(apiSetNamespace.ValueOffset + j * 20)..]);
                var value = Encoding.Unicode.GetString(bytes[apiSetValue.ValueOffset..(apiSetValue.ValueOffset+apiSetValue.ValueLength)]);

                Values[name].Add(value);
            }
        }
    }

    public int HashName(string name)
    {
        var hash = 0;

        foreach(var c in name.Remove(name.LastIndexOf('-')).ToLowerInvariant())
        {
            hash *= HashFactor;
            hash += c;
        }

        return hash;
    }

    public static ApiSetSection? FromExecutable(PortableExecutable pe)
    {
        var sections = from x in pe.SectionHeaders where x.Name == ".apiset" select x;
        
        if (sections.FirstOrDefault() is var section and not null)
        {
            using var stream = section.GetStream();
            var buffer = new byte[stream.Length].AsSpan();

            stream.Read(buffer);

            return new ApiSetSection(buffer);
        }

        return null;
    }
}

public class ApiSetSchemaNamespace
{
    public ApiSetFlags Flags { get; }
    internal int NameOffset { get; }
    internal int NameLength { get; }
    internal int HashedLength { get; }
    internal int ValueOffset { get; }
    internal int ValueCount { get; }

    public ApiSetSchemaNamespace(ReadOnlySpan<byte> bytes)
    {
        Flags = (ApiSetFlags)BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        NameOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
        NameLength = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
        HashedLength = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        ValueOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
        ValueCount = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..24]);
    }
}

public class ApiSetSchemaValue
{
    public ApiSetFlags Flags { get; }
    internal int NameOffset { get; }
    internal int NameLength { get; }
    internal int ValueOffset { get; }
    internal int ValueLength { get; }

    public ApiSetSchemaValue(ReadOnlySpan<byte> bytes)
    {
        Flags = (ApiSetFlags)BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        NameOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
        NameLength = BinaryPrimitives.ReadInt32LittleEndian(bytes[8..12]);
        ValueOffset = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        ValueLength = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
    }
}

[Flags]
public enum ApiSetFlags : int
{
    Sealed = 0x1
}