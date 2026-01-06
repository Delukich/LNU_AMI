#include <iostream>
#include <vector>
#include <algorithm>

class Rational {
private:
    int numerator;
    int denominator;


    int gcd(int a, int b) {
        return b == 0 ? a : gcd(b, a % b);
    }


    void reduce() {
        int commonDivisor = gcd(abs(numerator), abs(denominator));
        numerator /= commonDivisor;
        denominator /= commonDivisor;
    }

public:

    Rational() : numerator(0), denominator(1) {}
    Rational(int num, int denom) : numerator(num), denominator(denom) { reduce(); }


    Rational operator+(const Rational& other) const {
        return Rational(numerator * other.denominator + other.numerator * denominator,
            denominator * other.denominator);
    }

    Rational operator-(const Rational& other) const {
        return Rational(numerator * other.denominator - other.numerator * denominator,
            denominator * other.denominator);
    }

    Rational operator*(const Rational& other) const {
        return Rational(numerator * other.numerator, denominator * other.denominator);
    }

    Rational operator/(const Rational& other) const {
        return Rational(numerator * other.denominator, denominator * other.numerator);
    }

    bool operator<(const Rational& other) const {
        return numerator * other.denominator < other.numerator * denominator;
    }

    bool operator==(const Rational& other) const {
        return numerator * other.denominator == other.numerator * denominator;
    }


    friend std::ostream& operator<<(std::ostream& os, const Rational& rational) {
        os << rational.numerator << "/" << rational.denominator;
        return os;
    }

    friend std::istream& operator>>(std::istream& is, Rational& rational) {
        is >> rational.numerator >> rational.denominator;
        rational.reduce();
        return is;
    }


    Rational& operator=(const Rational& other) {
        if (this != &other) {
            numerator = other.numerator;
            denominator = other.denominator;
        }
        return *this;
    }


    void reduceFraction() {
        reduce();
    }
};

int main() {

    std::vector<Rational> sequence;
    std::cout << "Enter a sequence of rational numbers (numerator denominator): ";
    Rational inputRational;
    while (std::cin >> inputRational) {
        sequence.push_back(inputRational);
    }


    std::sort(sequence.begin(), sequence.end());


    Rational largest = sequence.back();
    Rational smallest = sequence.front();


    Rational sumBetween;
    bool between = false;
    for (const Rational& num : sequence) {
        if (num == smallest) {
            between = true;
        }
        else if (num == largest) {
            between = false;
            break;
        }

        if (between) {
            sumBetween = sumBetween + num;
        }
    }


    Rational productEven;
    Rational divisorOdd = sequence[0];
    for (const Rational& num : sequence) {
        if (num == divisorOdd) {
            divisorOdd.reduceFraction();
        }
        else {
            productEven = productEven * num;
        }
    }


    std::cout << "\nSorted Sequence: ";
    for (const Rational& num : sequence) {
        std::cout << num << " ";
    }

    std::cout << "\nSum between largest and smallest: " << sumBetween << std::endl;
    std::cout << "Product of even elements: " << productEven << std::endl;
    std::cout << "Divide odd elements by: " << divisorOdd << std::endl;

    return 0;
}
