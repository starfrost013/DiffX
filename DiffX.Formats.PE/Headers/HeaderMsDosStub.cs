using System;

namespace DiffX.Formats.PE
{
    /// <summary>
    /// DiffX Copyright © starfrost / DiffX team. 
    /// Licensed under the MIT License.
    /// 
    /// HeaderMsDosStub
    /// 
    /// MZ header implementation for PE format. This will be moved later when we add MZ/NE compat.
    /// The reason we don't skip over this is some PEs (such as Win98) were multimode, so we need to know where the PE *actually* is, and I intend to handle NEs as well.
    /// </summary>
    public class HeaderMsDosStub
    {
        /// <summary>
        /// The magic determining the file header.
        /// </summary>
        public const string Magic = "MZ";

        /// <summary>
        /// Very early compilers spat this out, this will be needed maybe for MDOS-4?
        /// </summary>
        public const string AlternativeMagic = "ZM";

        /// <summary>
        /// Number of bytes in the last page. 
        /// </summary>
        public ushort ExtraBytes { get; set; }

        /// <summary>
        /// Pages
        /// </summary>
        public ushort Pages { get; set; }

        /// <summary>
        /// Number of entries in the relocation table.
        /// For PE/NE, this doesn't matter.
        /// For MZ, it does.
        /// </summary>
        public ushort RelocationItems { get; set; }

        /// <summary>
        /// The number of paragraphs (16-bytes) taken up by the header.
        /// Nobody cares about the value
        /// </summary>
        public ushort HeaderSize { get; set; }

        /// <summary>
        /// Number of required paragraphs for the program.
        /// This value, multiplied by 16, is the numbr of bytes required for the program.
        /// </summary>
        public ushort MinAllocation { get; set; }

        /// <summary>
        /// Number of requested paragraphs by the program.
        /// This value, multiplied by 16, is the numbr of bytes the program wants.
        /// Should be FFFF in PE
        /// </summary>
        public ushort MaxAllocation { get; set; }

        /// <summary>
        /// Initial stack segment pointer for the application.
        /// </summary>
        public ushort InitialSS { get; set; }
        
        /// <summary>
        /// Initial stack pointer for the application.
        /// </summary>
        public ushort InitialSP { get; set; }

        /// <summary>
        /// Checksum - should be zero when added to all other bytes in file.
        /// Even the DOS 2.0 source code says that this field is ignore
        /// </summary>
        public ushort Checksum { get; set; }

        /// <summary>
        /// The initial IP value of the executable (instruction pointer register, CS+IP is where the code starts executing in DOS...)
        /// </summary>
        public ushort InitialIP { get; set; }

        /// <summary>
        /// The initial CS value of the executable at startup. CS+IP is the entry point
        /// </summary>
        public ushort InitialCS { get; set; }

        /// <summary>
        /// Offset to the relocation table.
        /// For PE this does not matter.
        /// NE it probably does, haven't looked into it
        /// </summary>
        public ushort RelocTableOffset { get; set; }

        /// <summary>
        /// Intended for overlays but not used in practice.
        /// </summary>
        public ushort Overlay { get; set; }

        /// <summary>
        /// Intended for overlays but not used in practice.
        /// </summary>
        public ushort OverlayInfo { get; set; } 

        // ...to do reloc class ... /
        
        /// <summary>
        /// Unused.
        /// Declared in Winnt.h
        /// </summary>
        public ushort OemIdentifier { get; set; }

        /// <summary>
        /// Unused
        /// Declared in Winnt.h
        /// </summary>
        public ushort OemInfo { get; set; }

        /// <summary>
        /// 0x3E: Pointer to PE header
        /// </summary>
        public uint PeHeaderStart { get; set; }

    }
}
