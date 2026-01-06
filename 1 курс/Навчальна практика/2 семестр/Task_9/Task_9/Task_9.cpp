#include <iostream>
#include <list>
#include <algorithm>

using namespace std;

void displayList(const list<int>& lst) {
    for (auto it = lst.begin(); it != lst.end(); ++it) {
        cout << *it << " ";
    }
    cout << endl;
}

void moveLastToFront(list<int>& lst) {
    int lastElement = lst.back();
    lst.pop_back();
    lst.push_front(lastElement);
}

void insertInOrder(list<int>& lst, int newElement) {
    auto it = lst.begin();
    while (it != lst.end() && *it < newElement) {
        ++it;
    }
    lst.insert(it, newElement);
}

void removeEveryThird(list<int>& lst) {
    auto it = lst.begin();
    int count = 0;
    while (it != lst.end()) {
        ++count;
        if (count % 3 == 0) {
            it = lst.erase(it);
        }
        else {
            ++it;
        }
    }
}

int main() {
    list<int> myList;

    int numElements;
    cout << "Enter the number of elements in the list: ";
    cin >> numElements;

    cout << "Enter " << numElements << " elements:\n";
    for (int i = 0; i < numElements; ++i) {
        int element;
        cin >> element;
        myList.push_back(element);
    }

    cout << "Initial list: ";
    displayList(myList);

    moveLastToFront(myList);
    cout << "List after moving the last element to the front: ";
    displayList(myList);

    myList.sort();
    cout << "List after sorting: ";
    displayList(myList);

    int newElement;
    cout << "Enter a new element to insert: ";
    cin >> newElement;
    insertInOrder(myList, newElement);
    cout << "List after inserting the new element " << newElement << ": ";
    displayList(myList);

    removeEveryThird(myList);
    cout << "List after removing every third element: ";
    displayList(myList);

    return 0;
}
