using DiffX.Formats.PE;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("PEDiff");
Console.WriteLine("© 2022 DiffX Team - v0.0.0\n"); //two newlines

#region temp
if (args.Length < 1)
{
    Console.WriteLine("Invalid arguments.\nPEDiff [file1] [file2]");
}
else
{
    PortableExecutable pe = new PortableExecutable(args[0]);

    Console.WriteLine(pe.FileHeader.ToString());
    if (pe.ImportDirectory != null) Console.WriteLine(pe.ImportDirectory.ToString());
    if (pe.ExportDirectory != null) Console.WriteLine(pe.ExportDirectory.ToString());
}
#endregion