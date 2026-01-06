#include <iostream>
#include <queue>

using namespace std;

struct Node {
    int key;
    Node* left;
    Node* right;
    int height;
};

Node* newNode(int key) {
    Node* node = new Node();
    node->key = key;
    node->left = nullptr;
    node->right = nullptr;
    node->height = 1;
    return node;
}

int height(Node* node) {
    if (node == nullptr) return 0;
    return node->height;
}

Node* rightRotate(Node* y) {
    Node* x = y->left;
    Node* T2 = x->right;
    x->right = y;
    y->left = T2;
    y->height = max(height(y->left), height(y->right)) + 1;
    x->height = max(height(x->left), height(x->right)) + 1;
    return x;
}

Node* leftRotate(Node* x) {
    Node* y = x->right;
    Node* T2 = y->left;
    y->left = x;
    x->right = T2;
    x->height = max(height(x->left), height(x->right)) + 1;
    y->height = max(height(y->left), height(y->right)) + 1;
    return y;
}

void printAVL(Node* root, int space = 0, int count = 10) {
    if (root == nullptr) return;
    space += count;
    printAVL(root->right, space);
    cout << endl;
    for (int i = count; i < space; i++)
        cout << " ";
    cout << root->key << "\n";
    printAVL(root->left, space);
}

Node* minValueNode(Node* node) {
    Node* current = node;
    while (current && current->left != nullptr)
        current = current->left;
    return current;
}

Node* deleteNode(Node* root, int key) {
    if (root == nullptr) return root;
    if (key < root->key)
        root->left = deleteNode(root->left, key);
    else if (key > root->key)
        root->right = deleteNode(root->right, key);
    else {
        if ((root->left == nullptr) || (root->right == nullptr)) {
            Node* temp = root->left ? root->left : root->right;
            if (temp == nullptr) {
                temp = root;
                root = nullptr;
            }
            else
                *root = *temp;
            delete temp;
        }
        else {
            Node* temp = minValueNode(root->right);
            root->key = temp->key;
            root->right = deleteNode(root->right, temp->key);
        }
    }
    if (root == nullptr)
        return root;
    root->height = 1 + max(height(root->left), height(root->right));
    int balance = height(root->left) - height(root->right);
    if (balance > 1 && height(root->left->left) >= height(root->left->right))
        return rightRotate(root);
    if (balance < -1 && height(root->right->right) >= height(root->right->left))
        return leftRotate(root);
    if (balance > 1 && height(root->left->left) < height(root->left->right)) {
        root->left = leftRotate(root->left);
        return rightRotate(root);
    }
    if (balance < -1 && height(root->right->right) < height(root->right->left)) {
        root->right = rightRotate(root->right);
        return leftRotate(root);
    }
    return root;
}

Node* insert(Node* node, int key) {
    if (node == nullptr) return newNode(key);
    if (key <= node->key)
        node->left = insert(node->left, key);
    else
        node->right = insert(node->right, key);
    node->height = 1 + max(height(node->left), height(node->right));
    int balance = height(node->left) - height(node->right);
    if (balance > 1 && key < node->left->key)
        return rightRotate(node);
    if (balance < -1 && key > node->right->key)
        return leftRotate(node);
    if (balance > 1 && key > node->left->key) {
        node->left = leftRotate(node->left);
        return rightRotate(node);
    }
    if (balance < -1 && key < node->right->key) {
        node->right = rightRotate(node->right);
        return leftRotate(node);
    }
    return node;
}

bool search(Node* root, int key) {
    if (root == nullptr || root->key == key)
        return root != nullptr;
    if (root->key > key)
        return search(root->left, key);
    return search(root->right, key);
}

int main() {
    Node* root = nullptr;

    root = insert(root, 10);
    root = insert(root, 2);
    root = insert(root, 13);
    root = insert(root, 22);
    root = insert(root, 1);
    root = insert(root, 9);
    root = insert(root, 8);

    cout << "Result: " << endl;
    printAVL(root);
    cout << endl;

    int searchKey = 9;
    if (search(root, searchKey))
        cout << "Element " << searchKey << " found in a tree" << endl;
    else
        cout << "Element " << searchKey << " not found in tree" << endl;

    root = deleteNode(root, 9);

    cout << "Result after remove: " << endl;
    printAVL(root);

    if (search(root, searchKey))
        cout << "Element " << searchKey << " found in a tree" << endl;
    else
        cout << "Element " << searchKey << " not found in tree" << endl;
    
    return 0;
}
