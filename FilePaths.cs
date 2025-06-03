using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIUG
{
    public static class FilePaths
    {
        private static string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
        private static string dataFolder = System.IO.Path.Combine(projectFolder, "Data");

        public static string abonamenteFilePath = System.IO.Path.Combine(dataFolder, "abonamente.csv");
        public static string angajatiFilePath = System.IO.Path.Combine(dataFolder, "angajati.csv");

    }
}
