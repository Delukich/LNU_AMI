#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
using namespace std;

int main() {
   ifstream inputFile("C:/Навчальна практика/Project_9.1/input.txt");

    if (!inputFile.is_open()) {
        cout << "Error opening the file " << endl;
        return 1;
    }

    string line;
    while (getline(inputFile, line)) {
        istringstream iss(line);
        string word;

        while (iss >> word) {
            if (word.find(word[0], 1) != string::npos) {
                cout << word << endl;
            }
        }
    }

    inputFile.close();

    return 0;
}
