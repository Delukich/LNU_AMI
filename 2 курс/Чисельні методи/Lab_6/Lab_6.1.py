import numpy as np
import matplotlib.pyplot as plt

def f(x):
    return 1 / np.sqrt(2 * x**2 + 0.7)

def F(x):
    return 1 / np.sqrt(2) * np.log(x + np.sqrt(x**2 + 0.35))

a = 1.4
b = 2
true_value = F(b) - F(a)

def rectangle_method(f, a, b, n):
    h = (b - a) / n
    result = sum(f(a + i * h) for i in range(n)) * h
    return result

def trapezoidal_method(f, a, b, n):
    h = (b - a) / n
    result = (f(a) + f(b)) / 2 + sum(f(a + i * h) for i in range(1, n))
    result *= h
    return result

def simpson_method(f, a, b, n):
    if n % 2 != 0:
        n += 1  
    h = (b - a) / n
    result = f(a) + f(b)
    result += 4 * sum(f(a + i * h) for i in range(1, n, 2))
    result += 2 * sum(f(a + i * h) for i in range(2, n-1, 2))
    result *= h / 3
    return result

epsilon = 1e-6

def iterate_to_precision(method, f, a, b, epsilon):
    n = 1
    current_result = method(f, a, b, n)
    iterations = [(n, current_result)]
    while True:
        n *= 2
        new_result = method(f, a, b, n)
        iterations.append((n, new_result))
        if abs(new_result - current_result) < epsilon:
            return new_result, n, iterations
        current_result = new_result

rect_result, rect_n, rect_iterations = iterate_to_precision(rectangle_method, f, a, b, epsilon)
trap_result, trap_n, trap_iterations = iterate_to_precision(trapezoidal_method, f, a, b, epsilon)
simp_result, simp_n, simp_iterations = iterate_to_precision(simpson_method, f, a, b, epsilon)

print("Метод прямокутників:")
print("Обчислене значення:", rect_result)
print("Кількість ітерацій:", rect_n)
print("Похибка:", abs(true_value - rect_result))
print("Ітерації:")
for n, value in rect_iterations:
    print(f"n = {n}, Значення = {value}")

print("\nМетод трапецій:")
print("Обчислене значення:", trap_result)
print("Кількість ітерацій:", trap_n)
print("Похибка:", abs(true_value - trap_result))
print("Ітерації:")
for n, value in trap_iterations:
    print(f"n = {n}, Значення = {value}")

print("\nМетод парабол (Сімпсона):")
print("Обчислене значення:", simp_result)
print("Кількість ітерацій:", simp_n)
print("Похибка:", abs(true_value - simp_result))
print("Ітерації:")
for n, value in simp_iterations:
    print(f"n = {n}, Значення = {value}")


plt.figure(figsize=(12, 8))

n_values_rect = [n for n, _ in rect_iterations]
values_rect = [value for _, value in rect_iterations]
plt.plot(n_values_rect, values_rect, marker='o', label='Метод прямокутників')

n_values_trap = [n for n, _ in trap_iterations]
values_trap = [value for _, value in trap_iterations]
plt.plot(n_values_trap, values_trap, marker='s', label='Метод трапецій')

n_values_simp = [n for n, _ in simp_iterations]
values_simp = [value for _, value in simp_iterations]
plt.plot(n_values_simp, values_simp, marker='D', label='Метод парабол')

plt.axhline(true_value, color='red', linestyle='--', label='Точне значення')
plt.xlabel('Кількість поділок (n)')
plt.ylabel('Значення інтегралу')
plt.legend()
plt.grid(True)
plt.xscale('log')
plt.yscale('log')
plt.show()
