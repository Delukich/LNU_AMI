import numpy as np
import matplotlib.pyplot as plt

x_value = np.array([0, 0.25, 1.25, 2.12, 3.25])
y_value = np.array([2, 1.6, 2.32, 2.02, 2.83])
x = 2.5

def lagrange(x, x_val, y_val):
    result = 0
    n = len(x_val)

    for i in range(n):
        L_i = y_val[i]
        for j in range(n):
            if i != j:
                L_i *= (x - x_val[j]) / (x_val[i] - x_val[j])
        result += L_i

    return result

result = lagrange(x, x_value, y_value)

x_range = np.linspace(min(x_value), max(x_value), 100)
y_range = lagrange(x_range, x_value, y_value)

plt.plot(x_range, y_range, 'b-')
plt.plot(x_value, y_value, 'ro')
plt.plot(x, result, 'go')
plt.axvline(x=x, color='green', linestyle='--')
plt.xlabel('x')
plt.ylabel('y')
plt.title('Інтерполяція Лагранжа')
plt.grid(True)
plt.show()

print(f"Result: {result}")
