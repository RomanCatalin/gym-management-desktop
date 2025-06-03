using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIUG
{
    using System;
    using System.Globalization;

 
        public class Angajat
        {
            public int ID { get; set; }
            public string Nume { get; set; }
            public string Functie { get; set; }
            public double Salariu { get; set; }

        public static Angajat FromFileLine(string linie)
        {
            var parti = linie.Split(',');
            if (parti.Length != 4)
                throw new FormatException("Format linie invalid");

            return new Angajat
            {
                ID = int.Parse(parti[0]),
                Nume = parti[1],
                Functie = parti[2],
                Salariu = double.Parse(parti[3], CultureInfo.InvariantCulture)
            };
        }


        public string ToFileLine()
            {
                return $"{ID},{Nume},{Functie},{Salariu.ToString(CultureInfo.InvariantCulture)}";
            }

            public override string ToString()
            {
                return $"{ID}: {Nume}  {Functie} ({Salariu} RON)";
            }
        }
    

}
