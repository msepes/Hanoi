using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
namespace HanoiTuerme
{
    class hanoi
    {
        public Stack<int> A;
        public Stack<int> B;
        public Stack<int> C;
        private int ScheibenAnzahl = 0;
        int Counter = 0;

        public hanoi(int Count)
        {
            A = new Stack<int>();
            B = new Stack<int>();
            C = new Stack<int>();
            ScheibenAnzahl = Count;
            for (int i = Count; i > 0; i--)
            {
                A.Push(i);
            }

        }


        private Stack<int> GetStackWhoHasTheSmallestItem(Stack<int> A, Stack<int> B, Stack<int> C, int ExcludedItem)
        {

            if (A.Count == 0 && B.Count == 0 && C.Count == 0)
                throw new Exception("Keine Elemente Vorhanden!");

            int PeekedItemFromA = int.MaxValue;
            int PeekedItemFromB = int.MaxValue;
            int PeekedItemFromC = int.MaxValue;

            if (A.Count > 0)
                PeekedItemFromA = A.Peek();
            if (B.Count > 0)
                PeekedItemFromB = B.Peek();
            if (C.Count > 0)
                PeekedItemFromC = C.Peek();

            if ((PeekedItemFromA < PeekedItemFromB || PeekedItemFromB == ExcludedItem) && (PeekedItemFromA < PeekedItemFromC || PeekedItemFromC == ExcludedItem) && PeekedItemFromA != ExcludedItem)
                return A;

            if ((PeekedItemFromB < PeekedItemFromA || PeekedItemFromA == ExcludedItem) && (PeekedItemFromB < PeekedItemFromC || PeekedItemFromC == ExcludedItem) && PeekedItemFromB != ExcludedItem)
                return B;


            return C;


        }

        private Stack<int> GetNextMoveSource(Stack<int> A, Stack<int> B, Stack<int> C, int ExcludedItem)
        {
            return GetStackWhoHasTheSmallestItem(A, B, C, ExcludedItem);
        }

        private bool CheckIfItIsTheStack(Stack<int> Source, Stack<int> S, int PeekedItemFromSource, bool CheckEmpty = true)
        {

            if (S == Source)
                return false;

            bool PrefereNoEmpty = false;
            if (CheckEmpty)
            {

                List<bool> Check = new List<bool>();
                Check.Add(CheckIfItIsTheStack(Source, A, PeekedItemFromSource, false));
                Check.Add(CheckIfItIsTheStack(Source, B, PeekedItemFromSource, false));
                Check.Add(CheckIfItIsTheStack(Source, C, PeekedItemFromSource, false));
                PrefereNoEmpty = Check.Count(e => e) == 2;
            }


            int peekedFromS = 0;
            if (S.Count > 0)
                peekedFromS = S.Peek();

            if ((PeekedItemFromSource % 2 != peekedFromS % 2 || (peekedFromS == 0 && !PrefereNoEmpty)) && (peekedFromS > PeekedItemFromSource || (peekedFromS == 0 && !PrefereNoEmpty)))
            {
                return true;
            }

            return false;

        }

        private Stack<int> GetNextMoveDestination(Stack<int> A, Stack<int> B, Stack<int> C, Stack<int> Source)
        {
            int PeekedItemFromSource = Source.Peek();



            if (CheckIfItIsTheStack(Source, C, PeekedItemFromSource))
                return C;


            if (CheckIfItIsTheStack(Source, A, PeekedItemFromSource))
                return A;

            if (CheckIfItIsTheStack(Source, B, PeekedItemFromSource))
                return B;


            return C;
        }

        public void MoveTurmItrativ()
        {
            Double Maxi = Math.Pow(2, ScheibenAnzahl);
            int PreSmalllestItem = A.Peek();
            if (ScheibenAnzahl % 2 != 0)
            {

                this.C.Push(this.A.Pop());
            }
            else
            {
                this.B.Push(this.A.Pop());
            }


            for (int i = 2; i < Maxi; i++)
            {

                Debug.WriteLine(i / Maxi * 100 + "%");
                if (this.C.Count == ScheibenAnzahl)
                    return;

                Stack<int> Source = this.GetNextMoveSource(this.A, this.B, this.C, PreSmalllestItem);
                Stack<int> Destination = this.GetNextMoveDestination(this.A, this.B, this.C, Source);

                PreSmalllestItem = Source.Peek();
                Destination.Push(Source.Pop());

            }

        }

        public void MoveTurmRekursiv()
        {
            MoveTurm(A.Count, A, B, C);
        }

        public void MoveTurm(int Count, Stack<int> Source, Stack<int> Helper, Stack<int> Destination)
        {

            if (Count <= 0)
                return;

            //Move n-1 from Source To Helper 
            this.MoveTurm(Count - 1, Source, Destination, Helper);



            //ActualMove Here 
            Destination.Push(Source.Pop());

            //Zusätzliches Code nur zum Testen ;)
            Counter++;
            Double Maxi = Math.Pow(2, ScheibenAnzahl);
            Debug.WriteLine(Math.Round((Counter / Maxi) * 100, 4) + "%");
            Debug.Assert(Counter < Maxi, "Fehler Maximal Bewegungen erreicht!");



            //Move n-1 From Helper To Target 
            this.MoveTurm(Count - 1, Helper, Source, Destination);
        }








        public DataTable GetTableview()
        {
            DataTable table = new DataTable();
            table.Columns.Add("A", typeof(int));
            table.Columns.Add("B", typeof(int));
            table.Columns.Add("C", typeof(int));
            var ACopy = A.ToList();
            var BCopy = B.ToList();
            var CCopy = C.ToList();

            for (int i = 1; i <= ScheibenAnzahl; i++)
            {
                int? ValueFromA = null;
                int? ValueFromB = null;
                int? ValueFromC = null;

                if (ACopy.Count() > 0)
                {
                    ValueFromA = ACopy.First();
                    ACopy.Remove(ACopy.First());
                }

                if (BCopy.Count() > 0)
                {
                    ValueFromB = BCopy.First();
                    BCopy.Remove(BCopy.First());
                }

                if (CCopy.Count() > 0)
                {
                    ValueFromC = CCopy.First();
                    CCopy.Remove(CCopy.First());
                }

                table.Rows.Add(ValueFromA, ValueFromB, ValueFromC);
            }
            return table;
        }


        public string GetStringFromA()
        {
            return GetStringFromStack(A);
        }

        public string GetStringFromB()
        {
            return GetStringFromStack(B);
        }

        public string GetStringFromC()
        {
            return GetStringFromStack(C);
        }

        private string GetStringFromStack(Stack<int> s)
        {

            string temp = "";
            foreach (var item in s.Reverse())
            {
                temp += item;
                temp += ",";
            }
            return temp.TrimEnd(',');
        }
    }
}
