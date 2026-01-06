#include <iostream>

using namespace std;

struct Node {
    int data;
    Node* next;

    Node(int value) {
        data = value;
        next = nullptr;
    }
};

void insert(Node*& head, int value) {
    Node* newNode = new Node(value);

    if (head == nullptr || head->data >= value) {
        newNode->next = head;
        head = newNode;
    }
    else {
        Node* current = head;
        while (current->next != nullptr && current->next->data < value) {
            current = current->next;
        }
        newNode->next = current->next; 
        current->next = newNode;
    }
}

void remove(Node*& head) {
    if (head == nullptr) return;

    if (head->data < 0) {
        Node* temp = head;
        head = head->next;
        delete temp;
        return;
    }

    Node* current = head;
    while (current->next != nullptr && current->next->data >= 0) {
        current = current->next;
    }

    if (current->next != nullptr && current->next->data < 0) {
        Node* temp = current->next;
        current->next = current->next->next;
        delete temp;
    }
}

void print(Node* head) {
    while (head != nullptr) {
        cout << head->data << " ";
        head = head->next;
    }
    cout << endl;
}

int main() {
    Node* head = nullptr;
    int length;
    cout << "Enter the length of the list: ";
    cin >> length;

    cout << "Enter the elements of the list:\n";
    for (int i = 0; i < length; ++i) {
        int element;
        cin >> element;
        insert(head, element);
    }

    cout << "List: ";
    print(head);

    int E;
    cout << "Enter a new element to insert: ";
    cin >> E;
    insert(head, E);
    cout << "List after inserting " << E << ": ";
    print(head);

    remove(head);
    cout << "List after removing the first negative element: ";
    print(head);

    return 0;
}
