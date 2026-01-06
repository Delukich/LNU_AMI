#include <iostream>
#include <vector>

using namespace std;

class Matrix {
private:
    vector<vector<int>> matrix;
    int rows, cols;

public:
    Matrix(int rows, int cols) : rows(rows), cols(cols) {
        matrix.resize(rows);
        for (int i = 0; i < rows; ++i) {
            matrix[i].resize(cols, 0);
        }
    }

    void addElement(int row, int col, int value) {
        if (row >= 0 && row < rows && col >= 0 && col < cols) {
            matrix[row][col] = value;
        }
        else {
            cout << "Invalid coordinates" << endl;
        }
    }

    void removeElement(int row, int col) {
        if (row >= 0 && row < rows && col >= 0 && col < cols) {
            matrix[row][col] = 0;
        }
        else {
            cout << "Invalid coordinates" << endl;
        }
    }

    int getElement(int row, int col) {
        if (row >= 0 && row < rows && col >= 0 && col < cols) {
            return matrix[row][col];
        }
        else {
            cout << "Invalid coordinates" << endl;
            return -1; 
        }
    }

    void printMatrix() {
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                cout << matrix[i][j] << " ";
            }
            cout << endl;
        }
    }
};

int main() {
    Matrix matrix(3, 3);

    matrix.addElement(0, 0, 5);
    matrix.addElement(1, 1, 2);
    matrix.addElement(1, 2, 7);
    matrix.addElement(2, 0, -6);

    cout << "Matrix:" << endl;
    matrix.printMatrix();


    cout << "Get element(0,0): " << matrix.getElement(0, 0)<< endl;
    cout << "Get element(2,2): " << matrix.getElement(2, 2) << endl;

    matrix.removeElement(0, 0);

    cout << "Matrix after removing an element:" << endl;
    matrix.printMatrix();

    return 0;
}