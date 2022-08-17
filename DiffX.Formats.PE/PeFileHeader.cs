using System.Buffers.Binary;

namespace DiffX.Formats.PE;

public class PeFileHeader
{
    public PeMachine Machine { get; init; }
    public ushort NumberOfSections { get; init; }
    public uint TimeDateStamp { get; init; }
    public uint PointerToSymbolTable { get; init; }
    public uint NumberOfSymbols { get; init; }
    public ushort SizeOfOptionalHeader { get; init; }
    public PeCharacteristics Characteristics { get; init; }

    public PeFileHeader(ReadOnlySpan<byte> bytes)
    {
        Machine = (PeMachine)BinaryPrimitives.ReadUInt16LittleEndian(bytes[0..2]);
        NumberOfSections = BinaryPrimitives.ReadUInt16LittleEndian(bytes[2..4]);
        TimeDateStamp = BinaryPrimitives.ReadUInt32LittleEndian(bytes[4..8]);
        PointerToSymbolTable = BinaryPrimitives.ReadUInt32LittleEndian(bytes[8..12]);
        NumberOfSymbols = BinaryPrimitives.ReadUInt32LittleEndian(bytes[12..16]);
        SizeOfOptionalHeader = BinaryPrimitives.ReadUInt16LittleEndian(bytes[16..18]);
        Characteristics = (PeCharacteristics)BinaryPrimitives.ReadUInt16LittleEndian(bytes[18..20]);

        if (Characteristics.HasFlag(PeCharacteristics.BytesReversedHi) ||
            Characteristics.HasFlag(PeCharacteristics.BytesReversedLo))
        {
            throw new NotSupportedException();
        }
    }
}


