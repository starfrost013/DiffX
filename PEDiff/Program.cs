using DiffX.Formats.PE;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("PEDiff");
Console.WriteLine("© 2022 DiffX Team - v0.0.0\n"); //two newlines

#region temp
if (args.Length < 2)
{
    Console.WriteLine("Invalid arguments.\nPEDiff [file1] [file2]");
}
else
{
    PortableExecutable pe = new PortableExecutable(args[1]);

    Console.WriteLine(pe.FileHeader.ToString());
}
#endregion