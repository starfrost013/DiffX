﻿namespace DiffX.Formats.PE;

/// <summary>
/// PeResourceType
/// 
/// Defines PE resource types.
/// </summary>
public enum PeResourceType : short
{
    Cursor = 1,

    Bitmap = 2,

    Icon = 3,

    Menu = 4,

    Dialog = 5,

    String = 6,

    FontDir = 7,

    Font = 8,

    Accelerator = 9,

    RcData = 10,

    MessageTable = 11,

    GroupCursor = 12,

    GroupIcon = 14,

    Version = 16,

    DlgInclude = 17,

    PlugPlay = 19,

    Vxd = 20,

    AnimatedCursor = 21,

    AnimatedIcon = 22,

    Html = 23,

    Manifest = 24
}
