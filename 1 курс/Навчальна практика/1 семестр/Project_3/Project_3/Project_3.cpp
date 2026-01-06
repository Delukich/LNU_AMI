#include <iostream>
#include <cmath>

double calculateY(double a, double x, double e) {
    double y = a;
    double y1 = 0;
    double y2 = y;

    do {
        y1 = y;
        y = 0.5 * (y1 + x / y1);
    } while (abs(pow(y, 2) - pow(y1, 2)) >= e);

    return y;
}

int main() {
    double a, x, e;
    std::cout << " a = ";
    std::cin >> a;
    std::cout << " x =  ";
    std::cin >> x;
    std::cout << " e = ";
    std::cin >> e;

    double result = calculateY(a, x, e);
    std::cout << "Result: " << result << std::endl;

    return 0;
}
