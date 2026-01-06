#include <iostream>
#include <string>
using namespace std;

string numCut(const string& inputString) {
    string resultString;

    for (char s : inputString) {
        if (!isdigit(s)) {
            resultString += s;
        }
    }

    return resultString;
}

int main() {
    string inputSentence;

    cout << "Enter the sentence: ";
    getline(cin, inputSentence);

    string result = numCut(inputSentence);

    cout << "Result after deleted numbers: " << result << endl;

    return 0;
}
