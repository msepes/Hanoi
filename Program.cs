using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiTuerme
{
    class Program
    {

        public static string DumpDataTable(DataTable table)
        {
            string data = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn item in table.Columns)
            {
                sb.Append(item.ColumnName);
                sb.Append("    ");
            }

            sb.AppendLine();
            sb.AppendLine();

            if (null != table && null != table.Rows)
            {
                foreach (DataRow dataRow in table.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        sb.Append(item);
                        sb.Append("     ");
                    }
                    sb.AppendLine();
                }

                data = sb.ToString();
            }
            return data;
        }

        static void Main(string[] args)
        {
            Console.Write("Geben Sie Bitte ein Nummer: ");
            int Count = Convert.ToInt32(Console.ReadLine());
            var hanoi = new hanoi(Count);

            Console.Clear();
            // Console.WriteLine(hanoi.GetStringFromA());

            Console.WriteLine(DumpDataTable(hanoi.GetTableview()));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            hanoi.MoveTurmItrativ();
            Console.WriteLine("***************************************");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine(DumpDataTable(hanoi.GetTableview()));


            // Console.WriteLine(hanoi.GetStringFromC());

            Console.WriteLine();
            Console.WriteLine("Bewegungen: " + (Math.Pow(2, Count) - 1));
            Console.ReadKey();
        }


    }
}
