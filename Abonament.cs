using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIUG
{
    public class Abonament
    {
        public int ID { get; set; }
        public string PersonName { get; set; }
        public string Type { get; set; } // Tipurile sunt: "Fitness", "Fitness & Group", "One day"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string ToString()
        {
            return $"[{ID}] {PersonName} - {Type}   [  {StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}  ]";
        }

        public string ToFileLine()
        {
            return $"{ID}|{PersonName}|{Type}|{StartDate:yyyy-MM-dd}|{EndDate:yyyy-MM-dd}";
        }

        public static Abonament FromFileLine(string line)
        {
            var parts = line.Split('|');
            return new Abonament
            {
                ID = int.Parse(parts[0]),
                PersonName = parts[1],
                Type = parts[2],
                StartDate = DateTime.Parse(parts[3]),
                EndDate = DateTime.Parse(parts[4])
            };
        }
    }

}
