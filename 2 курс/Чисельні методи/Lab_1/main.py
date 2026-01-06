#Завдання №14. Метод простої ітерації. Лук'янчук Денис
import math as m

def f(x):
    return (pow(m.e, x) - 2) / 3

def iteration(x_0, epsilon):
    x_k_minus_1 = x_0
    n = 0

    while True:

        x_k = f(x_k_minus_1)

        if abs(x_k - x_k_minus_1) < epsilon:
            return n, x_k

        x_k_minus_1 = x_k
        print(f"Root: {x_k}, Iterations: {n}")
        n += 1


x_0 = float(input("Enter a x_0: "))
epsilon = 0.0001

iterations, root = iteration(x_0, epsilon)
print(f"Root: {root}, Iterations: {iterations}")