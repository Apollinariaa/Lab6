// dllmain.cpp : Определяет точку входа для приложения DLL.

#include <iostream>
#include <cstdio>
#include <ctime>
#include "pch.h"
/*
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
*/

class Matrix
{
	int n = 0;  
	double* s;  

public:
	Matrix(int n)  
	{
		this->n = n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = 2 * i + 1;
	}

	Matrix(int n, double* mas)  
	{
		this->n = n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = mas[i];
	}

	Matrix(const Matrix& ob) 
	{
		n = ob.n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = ob.s[i];
	}

	~Matrix()  
	{
		delete s;
	}

	Matrix& operator=(const Matrix& temp)  
	{
		if (this == &temp) return *this;  
		delete[] s; 
		this->n = temp.n;  
		s = new double[n];  
		for (int i = 0; i < n; i++)
			s[i] = temp.s[i];   
		return *this;  
	}

	double* Solve(double b[], double* rez = nullptr) 
	{
		double* x = new double[n];
		double* y = new double[n];
		double** max1 = new double* [n]; 
		double** max2 = new double* [n];
		double** max3 = new double* [n];
		double** max4 = new double* [n];
		double** max12 = new double* [n];
	
		double** max34 = new double* [n];
		x[0] = 1.0 / s[0];
		y[0] = x[0];
		for (int k = 1; k < n; k++)
		{
			double Fk = 0;
			double Gk = 0;
			double* xk = new double[k];
			double* yk = new double[k];
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
			delete xk; delete yk;  
		}
	
		for (int i = 0; i < n; i++)
		{
			max1[i] = new double[n];
			max2[i] = new double[n];
			max3[i] = new double[n];
			max4[i] = new double[n];
			for (int j = 0; j < n; j++)
			{
				if (i >= j)  max1[i][j] = x[i - j]; 
				else max1[i][j] = 0;
				if (j >= i) max2[i][j] = y[n - 1 - j + i];
				else max2[i][j] = 0;
				if (i > j)  
				{
					max3[i][j] = y[i - 1 - j];
					max4[i][j] = 0;
				}
				else
				{
					max3[i][j] = 0;
					max4[i][j] = x[n - 1 - j + 1 + i];
				}
			}
		}

		for (int i = 0; i < n; i++)
		{
			max12[i] = new double[n];  
			max34[i] = new double[n];
			for (int j = 0; j < n; j++)
			{
				double sum12 = 0;
				double sum34 = 0;
				for (int k = 0; k < n; k++)
				{
					sum12 += max1[i][k] * max2[k][j];  
					sum34 += max3[i][k] * max4[k][j];  
				}
				max12[i][j] = sum12; 
				max34[i][j] = sum34;
			}
		}

		double** invert = new double* [n]; 
		for (int i = 0; i < n; i++)
		{
			invert[i] = new double[n];
			for (int j = 0; j < n; j++)
				invert[i][j] = (1.0 / x[0]) * (max12[i][j] - max34[i][j]);
		}

		if (rez == nullptr)  rez = new double[n];
		for (int i = 0; i < n; i++)
		{
			rez[i] = 0;
			for (int j = 0; j < n; j++)
				rez[i] += invert[i][j] * b[j];
		}

		delete[] max1;
		delete[] max2;
		delete[] max3;
		delete[] max4;
		delete[] max12;
		delete[] max34;
		delete[] invert;
		return rez;
	}
};


#include <iostream>
#include <cstdio>
#include <ctime>
using namespace std;

extern "C" __declspec(dllexport) double Time_VR(int n, int repeat)  
{
	double time = 0;
	clock_t begin = clock();
	Matrix m = Matrix(n);  
	double* b = new double[n];  
	for (int i = 0; i < n; i++)
		b[i] = 5 * i + 2;  
	for (int i = 0; i < repeat; i++)
		m.Solve(b);  
	time = (clock() - begin) / (double)CLOCKS_PER_SEC;  
	delete b;
	return time;
}

//решение матричного уравнения
extern "C" __declspec(dllexport) void Solve_VR(int n, double max_1[], double max_2[], double* max_3)
{
	Matrix matrix = Matrix(n, max_1);
	matrix.Solve(max_2, max_3);
}

