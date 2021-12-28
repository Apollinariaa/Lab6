using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_6_cs
{
    public class Matrix
    {
        double[] s;
        public Matrix(int n)
        {
            s = new double[n];
            for (int i = 0; i < n; i++)
                s[i] = 2 * i + 1;
        }
        public Matrix(int n, double[] temp)
        {
            s = new double[n];
            for (int i = 0; i < n; i++)
                s[i] = temp[i];
        }

        //решениe системы линейных уравнений
        public double[] Solve(double[] b)
        {
            int n = s.Length;
            double[] x = new double[n];
            double[] y = new double[n];
            double[,] max1 = new double[n, n];
            double[,] max2 = new double[n, n];
            double[,] max3 = new double[n, n];
            double[,] max4 = new double[n, n];
            double[,] max12 = new double[n, n];
            double[,] max34 = new double[n, n];
            double[,] invert = new double[n, n];
            double[] rez = new double[n];
            x[0] = 1.0 / s[0];
            y[0] = x[0];
            for (int k = 1; k < n; k++)
            {
                double Fk = 0;
                double Gk = 0;
                double[] xk = new double[k];
                double[] yk = new double[k];
                for (int i = 0; i < k; i++)
                {
                    Fk += s[k - i] * x[i];
                    Gk += s[i + 1] * y[i];
                }
                double rk = 1.0 / (1.0 - Fk * Gk);
                double sk = -rk * Fk;
                double tk = -rk * Gk;

                for (int i = 0; i < k; i++)
                {
                    xk[i] = x[i];
                    yk[i] = y[i];
                }
                x[0] = xk[0] * rk + 0 * sk;
                y[0] = xk[0] * tk + 0 * rk;
                for (int i = 1; i < k; i++)
                {
                    x[i] = xk[i] * rk + yk[i - 1] * sk;
                    y[i] = xk[i] * tk + yk[i - 1] * rk;
                }
                x[k] = 0 * rk + yk[k - 1] * sk;
                y[k] = 0 * tk + yk[k - 1] * rk;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    max1[i, j] = (i >= j) ? x[i - j] : 0;
                    max2[i, j] = (j >= i) ? y[n - 1 - j + i] : 0;
                    max3[i, j] = (i > j) ? y[i - 1 - j] : 0;
                    max4[i, j] = (j > i) ? x[n - 1 - j + 1 + i] : 0;
                }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    double sum_ab = 0;
                    double sum_cd = 0;
                    for (int k = 0; k < n; k++)
                    {
                        sum_ab += max1[i, k] * max2[k, j];
                        sum_cd += max3[i, k] * max4[k, j];
                    }
                    max12[i, j] = sum_ab;
                    max34[i, j] = sum_cd;
                }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    invert[i, j] = (1.0 / x[0]) * (max12[i, j] - max34[i, j]);

            for (int i = 0; i < n; i++)
            {
                rez[i] = 0;
                for (int j = 0; j < n; j++)
                    rez[i] += invert[i, j] * b[j];
            }
            return rez;
        }

        public override string ToString()
        {
            string res = "";
            int n = s.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) res += ("\t" + s[Math.Abs(i - j)]);
                res += "\n ";
            }
            return res;
        }
    }
}