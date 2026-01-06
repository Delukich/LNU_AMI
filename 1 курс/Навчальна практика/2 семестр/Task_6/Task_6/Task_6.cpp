#include <iostream>

using namespace std;

struct Node {
    int data;
    Node* prev;
    Node* next;

    Node(int val) : data(val), prev(nullptr), next(nullptr) {}
};

void moveToStart(Node*& head, int position) {
    if (head == nullptr || position <= 1) {
        return; 
    }

    Node* current = head;
    for (int i = 1; current != nullptr && i < position; ++i) {
        current = current->next; 
    }

    if (current == nullptr) {
        return; 
    }

    current->prev->next = current->next;
    if (current->next != nullptr) {
        current->next->prev = current->prev;
    }

    current->prev = nullptr;
    current->next = head;
    head->prev = current;
    head = current;
}

void print(Node* head) {
    while (head != nullptr) {
        cout << head->data << " ";
        head = head->next;
    }
    cout << endl;
}

int main() {
    int length;
    cout << "Enter the length of the list: ";
    cin >> length;

    if (length <= 0) {
        cout << "Invalid list length!" << endl;
        return 1;
    }

    cout << "Enter the elements of the list:" << endl;
    Node* head = nullptr;
    Node* tail = nullptr;
    for (int i = 0; i < length; ++i) {
        int value;
        cin >> value;
        Node* newNode = new Node(value);
        if (head == nullptr) {
            head = newNode;
        }
        else {
            tail->next = newNode;
            newNode->prev = tail;
        }
        tail = newNode;
    }

    cout << "List: ";
    print(head);

    int position;
    cout << "Enter the position of the element: ";
    cin >> position;

    moveToStart(head, position);

    cout << "After moving: ";  
    print(head);

    while (head != nullptr) {
        Node* temp = head;
        head = head->next;
        delete temp;
    }

    return 0;
}
