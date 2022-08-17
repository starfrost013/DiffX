using System.Buffers.Binary;
using System.Text;

namespace DiffX.Formats.PE;

/// <summary>
/// PeExportDirectory
/// 
/// Implements parsing of the PE export directory.
/// </summary>
public class PeExportDirectory : IPeDirectory<PeExportDirectory>
{
    public int Characteristics { get; init; } // todo: make an enum
    public DateTime TimeDateStamp { get; init; }
    public short MajorVersion { get; init; }
    public short MinorVersion { get; init; }
    public int Name { get; init; }
    public int Base { get; init; }
    public int NumberOfFunctions { get; init; }
    public int NumberOfNames { get; init; }
    public int AddressOfFunctions { get; init; }
    public int AddressOfNames { get; init; }
    public int AddressOfNameOrdinals { get; init; }

    public Dictionary<int, (string? Name, int? ExportRva, string? Forwarder)> Exports { get; init; } = new();

    internal PeExportDirectory(PortableExecutable pe, ReadOnlySpan<byte> bytes)
    {
        var directory = pe.OptionalHeader.DataDirectory![DirectoryEntry.Export];

        Characteristics = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
        uint timeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..8]);
        TimeDateStamp = new DateTime(1970, 1, 1, 1, 1, 1).AddSeconds(timeDateStamp);
        MajorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[8..10]);
        MinorVersion = BinaryPrimitives.ReadInt16LittleEndian(bytes[10..12]);
        Name = BinaryPrimitives.ReadInt32LittleEndian(bytes[12..16]);
        Base = BinaryPrimitives.ReadInt32LittleEndian(bytes[16..20]);
        NumberOfFunctions = BinaryPrimitives.ReadInt32LittleEndian(bytes[20..24]);
        NumberOfNames = BinaryPrimitives.ReadInt32LittleEndian(bytes[24..28]);
        AddressOfFunctions = BinaryPrimitives.ReadInt32LittleEndian(bytes[28..32]);
        AddressOfNames = BinaryPrimitives.ReadInt32LittleEndian(bytes[32..36]);
        AddressOfNameOrdinals = BinaryPrimitives.ReadInt32LittleEndian(bytes[36..40]);

        var functionsOffset = (int)(AddressOfFunctions - directory.VirtualAddress);
        var namesOffset = (int)(AddressOfNames - directory.VirtualAddress);
        var ordinalsOffset = (int)(AddressOfNameOrdinals - directory.VirtualAddress);

        string ReadZeroTerminatedString(ReadOnlySpan<byte> bytes)
        {
            var i = 0;
            while(bytes[i] != 0) i++;
            return Encoding.ASCII.GetString(bytes[..i]);
        }
        
        var names = new Dictionary<int, string>();

        for( var i = 0; i < NumberOfNames; i++)
        {
            var ordinal = BinaryPrimitives.ReadInt16LittleEndian(bytes[(ordinalsOffset + i * 2)..]);
            var offset = (int)(BinaryPrimitives.ReadInt32LittleEndian(bytes[(namesOffset + i * 4)..]) - directory.VirtualAddress);
            var name = ReadZeroTerminatedString(bytes[offset..]);

            names.Add(ordinal, name);
        }

        for (var i = 0; i < NumberOfFunctions; i++)
        {
            var rva = (BinaryPrimitives.ReadInt32LittleEndian(bytes[(functionsOffset + i * 4)..]));

            if (rva >= directory.VirtualAddress && rva < (directory.VirtualAddress + directory.Size))
            {
                // Forwarder RVA
                var offset = (int)(rva - directory.VirtualAddress);
                var name = ReadZeroTerminatedString(bytes[offset..]);

                Exports.Add(Base + i, new()
                {
                    Name = names.GetValueOrDefault(i),
                    Forwarder = name
                });
            }
            else
            {
                // Export RVA
                Exports.Add(Base + i, new()
                {
                    Name = names.GetValueOrDefault(i),
                    ExportRva = rva
                });
            }
        }
    }

    static PeExportDirectory IPeDirectory<PeExportDirectory>.Parse(PortableExecutable pe, ReadOnlySpan<byte> bytes)
        => new(pe, bytes);

    public override string ToString()
    {
        string baseStr = "Export Directory:\n\n" +
            $"Characteristics: {Characteristics}\n" +
            $"TimeDateStamp: {TimeDateStamp.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
            $"Version: {MajorVersion}.{MinorVersion}\n" +
            $"Name: {Name}\n" +
            $"Base: {Base}\n" +
            $"Number of Functions: {NumberOfFunctions}\n" +
            $"Number of Names: {NumberOfNames}\n" +
            $"Address of Functions: {AddressOfFunctions}\n" +
            $"Address of Names: {AddressOfNames}\n" +
            $"Address of Name Ordinals: {AddressOfNameOrdinals}\n";

        // add each export
        foreach (KeyValuePair<int, (string? Name, int? ExportRva, string? Forwarder)> export in Exports)
        {
            baseStr = $"{baseStr}\n" +
                $"Export: \n" +
                $"Name: {export.Value.Name} (ordinal {export.Key})\n";
            if (export.Value.ExportRva != null) baseStr = $"{baseStr}ExportRva: {export.Value.ExportRva}\n";
            if (export.Value.Forwarder != null) baseStr = $"{baseStr}Forwarder: {export.Value.Forwarder}\n";
        }

        return baseStr;
    }
}