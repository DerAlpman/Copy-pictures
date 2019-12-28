using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CopyPictures
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("     Syntax :: CopyPictures Quelle Ziel [Dateiformat [Dateiformat]...] [Optionen]");
                Console.WriteLine("     Quelle :: Quellverzeichnis (Laufwerk:\\Pfad");
                Console.WriteLine("       Ziel :: Zielverzeichnis (Laufwerk:\\Pfad");
                Console.WriteLine("Dateiformat :: z.B. *.JPG, mehrere durch ',' getrennt, keine Leerzeichen.");
                Console.WriteLine("\nOptionen:");
                Console.WriteLine("/y oder /m oder /d :: Je nach Auswahl werden die Dateien im Zielverzeichnis sortiert:");
                Console.WriteLine("                /y :: nach Jahr");
                Console.WriteLine("                /m :: nach Jahr\\Monat");
                Console.WriteLine("                /d :: nach Jahr\\Monat\\Tag");
                Console.WriteLine("\nBeispiel:");
                Console.WriteLine("\nG:\\DCIM F:\\Digi-Fotos\\Emelie *.JPG,*.MOV,*.NEF [/y/m/d]");
                return;
            }

            string[] splitCharsFileExtensions = new string[] { "," };

            DirectoryInfo dInfo = new DirectoryInfo(args[0]);
            string[] searchPatterns = args[2].Split(splitCharsFileExtensions, StringSplitOptions.RemoveEmptyEntries);

            string pathFlag = "";
            if (args.Length == 4)
                pathFlag = args[3];

            int counter = 0;

            foreach (FileInfo fInfo in searchPatterns.SelectMany(searchPattern => dInfo.EnumerateFiles(searchPattern, SearchOption.AllDirectories)))
            {
                try
                {
                    if ((counter % 100) == 0)
                        Console.WriteLine(counter);

                    DateTime parsedDate;
                    IMediaCopier mediaCopier = MediaCopierFactory.CreateMediaCopier(fInfo.Extension.ToUpper(), fInfo);

                    if (mediaCopier == null)
                    {
                        Console.WriteLine(String.Format("{0}: Für den Dateityp {1} ist kein Kopierer vorhanden.",
                            fInfo.Name,
                            fInfo.Extension));
                        continue;
                    }

                    parsedDate = mediaCopier.GetDate();

                    string year = parsedDate.Year.ToString();
                    string month = parsedDate.ToString("MM");
                    string day = parsedDate.ToString("dd");
                    string hour = parsedDate.Hour.ToString();
                    string minutes = parsedDate.Minute.ToString();

                    string fDate = year + month + day + hour + minutes;
                    StringBuilder targetDir = new StringBuilder(args[1]);

                    targetDir.Append("\\");
                    targetDir.Append(year);
                    targetDir.Append("\\");
                    targetDir.Append(month);
                    targetDir.Append("\\");
                    targetDir.Append(day);

                    int removals = 0;
                    switch (pathFlag)
                    {
                        case "/m":
                            removals = 3;
                            break;
                        case "/y":
                            removals = 6;
                            break;
                        case "":
                            removals = 11;
                            break;
                    }

                    targetDir = targetDir.Remove(targetDir.Length - removals, removals);

                    string[] splitCharsFileName = new string[] { "." };
                    string[] fileNameParts = fInfo.Name.Split(splitCharsFileName, StringSplitOptions.RemoveEmptyEntries);

                    string newName = String.Join(
                        ".",
                        fileNameParts.FirstOrDefault<string>(),
                        fDate,
                        fileNameParts.LastOrDefault<string>());

                    string targetFile = targetDir + "\\" + newName;

                    mediaCopier.CopyMedia(targetDir.ToString(), targetFile);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    Console.WriteLine(exc.InnerException);
                }

                counter += 1;
            }
            Console.WriteLine("Fertig");
            Console.ReadKey();
        }
    }
}
