import numpy as np
import matplotlib.pyplot as plt

def f(x, y):
    return (y * (x + y)) / (x**2)

def exact_solution(x):
    return -x / (1 + np.log(x))

def euler_method(f, x0, y0, h, n):
    x = np.linspace(x0, x0 + n * h, n + 1)
    y = np.zeros(n + 1)
    y[0] = y0
    for i in range(n):
        y[i + 1] = y[i] + h * f(x[i], y[i])
    return x, y

def runge_method(f, x0, y0, h, n):
    x = np.linspace(x0, x0 + n * h, n + 1)
    y = np.zeros(n + 1)
    y[0] = y0
    for i in range(n):
        k1 = f(x[i], y[i])
        k2 = f(x[i] + h / 2, y[i] + h * k1 / 2)
        k3 = f(x[i] + h / 2, y[i] + h * k2 / 2)
        k4 = f(x[i] + h, y[i] + h * k3)
        y[i + 1] = y[i] + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4)
    return x, y

x0, y0 = 1, -1
h = 0.1
n = int((2 - x0) / h)

x_euler, y_euler = euler_method(f, x0, y0, h, n)
x_rk, y_rk = runge_method(f, x0, y0, h, n)

x_fn = np.linspace(x0, 2, 1000)
y_fn = exact_solution(x_fn)

error_euler = np.abs(exact_solution(x_euler) - y_euler)
error_rk = np.abs(exact_solution(x_rk) - y_rk)

plt.figure(figsize=(12, 6))
plt.plot(x_fn, y_fn, label="Точний розв'язок", color="black", linewidth=2)
plt.plot(x_euler, y_euler, label="Метод Ейлера", linestyle="--")
plt.plot(x_rk, y_rk, label="Метод Рунге-Кутта 4-го порядку", linestyle="--")
plt.title("Розв'язки задачі Коші")
plt.xlabel("x")
plt.ylabel("y")
plt.legend()
plt.grid()
plt.show()

print(f"Максимальна помилка методу Ейлера: {np.max(error_euler)}")
print(f"Максимальна помилка методу Рунге-Кутта 4-го порядку: {np.max(error_rk)}")
