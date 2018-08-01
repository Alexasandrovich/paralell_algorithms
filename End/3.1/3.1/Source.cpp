#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <vector>
#include <chrono>
#include <omp.h>
using namespace std;
using namespace chrono;

#ifdef _WIN32
#include <windows.h>
#elif MACOS
#include <sys/param.h>
#include <sys/sysctl.h>
#else
#include <unistd.h>
#endif

int getNumberOfCores() {
#ifdef WIN32
    SYSTEM_INFO sysinfo;
    GetSystemInfo(&sysinfo);
    return sysinfo.dwNumberOfProcessors;
#elif MACOS
    int nm[2];
    size_t len = 4;
    uint32_t count;

    nm[0] = CTL_HW; nm[1] = HW_AVAILCPU;
    sysctl(nm, 2, &count, &len, NULL, 0);

    if (count < 1) {
        nm[1] = HW_NCPU;
        sysctl(nm, 2, &count, &len, NULL, 0);
        if (count < 1) { count = 1; }
    }
    return count;
#else
    return sysconf(_SC_NPROCESSORS_ONLN);
#endif
}


template<typename T>
struct matrix
{

    int row = 0;
    int col = 0;

    vector<vector<T> > m;

    matrix() = default;
    matrix(int r, int c)
        : row(r)
        , col(c)
    {
        m.assign(row, vector<T>(col, 0));
    }

    vector<T>& operator[](int i)
    {
        return m[i];
    }

    matrix operator*(matrix a)
    {
        int n = this->row;
        int m = a.col;

        matrix res(n, m);

#pragma omp parallel for schedule (static, 10)
        for (int i = 0; i < n; ++i)
            for (int k = 0; k < n; ++k)
                for (int j = 0; j < m; ++j)
                    res[i][j] += (*this)[i][k] * a[k][j];

        return res;
    }

};


int main()
{
    matrix<double> a, b;
    omp_set_num_threads(getNumberOfCores());
    bool yes;
    cin >> yes;
    if (yes)
    {
        int n = rand() % 999 + 1;
        n = 1000;
        a = matrix<double>(n, n);
        b = matrix<double>(n, n);

        for (auto& itt : a.m)
            for (auto& it : itt)
                it = rand();

        for (auto& itt : b.m)
            for (auto& it : itt)
                it = rand();


    }
    else
    {
        freopen("input.txt", "r", stdin);

        int n;
        cin >> n;

        a = matrix<double>(n, n);
        b = matrix<double>(n, n);

        for (auto& itt : a.m)
            for (auto& it : itt)
            {
                cin >> it;
                cout << it << endl;
            }


        for (auto& itt : b.m)
            for (auto& it : itt)
                cin >> it;
    }
    freopen("output.txt", "w", stdout);

    steady_clock::time_point c = steady_clock::now();
    auto res = a * b;
    steady_clock::time_point d = steady_clock::now();

    //cout << steady_clock::duration(d - c).count() / 1000000000.0;

    return 0;
}