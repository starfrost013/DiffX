using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffX.Formats.PE
{
    /// <summary>
    /// DiffX Copyright © starfrost / DiffX team. 
    /// Licensed under the MIT License.
    /// 
    /// HeaderPeHeader
    /// 
    /// Defines the valid PE architectures. Always assume little-endian.
    /// https://docs.microsoft.com/en-us/windows/win32/debug/pe-format.
    /// Winnt.h version 10.0.22000.1 (22621 is identical)
    /// </summary>
    public enum HeaderPeHeaderArchitecture
    {
        /// <summary>
        /// Unknown processors
        /// </summary>
        Unknown = 0x0,

        /// <summary>
        /// Used for Hyper-V
        /// </summary>
        TargetHost = 0x1,

        /// <summary>
        /// Intel 386+ (IA-32)
        /// </summary>
        I386 = 0x14c,

        /// <summary>
        /// MIPS R3000 
        /// </summary>
        R3000 = 0x162,

        /// <summary>
        /// MIPS R4000 family
        /// </summary>
        R4000 = 0x166,

        /// <summary>
        /// MIPS R10000
        /// </summary>
        R10000 = 0x168,

        /// <summary>
        /// per winnt.h, "MIPS little-endian WCE v2"
        /// </summary>
        WCEMIPSV2 = 0x169,

        /// <summary>
        /// Alpha AXP
        /// </summary>
        ALPHA = 0x184,

        /// <summary>
        /// Hitachi SuperH-3
        /// </summary>
        SH3 = 0x1a2,
        
        /// <summary>
        /// Hitachi SuperH-3-DSP
        /// </summary>
        SH3DSP = 0x1a3,

        /// <summary>
        /// Hitachi SuperH-3E
        /// </summary>
        SH3E = 0x1a4,

        /// <summary>
        /// Hitachi SuperH-4
        /// (CE for Dreamcast?)
        /// </summary>
        SH4 = 0x1a6,

        /// <summary>
        /// Hitachi SuperH-5
        /// (were any CPUs made in this architecture?)
        /// </summary>
        SH5 = 0x1a8,

        /// <summary>
        /// ARM for Windows CE
        /// </summary>
        ARM = 0x1c0,

        /// <summary>
        /// ARM Thumb-1
        /// </summary>
        THUMB = 0x1c2,

        /// <summary>
        /// ARM Thumb-2
        /// Used for NT for ARM-v5/6/7, maybe 4 too
        /// There are extant ARMv6 binaries
        /// </summary>
        ARMNT = 0x1c4,

        /// <summary>
        /// Matsushita AM33
        /// </summary>
        AM33 = 0x1d3,

        /// <summary>
        /// PowerPC
        /// </summary>
        PowerPC = 0x1f0,

        /// <summary>
        /// PowerPC with floating point support
        /// </summary>
        PowerPCFP = 0x1f1,

        /// <summary>
        /// Itanium (IA-64)
        /// </summary>
        IA64 = 0x200,

        /// <summary>
        /// MIPS 16-bit
        /// </summary>
        MIPS16 = 0x266,

        /// <summary>
        /// Axp64
        /// This was used for early testing before amd64 was available
        /// https://docs.microsoft.com/en-us/previous-versions/technet-magazine/cc718978(v=msdn.10)
        /// </summary>
        ALPHA64 = 0x284,

        /// <summary>
        /// MS also define them.
        /// </summary>
        AXP64 = ALPHA64, 

        /// <summary>
        /// MIPS with FPU (probably WinCE only)
        /// </summary>
        MIPSFPU = 0x366,

        /// <summary>
        /// MIPS-16 with FPU (probably WinCE only)
        /// </summary>
        MIPSFPU16 = 0x466,

        /// <summary>
        /// Infineon TriCore
        /// </summary>
        TRICORE = 0x520,

        /// <summary>
        /// AMD64 (0x8664) / K8+
        /// </summary>
        AMD64 = 0x8664,

        /// <summary>
        /// Mitsubishi M32R
        /// </summary>
        M32R = 0x9041,

        /// <summary>
        /// ARMv8+
        /// </summary>
        ARM64 = 0xaa64,

        /// <summary>
        /// CEE? 
        /// </summary>
        CEE = 0xCEE,

        /// <summary>
        /// CEF?
        /// </summary>
        CEF = 0xCEF,

        /// <summary>
        /// EFI Bytecode
        /// </summary>
        EBC = 0xEBC,
    }

}
