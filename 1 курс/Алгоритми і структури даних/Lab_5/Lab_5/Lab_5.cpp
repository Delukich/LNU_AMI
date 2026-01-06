#include <iostream>
#include <stack>
#include <string>
#include <sstream>
#include <cmath>

using namespace std;

double evaluateRPN(const string& expr) {
    stack<double> operands;

    stringstream ss(expr);
    string token;

    while (ss >> token) {
        if (isdigit(token[0])) {
            operands.push(stod(token)); 
        }
        else {
            
            double operand2 = operands.top(); operands.pop();
            double operand1 = operands.top(); operands.pop();
            double result;

            switch (token[0]) {
            case '+':
                result = operand1 + operand2;
                break;
            case '-':
                result = operand1 - operand2;
                break;
            case '*':
                result = operand1 * operand2;
                break;
            case '/':
                result = operand1 / operand2;
                break;
            case '^':
                result = pow(operand1, operand2);
                break;
            default:
                cout << "Unknown operator: " << token << endl;
                return 0;
            }
            operands.push(result); 
        }
    }
    return operands.top(); 
}

int main() {
    string expr;
    cout << "Enter an expression: ";
    getline(cin, expr);

    double result = evaluateRPN(expr);
    cout << "Result: " << result << endl;

    return 0;
    
}
