#include <iostream>
#include <cmath>
#include <iomanip>

double calculateLnSeries(double x, double e) {
    double sum = 0;
    double term = x;
    int n = 0;

    while (abs(term) >= e) {
        sum += term;
        n++;
        term = pow(x, 2 * n + 1) / (2 * n + 1);
    }

    return 2 * sum;
}

int main() {
    double x, e;

    std::cout << "Enter the value of x (-1 < x < 1): ";
    std::cin >> x;

    if (abs(x) >= 1) {
        std::cout << "Invalid input: |x| must be less than 1." << std::endl;
        return 1;
    }

    std::cout << "Enter the value of e (e > 0): ";
    std::cin >> e;

    if (e <= 0) {
        std::cout << "Invalid input: e must be bigger than 0." << std::endl;
        return 1;
    }

    double result = calculateLnSeries(x, e);

    std::cout << "Result = " << result;


    return 0;
}
