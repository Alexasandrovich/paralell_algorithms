#include <iostream>
#include <cmath>
#include <omp.h>
#include <chrono>
#include <time.h>


using namespace std;


double static func(int n, double alpha)
{
    return 1 / pow(n, alpha);

}

double static summ_parallel(int n, double alpha, int thr)
{
    double summ = 0;
    omp_set_num_threads(thr);
#pragma omp parallel for
    for (int i = 1; i < n + 1; i++)
    {
        summ += func(i, alpha);
    }
    return summ;
}

double summ_just(int n, double alpha)
{
    double summ = 0;
    for (int i = 1; i < n + 1; i++)
    {
        summ += func(i, alpha);
    }
    return summ;
}


int main()
{
    int N;
    cout << "Put down N" << endl;
    cin >> N;
    while (N < 0)
    {
        cin >> N;
        cout << "Error due to N<0 " << endl;
    }

    int thr;
    cout << "Put down number of thread" << endl;
    cin >> thr;
    while (thr < 0)
    {
        cin >> thr;
        cout << "Error due to thred < 0 " << endl;
    }

    int alpha;
    cout << "Put down alpha" << endl;
    cin >> alpha;

    clock_t watch_1, watch_2;

    watch_1 = clock();
    double a1 = summ_just(N, alpha);
    watch_2 = clock();
    cout << "parallel with " << N << " elements:  " << a1 << " for " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << " seconds" << endl;

    watch_1 = clock();
    double a2 = summ_parallel(N, alpha, thr);
    watch_2 = clock();
    cout << "ordinary with " << N << " elements:  " << a2 << " for " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << " seconds" << endl;

    double N1 = N;

    for (double i = 0; i < 100; i++)
    {
        N1 *= i;
        watch_1 = clock();
        double a1 = summ_just(N1, alpha);
        watch_2 = clock();
        cout << N1 << "   " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;
    }

    double N2 = N;

    cout << "----------------------" << endl;

    for (double i = 0; i < 100; i++)
    {
        watch_1 = clock();
        double a2 = summ_parallel(N2, alpha, thr);
        watch_2 = clock();
        cout << N2 << "   " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;
        N2 *= i;
    }

    system("pause");
}

//{ double summ = 0; 
//#pragma omp parallel shared(a) reduction (+: sum) num_threads(2)
//    {
//# pragma omp for
//        for (int i = 1; i < N + 1; i++)
//        {
//            summ += 1 / (pow(i, alpha));
//        }
//    }
//
//    cout << "summ of range is " << summ << endl;
//    system("pause");
//}