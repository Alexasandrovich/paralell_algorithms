#define _CRT_SECURE_NO_WARNINGS
#include <omp.h>
#include <iostream>
#include <sstream>
#include <vector>
#include <time.h>
#include "windows.h"

#define wheels_change 0
#define tires_change 1

using namespace std;

template <typename T>
void crit_out(int time_now, T s) // show current time
{
#pragma omp critical // the block after the critical Directive will be executed by all threads, but in queue
    {
        cout << s << ", current time:  " << time_now << endl;
        cerr << s << ", current time:  " << time_now << endl;
    }
}


struct car_enthusiast
{
    int number;  
    bool was_served; 
    int time_start_of_order;
    int time_go_out;
    int wheels_or_tires;

    car_enthusiast() {}; // default constructor

    car_enthusiast(int time, int i)
    {
        number = i+1; // due to start '0'
        time_start_of_order = time;
        time_go_out = time + 100 + rand() % 300;
        was_served = false;
        wheels_or_tires = rand() & 1; // conjuction
    }

    void serve(int& cur_time)
    {
#pragma omp critical
        {
            was_served = true;
        }
        cur_time += 450 - wheels_or_tires * 300; //depends on wheels_or_tires
        crit_out(cur_time, "Car enthusiast " + to_string(number) + " was served");
    }

    void working()
    {
        if (wheels_or_tires == wheels_change)
            crit_out( time_start_of_order, "Car enthusiast " + to_string(number) + " came to change wheels");
        else
            crit_out( time_start_of_order, "Car enthusiast " + to_string(number) + " came to change tires");
        
        if (!was_served)
            crit_out(time_go_out, "Car enthusiast " + to_string(number) + " is gone away");
    }
    
};

struct queue_in_box
{
    int num;
    vector<car_enthusiast> car_enthusiasts;
};


void master(queue_in_box& que)
{
    int current_time = 0;
    bool tv = true;
    for (int i = 0; i < que.num; ++i)
    {
        if (que.car_enthusiasts[i].time_start_of_order > current_time) 
        {
            if (!tv) // check, that TV is off
            {
                tv = true;
                crit_out(current_time, "let's watch TV");
            }
            current_time = que.car_enthusiasts[i].time_start_of_order;
        }
        if (current_time >= que.car_enthusiasts[i].time_start_of_order && current_time <= que.car_enthusiasts[i].time_go_out)
        {
            if (tv) // check, that TV is on
            {
                tv = false;
                crit_out(current_time, "Go to Work, turn off your TV");
            }
            que.car_enthusiasts[i].serve(current_time);
        }
        if (current_time >  que.car_enthusiasts[i].time_start_of_order)
            continue;
    }
    crit_out(current_time, "Can exhale, customers is now over!");
}

int main() {

    srand(time(NULL)); // random

    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);

    queue_in_box que;
    cin >> que.num;

    que.car_enthusiasts.resize(que.num);

    int current_time = 0;

    for (int i = 0; i < que.num; ++i)
    {
        que.car_enthusiasts[i] = car_enthusiast(current_time, i);
        current_time += 100 + rand() % 900;
    }

    omp_set_num_threads(que.num + 10);

#pragma omp parallel sections
    {
#pragma omp section
        {
#pragma omp parallel for 
            for (int i = 0; i < que.num; ++i)
            {
                que.car_enthusiasts[i].working();
                //master(que);
                system("pause");
            }
        }
#pragma omp section
        {
            master(que);
            system("pause");
        }
    }
    system("pause");
}