#include <iostream>
#include <string>
using namespace std;

int main() {
    string text;
    cout << "Enter the text: ";
    getline(cin, text);

    size_t pos = text.find("1");

    while (pos != string::npos) {
        text.replace(pos, 1, "one"); 
        pos = text.find("1", pos + 1); 
    }

    cout << "Result: " << text << endl;

    return 0;
}
