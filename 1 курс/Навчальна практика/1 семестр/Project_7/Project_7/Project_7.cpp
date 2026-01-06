#include <iostream>
#include <vector>
using namespace std;

int main() {
    
    int n, m;
    cout << "Enter the number of rows (n): ";
    cin >> n;
    cout << "Enter the number of columns (m): ";
    cin >> m;

    double m;
    double matrix(n,m);
    cout << "Enter the matrix elements:\n";
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < m; ++j) {
            cout << "Enter element at position (" << i + 1 << ", " << j + 1 << "): ";
            cin >> matrix[i][j];
        }
    }

    double maxSmallestElement = matrix[0][0];
    int maxRowIndex = 0;
    int maxColIndex = 0;

    for (int i = 0; i < n; ++i) {
        double smallestElement = matrix[i][0];
        int colIndex = 0;

        for (int j = 1; j < m; ++j) {
            if (matrix[i][j] < smallestElement) {
                smallestElement = matrix[i][j];
                colIndex = j;
            }
        }

        if (smallestElement > maxSmallestElement) {
            maxSmallestElement = smallestElement;
            maxRowIndex = i;
            maxColIndex = colIndex;
        }
    }

    cout << "The smallest element in each row is: " << maxSmallestElement << endl;
    cout << "Index of the found element: (" << maxRowIndex + 1 << ", " << maxColIndex + 1 << ")" << endl;

    return 0;
}
