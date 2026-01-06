import numpy as np
import matplotlib.pyplot as plt

def f1(x, y):
    return (x + 1) ** 2 + y ** 2 - 2

def f2(x, y):
    return np.exp(x * y) - x + y - 1.5

# Метод простої ітерації
def simple_iterate(x0, y0, epsilone):
    x, y = x0, y0
    iterations = 0

    while True:
        iterations += 1
        x_new = np.sqrt(2 - y ** 2) - 1
        y_new = 1.5 - np.exp(x * y) + x

        print(f"Ітерація №{iterations}: x = {x}, y = {y}")

        if abs(x_new - x) < epsilone and abs(y_new - y) < epsilone:
            break
        x, y = x_new, y_new
    return x, y, iterations

# Метод Ньютона
def newton(x0, y0, epsilon):
    x, y = x0, y0
    iterations = 0

    def jacobi(x, y):
        return np.array([[2 * (x + 1), 2 * y],
                         [y * np.exp(x * y) - 1, x * np.exp(x * y) + 1]])

    def F(x, y):
        return np.array([f1(x, y), f2(x, y)])

    while True:
        iterations += 1
        Jac = jacobi(x, y)
        vuznach_Jac = np.linalg.det(Jac)
        if np.abs(vuznach_Jac) < epsilon:
            return None, None, iterations
        J_obern = np.linalg.inv(Jac)
        F_value = F(x, y)
        dobutok = np.dot(J_obern, F_value)
        x_new, y_new = np.array([x, y]) - dobutok

        print(f"Ітерація №{iterations}: x = {x_new}, y = {y_new}")

        if np.linalg.norm(dobutok) < epsilon:
            return x_new, y_new, iterations
        x, y = x_new, y_new

x_vals = np.linspace(-3, 3, 400)
y_vals = np.linspace(-3, 3, 400)
X, Y = np.meshgrid(x_vals, y_vals)

F1 = (X + 1) ** 2 + Y ** 2 - 2
F2 = np.exp(X * Y) - X + Y - 1.5

plt.contour(X, Y, F1, [0], colors='blue')
plt.contour(X, Y, F2, [0], colors='red')
plt.title("Графік")
plt.xlabel("x")
plt.ylabel("y")
plt.grid(True)
plt.show()

x0, y0 = 0.35, 0.65
epsilon = 0.0001

x_sol, y_sol, iters_newton3 = simple_iterate(x0, y0, epsilon)
x_newton, y_newton, iters_newton = newton(x0, y0, epsilon)

print(f"\nМетод простої ітерації: x = {x_sol}, y = {y_sol}, кількість ітерацій = {iters_newton3}")
print(f"Метод Ньютона: x = {x_newton}, y = {y_newton}, кількість ітерацій = {iters_newton}")