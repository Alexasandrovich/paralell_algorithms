#include <thread>
#include <iostream>
#include <iostream>
#include <algorithm>
#include <vector>
#include <omp.h>
#include <time.h>
using namespace std;

void sort_proc(int * a, int i, int size) {
    for (int j = size - 1; j > i; j--)
    {
        // Если соседние элементы расположены
        // в неправильном порядке, то меняем
        // их местами
        if (a[j] < a[j - 1])
        {
            swap(a[j], a[j - 1]);
        }
    }
}

int main()
{

    

    clock_t watch_1, watch_2;
    clock_t watch_3, watch_4;
    watch_1 = clock();
    int n = 10000;
    int a[10000];
    int b[10000];
    for (int i = 0; i < n; i++)
        a[i] = rand();

    watch_3 = clock();

    for (int i = 0; i < n; i++)
        b[i] = rand();
    for (int i = 0; i < n; i++)
    {
        for (int j = n - 1; j > i; j--)
        {
            if (b[j] < b[j - 1])
            {
                swap(b[j], b[j - 1]);
            }
        }
    }
    watch_4 = clock();
    int size = sizeof(a) / sizeof(a[0]);
    thread ** pool = new thread*[size];
    // Внешний цикл алгоритма совершает
    // ровно size итераций
    for (int i = 0; i < size; i++)
    {
        // Массив просматривается с конца до
        // позиции i и "легкие элементы всплывают"
        /*for (int j = size - 1; j > i; j--)
        {
        // Если соседние элементы расположены
        // в неправильном порядке, то меняем
        // их местами
        if (a[j] < a[j - 1])
        {
        swap (a[j], a[j - 1]);
        }
        }*/
        pool[i] = new thread(sort_proc, a, i, size);
        pool[i]->join();

    }
    watch_2 = clock();
    cout << "parallel with " << n << " elements:  " << (watch_2 - watch_1) / double(CLOCKS_PER_SEC) << endl;
    cout << "NOT parallel with " << n << " elements:  " << (watch_4 - watch_3) / double(CLOCKS_PER_SEC) << endl;
    delete[] pool;
    system("pause");
    return 0;
}