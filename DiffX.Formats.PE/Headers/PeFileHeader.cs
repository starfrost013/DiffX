using System.Buffers.Binary;

namespace DiffX.Formats.PE;

/// <summary>
/// PeFileHeader
/// 
/// Defines a PE file header.
/// </summary>
public class PeFileHeader
{
    /// <summary>
    /// The CPU architecture this PE file was compiled for.
    /// 
    /// See <see cref="PeMachine"/>
    /// </summary>
    public PeMachine Machine { get; init; }

    /// <summary>
    /// The number of sections in this PE file.
    /// </summary>
    public ushort NumberOfSections { get; init; }

    /// <summary>
    /// Either the timestamp of compilation,
    /// or a hash of all executable sections before resource is added
    /// </summary>
    public DateTime TimeDateStamp { get; init; }

    /// <summary>
    /// Pointer to symbol table.
    /// TODO: Figure out what is done here
    /// </summary>
    public uint PointerToSymbolTable { get; init; }

    /// <summary>
    /// Number of symbols in the binary.
    /// </summary>
    public uint NumberOfSymbols { get; init; }

    /// <summary>
    /// The size of the optional header.
    /// </summary>
    public ushort SizeOfOptionalHeader { get; init; }

    /// <summary>
    /// The characteristics of this PE.
    /// See <see cref="PeCharacteristics"/>.
    /// </summary>
    public PeCharacteristics Characteristics { get; init; }

    public PeFileHeader(ReadOnlySpan<byte> bytes)
    {
        Machine = (PeMachine)BinaryPrimitives.ReadUInt16LittleEndian(bytes[0..2]);
        NumberOfSections = BinaryPrimitives.ReadUInt16LittleEndian(bytes[2..4]);
        uint timeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..8]);
        TimeDateStamp = new DateTime(1970, 1, 1, 1, 1, 1).AddSeconds(timeDateStamp);
        PointerToSymbolTable = BinaryPrimitives.ReadUInt32LittleEndian(bytes[8..12]);
        NumberOfSymbols = BinaryPrimitives.ReadUInt32LittleEndian(bytes[12..16]);
        SizeOfOptionalHeader = BinaryPrimitives.ReadUInt16LittleEndian(bytes[16..18]);
        Characteristics = (PeCharacteristics)BinaryPrimitives.ReadUInt16LittleEndian(bytes[18..20]);

        if (Characteristics.HasFlag(PeCharacteristics.BytesReversedHi) ||
            Characteristics.HasFlag(PeCharacteristics.BytesReversedLo))
        {
            throw new NotSupportedException("Big endian is not supported at the current time");
        }
    }

    /// <summary>
    /// Converts a PEFileHeader to a string interpretation
    /// </summary>
    /// <returns>A string representing this PEFileHeader</returns>
    public override string ToString()
    {
        return "PE Header Information: \n" +
            $"Architecture: {Machine}\n" +
            $"Number of Sections: {NumberOfSections}\n" +
            $"TimeDateStamp: {TimeDateStamp.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
            $"Symbol Table Location: 0x{PointerToSymbolTable.ToString("X")}\n" +
            $"Number of Symbols {NumberOfSymbols}\n" +
            $"Size of Optional Header: {SizeOfOptionalHeader}\n" +
            $"Characteristics: {Characteristics}\n";
    }
}