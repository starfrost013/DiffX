/// <summary>
/// PeCharacteristics
/// 
/// © 2022 ryuzaki
/// 
/// Enumerates PE characteristics.
/// </summary>
[Flags]
public enum PeCharacteristics : ushort
{

    // Relocation info stripped from file.
    RelocsStripped = 0x0001,

    // File is executable  (i.e. no unresolved external references).
    ExecutableImage = 0x0002,

    // Line nunbers stripped from file.
    LineNumsStripped = 0x0004,

    // Local symbols stripped from file.
    LocalSymsStripped = 0x0008,

    // Aggressively trim working set
    AggressiveWsTrim = 0x0010,

    // App can handle >2gb addresses
    LargeAddressAware = 0x0020,

    // Bytes of machine word are reversed.
    BytesReversedLo = 0x0080,

    // 32 bit word machine.
    Is32BitMachine = 0x0100,

    // Debugging info stripped from file in .DBG file
    DebugStripped = 0x0200,

    // If Image is on removable media, copy and run from the swap file.
    RemovableRunFromSwap = 0x0400,

    // If Image is on Net, copy and run from the swap file.
    NetRunFromSwap = 0x0800,

    // System File.
    System = 0x1000,

    // File is a DLL.
    Dll = 0x2000,

    // File should only be run on a UP machine
    UpSystemOnly = 0x4000,

    // Bytes of machine word are reversed.
    BytesReversedHi = 0x8000,
}
