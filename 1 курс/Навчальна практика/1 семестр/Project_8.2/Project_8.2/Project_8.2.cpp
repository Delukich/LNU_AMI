#include <iostream>
#include <fstream>
#include <list>
using namespace std;

int main() {
  
    ifstream inputFile("C:/Навчальна практика/Project_8.2/input.txt");
    if (!inputFile.is_open()) {
        cout << "Eror " << endl;
        return 1;
    }

    list<int> myList;
    int num;
    while (inputFile >> num) {
        myList.push_back(num);
    }

    inputFile.close();

    cout << "List:" << endl;
    for (const auto& element : myList) {
        cout << element << " ";
    }
    cout << endl;

    int E;
    cout << "E = ";
    cin >> E;
    auto it = myList.begin();
    while (it != myList.end() && *it == 0) {
        ++it;
    }
    myList.insert(it, E);

    cout << "Result:" << endl;
    for (const auto& element : myList) {
        cout << element << " ";
    }
    cout << endl;

    return 0;
}
