#include <iostream>
#include <algorithm>

using namespace std;

template<typename T>
void customSort(T arr[], int size) {
    bool par = size % 2 != 0;

    sort(arr, arr + size / 2, greater<T>());

    sort(arr + size / 2 + par, arr + size, less<T>());
}

int main() {
    int size;
    cout << "Enter the size of the array: ";
    cin >> size;

    int* intArr = new int[size]; 
    cout << "Enter " << size << " integer elements: ";
    for (int i = 0; i < size; ++i) {
            cin >> intArr[i];
    }
    customSort(intArr, size);

    cout << "Sorted array: ";
    for (int i = 0; i < size; ++i) {
        cout << intArr[i] << " ";
    }
    cout << endl;

    delete[] intArr; 

 
    cout << "Enter the size of the array: ";
    cin >> size;

    char* charArr = new char[size];
    cout << "Enter " << size << " character elements: ";
    for (int i = 0; i < size; ++i) {
        cin >> charArr[i];
    }
    customSort(charArr, size);

    cout << "Sorted character array: ";
    for (int i = 0; i < size; ++i) {
        cout << charArr[i] << " ";
    }
    cout << endl;

    delete[] charArr;

    return 0;

}
