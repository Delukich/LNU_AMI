#include <iostream>
#include <cmath>

int main() {
    const double PI = 3.14159;
    double a;

    std::cout << "Enter the value of a: ";
    std::cin >> a;

    double z1 = pow(cos(3.0 / 8 * PI - a / 4), 2) - pow(cos(11.0 / 8 * PI + a / 4), 2);
    std::cout << "z1(in radians) = " << z1 << std::endl;

    z1 = z1 * 180 / PI;
    std::cout << "z1(in degrees) = " << z1 << std::endl;

    double z2 = (2 * a - 4) / abs(pow(a, 8) - 12 * pow(a, 4) + 5.1 * pow(a, 3));
    std::cout << "z2 = " << z2 << std::endl;

    return 0;
}

