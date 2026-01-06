#include <iostream>
#include <vector>
using namespace std;

double evaluatePolynomial(const vector<double>& coefficients, double x) {
    double result = 0.0;
    int n = coefficients.size();

    for (int i = 0; i < n; ++i) {
        result += coefficients[i] * pow(x, i);
    }

    return result;
}

int main() {
    
    int n;
    cout << "n = ";
    cin >> n;

    vector<double> coefficientsP(n + 1);
    cout << "Enter the coefficients of the polynomial P(x) degree " << n << "\n";
    for (int i = n; i >= 0; --i) {
        cout << "The coefficient at x^" << i << ": ";
        cin >> coefficientsP[i];
    }

    vector<double> coefficientsQ(n + 1);
    cout << "Enter the coefficients of the polynomial Q(x) degree " << n << "\n";
    for (int i = n; i >= 0; --i) {
        cout << "The coefficient at x^" << i << ": ";
        cin >> coefficientsQ[i];
    }

    double a;
    cout << "a = ";
    cin >> a;

    double result = evaluatePolynomial(coefficientsP, a + evaluatePolynomial(coefficientsQ, a) * evaluatePolynomial(coefficientsP, a + 1));

    cout << "Result: " << result << endl;

    return 0;
}
