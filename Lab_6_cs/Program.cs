using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Lab_6_cs
{
    class Program
    {
        const string MatrixDll = "C:\\Users\\Полина\\source\repos\\MatrixDll.dll";
        [DllImport(MatrixDll)]
        static extern double Time_VR(int n, int repeat);
        [DllImport(MatrixDll)]
        static unsafe extern void Solve_VR(int n, double[] max_1, double[] max_2, double* max_3);

        static void Main(string[] args)
        {     
            const int n = 3;
            double[] max_1 = new double[n] { 6, 4, 5 };
            double[] max_2 = new double[n] { 7, -3, 4 };
            double[] max_3 = new double[n];
            // задаем матрицу 3-го порядка и правую часть с несовпадающими эл-тами в строках и столбцах.
            Matrix ob = new Matrix(n, max_1);
            double[] ans = ob.Solve(max_2);
            Console.WriteLine("----------------Матрица-----------------");
            Console.WriteLine(ob);
            Console.WriteLine("----------------------------------------");
            foreach (double i in max_2)
                Console.WriteLine(i);
            //Решаем систему линейных уравнений и выводим матрицу, правую часть и решение в С#
            Console.WriteLine("C#");
            foreach (double i in ans)
                Console.WriteLine(i);
            fixed (double* result = max_3)  Solve_VR(3, max_1, max_2, result);

            //Решаем систему линейных уравнений и выводим матрицу, правую часть и решение в С++
            Console.WriteLine("C++");
            for (int i = 0; i < max_3.Length; i++)
                Console.WriteLine(max_3[i]);
            TimeList t = new TimeList();
            //Создать один объект типа TimesList и предлогаем пользователю ввести имя файла
            Console.WriteLine("Введите имя файла");
            string filename = Console.ReadLine();
            if (File.Exists(filename))
            {
                Console.WriteLine("Файл найден и считан...");
                t.Load(filename);
                Console.WriteLine(t);
            }
            else
            {
                Console.WriteLine("Файла с таким именем не существует");
                File.Create(filename);
            }

            static double Time_VR(int n, int k)
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                Matrix matrix = new Matrix(n);
                double[] right = new double[n];

                for (int i = 0; i < n; i++)
                    right[i] = (i + 1) * 10;

                for (int i = 0; i < k; i++)
                    matrix.Solve(right);
                sw.Stop();
                return sw.Elapsed.TotalSeconds;
            }

        }
    }
}