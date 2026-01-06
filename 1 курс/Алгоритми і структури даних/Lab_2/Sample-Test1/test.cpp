#include "pch.h"
#include "C:/Алгоритми та структури даних/Lab_2/Lab_2/Lab_2.cpp"

TEST(StackTest, PushAndPeek) {
    Stack stack(3);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    ASSERT_EQ(stack.peek(), 3);
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
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}