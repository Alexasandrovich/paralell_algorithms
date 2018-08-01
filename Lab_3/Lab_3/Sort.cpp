/*#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <algorithm>
#include <vector>
#include <omp.h>
#include <time.h>

using namespace std;

static int n;

void q_sort_parallel(vector<double> arr, int start = 0, int size = n - 1)
{
    double mid = arr[size / 2];
    int i = start;
    int j = size;
    while (i <= j)
    {
        while (arr[i] < mid)
        {
            i++;
        }
        while (arr[j] > mid)
        {
            j--;
        }
        if (i <= j)
        {
            swap(arr[i], arr[j]);
            i++;
            j--;
        }
    }
    {
#pragma omp parallel sections
        {
#pragma omp section
            q_sort_parallel(arr, start, j);
#pragma omp section
            q_sort_parallel(arr, i, size);
        }   
    }

}

void q_sort(vector<double> arr, int start = 0, int size = n - 1) {
    int i = start;
    int j = size - 1;

    int mid = arr[size / 2];

    while (i <= j); {
        while (arr[i] < mid) {
            i++;
        }
        while (arr[j] > mid) {
            j--;
        }
        if (i <= j) {
            swap(arr[i++], arr[j--]);
        }
    }

    q_sort(arr, start, j);
    q_sort(arr, i, size);
}

int main()
{
    clock_t watch_1, watch_2;
    int m;

    cout << "Put down numbers of elements" << endl;
    cin >> n;
    vector<double> vec_1(n), vec_2(n);

    for (int i = 0; i < n; i++)
        vec_1[i] = rand();

    for (int i = 0; i < n; i++)
        vec_2[i] = rand();

    cout << "Put down numbers of treads" << endl;
    cin >> m;
    omp_set_num_threads(m);


    watch_1 = clock();
    q_sort_parallel(vec_1, 0, n-1);
    watch_2 = clock();
    cout << "parallel with " << n << " elements:  " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;

    watch_1 = clock();
    q_sort(vec_2, 0, n-1);
    watch_2 = clock();


    cout << "NOT parallel with " << n << " elements:  " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;


    return 0;
}*/
#include <iostream>
#include <algorithm>
#include <vector>
#include <omp.h>
#include <time.h>

using namespace std;

static int n;

void mSort(float* a, const long n) {
    long i = 0, j = n;
    float mid = a[n / 2];

    do {
        while (a[i] < mid) i++;
        while (a[j] > mid) j--;

        if (i <= j) {
            std::swap(a[i], a[j]);
            i++; j--;
        }
    } while (i <= j);

    if (j > 0) mSort(a, j);

    if (n > i) mSort(a + i, n - i);
}


void mSort_parallel(float* a, const long n) {
    long i = 0, j = n;
    float mid = a[n / 2];

    do {
        while (a[i] < mid) i++;
        while (a[j] > mid) j--;

        if (i <= j) {
            std::swap(a[i], a[j]);
            i++; j--;
        }
    } while (i <= j);

    {
#pragma omp parallel sections
        {
#pragma omp section
            if (j > 0) mSort(a, j);
#pragma omp section
            if (n > i) mSort(a + i, n - i);
        }
    }
}

int main()
{
    clock_t watch_1, watch_2;
    int m;
    n = 100000;
    float vec_1[100000], vec_2[100000];

    for (int i = 0; i < n; i++)
        vec_1[i] = rand();

    for (int i = 0; i < n; i++)
        vec_2[i] = rand();

    cout << "Put down numbers of treads" << endl;
    cin >> m;
    omp_set_num_threads(m);


    watch_1 = clock();
    mSort_parallel(vec_1, n);
    watch_2 = clock();
    cout << "parallel with " << n << " elements:  " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;

    watch_1 = clock();
    mSort(vec_2, n);
    watch_2 = clock();


    cout << "NOT parallel with " << n << " elements:  " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;
}