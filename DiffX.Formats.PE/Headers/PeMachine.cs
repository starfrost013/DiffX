using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffX.Formats.PE;

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
public enum PeMachine : ushort
{
    Unknown = 0,

    /// <summary>
    /// Useful for indicating we want to interact with the host and not a WoW guest.
    /// Used for Hyper-V.
    /// </summary>
    TargetHost = 0x0001,

    /// <summary>
    /// Intel 386.
    /// </summary>
    I386 = 0x014c,

    /// <summary>
    /// MIPS big-endian
    /// </summary>
    R3000BigEndian = 0x160,

    /// <summary>
    /// MIPS little-endian
    /// </summary>
    R3000 = 0x0162,

    /// <summary>
    /// MIPS little-endian
    /// </summary>
    R4000 = 0x0166,

    /// <summary>
    /// MIPS little-endian
    /// </summary>
    R10000 = 0x0168,

    /// <summary>
    /// MIPS little-endian WCE v2
    /// </summary>
    WCeMipsV2 = 0x0169,

    /// <summary>
    /// Alpha_AXP
    /// </summary>
    Alpha = 0x0184,

    /// <summary>
    /// SH3 little-endian
    /// </summary>
    Sh3 = 0x01a2,

    /// <summary>
    /// SH3 little-endian with DSP support
    /// </summary>
    Sh3Dsp = 0x01a3,

    /// <summary>
    /// SH3E little-endian
    /// </summary>
    Sh3E = 0x01a4,

    /// <summary>
    /// SH4 little-endian
    /// </summary>
    Sh4 = 0x01a6,

    /// <summary>
    /// SH5
    /// </summary>
    Sh5 = 0x01a8,

    /// <summary>
    /// ARM Little-Endian
    /// </summary>
    Arm = 0x01c0,

    /// <summary>
    /// ARM Thumb/Thumb-2 Little-Endian
    /// </summary>
    Thumb = 0x01c2,

    /// <summary>
    /// ARM Thumb-2 Little-Endian
    /// </summary>
    ArmNt = 0x01c4,

    /// <summary>
    /// Matsushita AM33
    /// </summary>
    Am33 = 0x01d3,

    /// <summary>
    /// IBM PowerPC Little-Endian
    /// </summary>
    PowerPc = 0x01F0,

    /// <summary>
    /// IBM PowerPC Little-Endian with Floating Point support
    /// </summary>
    PowerPcFp = 0x01f1,

    /// <summary>
    /// Itanium
    /// </summary>
    Ia64 = 0x0200,

    /// <summary>
    /// MIPS
    /// </summary>
    Mips16 = 0x0266,

    /// <summary>
    /// ALPHA64
    /// Internal port for AMD64 prep work, year 2000
    /// </summary>
    Alpha64 = 0x0284,

    /// <summary>
    /// MIPS with FPU support
    /// </summary>
    MipsFpu = 0x0366,

    /// <summary>
    /// MIPS-16 with FPU support
    /// </summary>
    MipsFpu16 = 0x0466,

    /// <summary>
    /// See <see cref="Alpha64"/>.
    /// </summary>
    Axp64 = Alpha64,

    /// <summary>
    /// Infineon
    /// </summary>
    Tricore = 0x0520,

    /// <summary>
    /// 0xCEF
    /// </summary>
    Cef = 0x0CEF,

    /// <summary>
    /// EFI Byte Code
    /// </summary>
    Ebc = 0x0EBC,

    /// <summary>
    /// AMD64 (K8)
    /// </summary>
    Amd64 = 0x8664,

    /// <summary>
    /// M32R little-endian
    /// </summary>
    M32R = 0x9041,

    /// <summary>
    /// ARM64 Little-Endian (ARMv8+)
    /// </summary>
    Arm64 = 0xAA64,

    /// <summary>
    /// 0xCEE
    /// </summary>
    Cee = 0xC0EE,
}