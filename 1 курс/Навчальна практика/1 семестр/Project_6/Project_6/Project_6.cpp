#include <iostream>
using namespace std;

int main() {
    int n, k;
    cout << "n = ";
    cin >> n;

    int arr[n];
    cout << "Enter the elements of the array: ";
    for (int i = 0; i < n; ++i) {
        cin >> arr[i];
    }

    cout << "k = ";
    cin >> k;

    int lastElement = -1;

    for (int i = n - 1; i >= 0; --i) {
        if (arr[i] >= -k && arr[i] <= k) {
            lastElement = i;
            break;
        }
    }

    if (lastElement != -1) {
        cout << "The last element in the span [-" << k << ", " << k << "] has an index: " << lastElement << endl;
    }
    else {
        cout << "There are no elements in the array [-" << k << ", " << k << "] " << endl;
    }

    return 0;
}

