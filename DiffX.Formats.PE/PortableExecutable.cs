namespace DiffX.Formats.PE;
using System.Text;
public class PortableExecutable
{
    Stream stream;
    public DosExecutableHeader? DosExecutableHeader { get; }
    public PeFileHeader FileHeader { get; }
    public PeOptionalHeader OptionalHeader { get; }
    public PeSectionHeader[] SectionHeaders { get; }
    public PeExportDirectory? ExportDirectory { get; }
    public PeImportDirectory? ImportDirectory { get; }
    public PeResourceDirectory? ResourceDirectory { get; }

    public PortableExecutable(string path)
    {
        stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var buffer = new byte[256].AsSpan();

        // DOS stub header
        stream.Read(buffer[0..64]);

        try
        {
            DosExecutableHeader = new(buffer[0..64]);
        }
        catch(InvalidDataException)
        {
            // This is not a valid MZ header.
            stream.Position = 0;
        }

        if (DosExecutableHeader is not null)
        {
            // PE magic number
            stream.Position = DosExecutableHeader.LfaNew;
            stream.Read(buffer[0..4]);

            var magic = Encoding.ASCII.GetString(buffer[0..4]);
            if (magic != "PE\0\0")
            {
                throw new InvalidDataException();
            }
        }

        // File header
        stream.Read(buffer[0..20]);
        FileHeader = new(buffer[0..20]);

        // Optional header
        stream.Read(buffer[0..FileHeader.SizeOfOptionalHeader]);
        OptionalHeader = new(buffer[0..FileHeader.SizeOfOptionalHeader]);

        // Section headers
        SectionHeaders = new PeSectionHeader[FileHeader.NumberOfSections];
        for(var i = 0; i < FileHeader.NumberOfSections; i++)
        {
            stream.Read(buffer[0..40]);
            SectionHeaders[i] = new(this, buffer[0..40]);
        }

        // Directory tables
        ExportDirectory = LoadDirectory<PeExportDirectory>(DirectoryEntry.Export);
        ImportDirectory = LoadDirectory<PeImportDirectory>(DirectoryEntry.Import);
        ResourceDirectory = LoadDirectory<PeResourceDirectory>(DirectoryEntry.Resource);
    }

    private T? LoadDirectory<T>(DirectoryEntry entry) where T : class, IPeDirectory<T>
    {
        if(OptionalHeader.DataDirectory?.ContainsKey(entry) ?? false)
        {
            var directory = OptionalHeader.DataDirectory[entry];
            var sections = from x in SectionHeaders
                           where directory.VirtualAddress >= x.VirtualAddress && directory.VirtualAddress < (x.VirtualAddress + x.VirtualSize)
                           select x;

            if (sections.FirstOrDefault() is var section and not null)
            {
                var buffer = new byte[Math.Min(section.VirtualSize, section.SizeOfRawData)].AsSpan();
                stream.Position = section.PointerToRawData;
                stream.Read(buffer);

                var offset = (int)(directory.VirtualAddress - section.VirtualAddress);
                return T.Parse(this, buffer[offset..(int)(offset + directory.Size)]);
            }
        }

        return null;
    }

    internal Stream GetFileStream() => stream;
}