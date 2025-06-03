using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PIUG;

public static class StatsUtilities
{
    public static int getNumarAngajati()
    {
        string path = FilePaths.angajatiFilePath;
        if (!File.Exists(path)) return 0;
        return File.ReadAllLines(path).Length;
    }

    public static int getNumarAbonamentes()
    {
        string path = FilePaths.abonamenteFilePath;
        if (!File.Exists(path)) return 0;
        return File.ReadAllLines(path).Length;
    }

    public static int getNumarAbonamenteLunaCurenta()
    {
        string path = FilePaths.abonamenteFilePath;
        if (!File.Exists(path)) return 0;

        var lines = File.ReadAllLines(path);
        int count = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length != 5) continue;

            if (DateTime.TryParse(parts[3], out DateTime startDate))
            {
                if (startDate.Month == DateTime.Today.Month && startDate.Year == DateTime.Today.Year)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
