#include <iostream>

int main() {
    double a, b, c, x;

    std::cout << " a = ";
    std::cin >> a;

    std::cout << " b = ";
    std::cin >> b;

    std::cout << " c = ";
    std::cin >> c;

    std::cout << " x = ";
    std::cin >> x;

    double result;

    if (x + 5 < 0 && c == 0) {
        double result1 = (1 / (a * x)) - b;
        std::cout << " Result 1 = " << result1 << std::endl;
    }
    else if (x + 5 > 0 && c != 0) {
        double result2 = (x - a) / x;
        std::cout << " Result 2 = " << result2 << std::endl;
    }
    else {
        double result3 = (10 * x) / (c - 4);
        std::cout << " Result 3 = " << result3 << std::endl;
    }

    return 0;
}



