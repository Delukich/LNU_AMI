#include <iostream>

using namespace std;

void sort(int arr[], int n) {
    for (int i = 1; i < n; i++) {
        int first = arr[i];
        int j = i - 1;

        while (j >= 0 && arr[j] > first) {
            arr[j + 1] = arr[j];
            j = j - 1;
        }

        arr[j + 1] = first;

        cout << "Iteration " << i << ": ";
        for (int k = 0; k < n; ++k) {
            cout << arr[k] << " ";
        }
        cout << endl;
    }
}

void print(int arr[], int size) {
    for (int i = 0; i < size; i++) {
        cout << arr[i] << " ";
    }
    cout << endl;
}

int main() {
    int arr[] = { 20, -3, 7, 4, 13, -1, 6, -8, 7 };
    int n = sizeof(arr) / sizeof(arr[0]);

    cout << "Initial Array: ";
    print(arr, n);

    sort(arr, n);

    cout << "Sorrted Array: ";
    print(arr, n);

    return 0;
}
