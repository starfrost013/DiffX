using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace DiffX.Formats.PE;

/// <summary>
/// PeResourceDirectory
/// 
/// Defines a PE resource directory.
/// </summary>
public class PeResourceDirectory : IPeDirectory<PeResourceDirectory>
{
    public int Characteristics { get; } // todo: make enum
    public DateTime TimeDateStamp { get; }
    public short MajorVersion { get; }
    public short MinorVersion { get; }
    public short NumberOfNamedEntries { get; }
    public short NumberOfIdEntries { get; }

    public Dictionary<string, PeResourceDirectoryNamedEntry> NamedEntries { get; } = new();
    public Dictionary<short, PeResourceDirectoryIdEntry> IdEntries { get; } = new();

    public PeResourceDirectory(ReadOnlySpan<byte> bytes, int offset)
    {
        Characteristics = BinaryPrimitives.ReadInt32LittleEndian(bytes[offset..(offset+=4)]);
        uint timeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[offset..(offset+=4)]);
        TimeDateStamp = DateTime.UnixEpoch.AddSeconds(timeDateStamp);
        MajorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        MinorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        NumberOfNamedEntries = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);
        NumberOfIdEntries = BinaryPrimitives.ReadInt16LittleEndian(bytes[offset..(offset+=2)]);

        for (var i = 0; i < NumberOfNamedEntries; i++)
        {
            var entry = new PeResourceDirectoryNamedEntry(bytes, offset);
            NamedEntries.Add(entry.Name, entry);

            offset += 8;
        }

        for (var i = 0; i < NumberOfIdEntries; i++)
        {
            var entry = new PeResourceDirectoryIdEntry(bytes, offset);
            IdEntries.Add(entry.Id, entry);

            offset += 8;
        }
    }

    static PeResourceDirectory IPeDirectory<PeResourceDirectory>.Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes)
        => new(bytes, 0);

    public override string ToString()
    {
        string baseStr = $"Resource Directory:\n" +
            $"Characteristics: {Characteristics}\n" +
            $"TimeDateStamp: {TimeDateStamp.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
            $"Version: {MajorVersion}.{MinorVersion}\n" +
            $"Number of Named Entries: {NumberOfNamedEntries}\n" +
            $"Number of Id Entries: {NumberOfIdEntries}\n";

        foreach (KeyValuePair<string, PeResourceDirectoryNamedEntry> namedEntry in NamedEntries)
        {
            baseStr = $"{baseStr}\n" +
                $"Named Entry ({namedEntry.Key}):\n\n" +
                $"Data Offset: {namedEntry.Value.OffsetToData}\n" +
                $"Data is Directory: {namedEntry.Value.DataIsDirectory}\n" +
                $"Name is String: {namedEntry.Value.NameIsString}\n" +
                $"Name: {namedEntry.Value.Name}\n";

            if (namedEntry.Value.DataIsDirectory
                && namedEntry.Value.Directory != null)
            {
                foreach (KeyValuePair<string, PeResourceDirectoryNamedEntry> langEntry in namedEntry.Value.Directory.NamedEntries)
                {
                    baseStr = $"{baseStr}\n" +
                        $"Named Entry ({langEntry.Key}):\n\n" +
                        $"Data Offset: {langEntry.Value.OffsetToData}\n" +
                        $"Data is Directory: {langEntry.Value.DataIsDirectory}\n" +
                        $"Name is String: {langEntry.Value.NameIsString}\n" +
                        $"Name: {langEntry.Value.Name}\n";
                }
            }
        }

        foreach (KeyValuePair<short, PeResourceDirectoryIdEntry> idEntry in IdEntries)
        {
            baseStr = $"{baseStr}\n" +
                $"ID Entry ({idEntry.Key}):\n\n" +
                $"Data Offset: {idEntry.Value.OffsetToData}\n" +
                $"Data is Directory: {idEntry.Value.DataIsDirectory}\n" +
                $"Name is String: {idEntry.Value.NameIsString}\n" +
                $"ID: {idEntry.Value.Id} ({(PeResourceType)idEntry.Value.Id})\n";


            if (idEntry.Value.DataIsDirectory
                && idEntry.Value.Directory != null)
            {
                foreach (KeyValuePair<short, PeResourceDirectoryIdEntry> langEntry in idEntry.Value.Directory.IdEntries)
                {
                    baseStr = $"{baseStr}\n" +
                        $"Named Entry ({langEntry.Key}):\n\n" +
                        $"Data Offset: {langEntry.Value.OffsetToData}\n" +
                        $"Data is Directory: {langEntry.Value.DataIsDirectory}\n" +
                        $"Name is String: {langEntry.Value.NameIsString}\n";
                }
            }
        }

        return baseStr;
    }
}

