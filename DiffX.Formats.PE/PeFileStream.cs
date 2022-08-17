namespace DiffX.Formats.PE;

public class PeFileStream : Stream
{
    Stream _stream;
    long _position;

    PortableExecutable _pe;

    public override bool CanRead => _stream.CanRead;

    public override bool CanSeek => _stream.CanSeek;

    public override bool CanWrite => _stream.CanWrite;

    public override long Length
        => _pe.SectionHeaders.Max(x => x.VirtualAddress + x.VirtualSize);

    public override long Position { get => _position; set => Seek(value, SeekOrigin.Begin); }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var sections = from x in _pe.SectionHeaders
                       orderby x.VirtualAddress
                       where _position >= x.VirtualAddress && (_position + offset) < (x.VirtualAddress + Math.Min(x.VirtualSize, x.SizeOfRawData))
                       select x;

        count = (int)Math.Min(Length, _position + count);
        if (count <= 0) return 0;

        if(sections.Count() == 0)
        {
            // We don't actually end up reading the contents of any section
            _position += count;
            return ReadZeros(buffer, offset, count);
        }

        var totalRead = 0;

        foreach(var section in sections)
        {
            var bytesRead = 0;

            if (_position < section.VirtualAddress)
            {
                // We are currently in a gap between two sections
                bytesRead = ReadZeros(buffer, offset, (int)(sections.First().VirtualAddress - (uint)Position));
                totalRead += bytesRead;
            }

            // Update the base stream position
            Seek(bytesRead, SeekOrigin.Current);

            bytesRead = 0;

            // Read section data
            var sectionSize = Math.Min(section.SizeOfRawData, section.VirtualSize);
            bytesRead += _stream.Read(buffer, offset + bytesRead, (int)Math.Min(count, sectionSize));

            if (section.VirtualSize > section.SizeOfRawData)
            {
                // The section has extra padding in memory
                bytesRead += ReadZeros(buffer, offset + bytesRead, (int)(section.VirtualSize - section.SizeOfRawData));
            }

            _position += bytesRead;
            totalRead += bytesRead;
        }

        if (totalRead < count)
            // We ended up in a gap following the last read section
            ReadZeros(buffer, offset + totalRead, count - totalRead);

        return count;
    }

    int ReadZeros(byte[] buffer, int offset, int count)
    {
        for(var i = offset; i < count; i++)
        {
            buffer[i] = 0;
        }

        return count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        offset = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _position + offset,
            SeekOrigin.End => Length - offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin))
        };

        var section = _pe.SectionHeaders.Where(x => offset >= x.VirtualAddress && offset < x.VirtualAddress + x.SizeOfRawData)
                                       .FirstOrDefault();

        if (section is not null && section.PointerToRawData > 0)
        {
            _stream.Position = offset - section.VirtualAddress + section.PointerToRawData;
        }

        return offset;
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    internal PeFileStream(PortableExecutable pe, Stream stream)
    {
        this._pe = pe;
        this._stream = stream;
    }
}