#include <gtest/gtest.h>
#include <iostream>

using namespace std;

class Stack {
private:
    int* array;
    int capacity;
    int top;

public:
    Stack(int size) {
        capacity = size;
        array = new int[capacity];
        top = -1;
    }

    ~Stack() {
        delete[] array;
    }

    void push(int value) {
        if (isFull()) {
            cout << "Stack overflow" << endl;
            return;
        }
        array[++top] = value;
    }

    int pop() {
        if (isEmpty()) {
            cout << "Stack is empty" << endl;
            return -1;
        }
        return array[top--];
    }

    bool isEmpty() {
        return top == -1;
    }

    bool isFull() {
        return top == capacity - 1;
    }

    int peek() {
        if (isEmpty()) {
            cout << "Stack is empty" << endl;
            return -1;
        }
        return array[top];
    }
};

TEST(StackTest, PushAndPeek) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    ASSERT_EQ(stack.peek(), 2);
}

TEST(StackTest, PopAndPeek) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    stack.pop();
    ASSERT_EQ(stack.peek(), 2);
}

TEST(StackTest, PopUntilEmpty) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    stack.pop();
    stack.pop();
    stack.pop();
    ASSERT_TRUE(stack.isEmpty());
}

TEST(StackTest, Overflow) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    stack.push(4);
    stack.push(5);
    stack.push(6);
    ASSERT_TRUE(stack.isFull());
}

TEST(StackTest, Underflow) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    stack.pop();
    stack.pop();
    stack.pop();
    ASSERT_TRUE(stack.isEmpty());
}

int main(int argc, char** argv) {

    Stack stack(4);

    stack.push(5);
    stack.push(0);
    stack.push(8);
    stack.push(-3);
    stack.push(6);

    cout << "Top element: " << stack.peek() << std::endl;
    cout << "Popped: " << stack.pop() << endl;
    cout << "Top element: " << stack.peek() << endl;

    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}
