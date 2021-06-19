using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrel_Merge_Sort
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<int> unsorted = new List<int>();
            List<int> sorted_lo = new List<int>();
            List<int> sorted_hi = new List<int>();
            List<int> sorted;
            Random random = new Random();
            Console.Write("Podaj ilości losowo generowanych elementów do posortowania: ");
            int g;
            while (!int.TryParse(Console.ReadLine(), out g) || (g<0))
            {//wypisanie komunikatu o błedzie
                Console.WriteLine("\nERROR: Ilość elementów musi być liczbą i musi być wieksza od 0 ");
                Console.Write("\nPodaj poprawną ilość elementów: ");
            }

            Console.WriteLine("Nieposortowane elementy: ");
            for (int i = 0; i < g; i++)
            {
                unsorted.Add(random.Next(0, 10*g));
                Console.Write(unsorted[i] + ", ");
            }

            

            var ha = g / 2;
            var unsorted_LO = unsorted.GetRange(0, ha);
            var unsorted_HI = unsorted.GetRange(ha, g - ha);


            Task Task1 = new Task(() => { sorted_lo = MergeSort(unsorted_LO); });
            Task Task2 = new Task(() => { sorted_hi = MergeSort(unsorted_HI); });


            Task[] tasks = new Task[2];
            Task1.Start();
            Task2.Start();
            tasks[0] = Task1;
            tasks[1] = Task2;

            Task.WaitAll(tasks);

            sorted = Merge(sorted_lo, sorted_hi);

            Console.WriteLine("Posortowane elementy: ");
            foreach (int x in sorted)
            {
                Console.Write(x + ", ");
            }
            Console.Write("\n");
        }
        

        private static List<int> MergeSort(List<int> unsorted_LO)
        {
            if (unsorted_LO.Count <= 1)
                return unsorted_LO;

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            int middle = unsorted_LO.Count / 2;
            for (int i = 0; i < middle; i++)  //Dzielenie nieposortowanej listy
            {
                left.Add(unsorted_LO[i]);
            }
            for (int i = middle; i < unsorted_LO.Count; i++)
            {
                right.Add(unsorted_LO[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }
       
        

        private static List<int> Merge(List<int> left, List<int> right)
        {
            List<int> result = new List<int>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First() <= right.First())  //porównywanie dwóch elementów do okreslenia który jest mniejszy
                    {
                        result.Add(left.First());
                        left.Remove(left.First());      //reszta listy bez pierwszego elementu
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());

                    right.Remove(right.First());
                }
            }
            return result;
        }
        
    }
}
