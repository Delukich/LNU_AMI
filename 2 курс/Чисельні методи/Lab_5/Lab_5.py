import numpy as np
import matplotlib.pyplot as plt

# Лінійний сплайн
def linear_spline(x, y, x_interp):
    for i in range(len(x) - 1):
        if x[i] <= x_interp <= x[i + 1]:
            return y[i] + (y[i + 1] - y[i]) / (x[i + 1] - x[i]) * (x_interp - x[i])
    return None

x_points = np.array([0, 1, 2])
y_points = np.array([0, 1, 4])
x_interp = 1.5
y_interp = linear_spline(x_points, y_points, x_interp)
print(f"Значення функції у точці x={x_interp}: y={y_interp}")

# Кубічний сплайн
def cubic_spline(x, y, x_interp):
    n = len(x)
    a = y
    h = np.diff(x)
    alpha = [0] + [3/h[i] * (a[i+1] - a[i]) - 3/h[i-1] * (a[i] - a[i-1]) for i in range(1, n-1)]
    l = [1] + [2 * (x[i+1] - x[i-1]) for i in range(1, n-1)] + [1]
    mu = [0] + [h[i] / (2 * (x[i+1] - x[i-1])) for i in range(1, n-1)]
    z = [0] * n

    for i in range(1, n-1):
        l[i] = l[i] - mu[i-1] * h[i-1]
        alpha[i] = alpha[i] - mu[i-1] * alpha[i-1]
        mu[i] = h[i] / l[i]

    for i in range(n-2, 0, -1):
        z[i] = (alpha[i] - h[i] * z[i+1]) / l[i]

    b = [(a[i+1] - a[i]) / h[i] - h[i] * (z[i+1] + 2 * z[i]) / 3 for i in range(n-1)]
    c = z[:-1]
    d = [(z[i+1] - z[i]) / (3 * h[i]) for i in range(n-1)]

    for i in range(len(x) - 1):
        if x[i] <= x_interp <= x[i + 1]:
            dx = x_interp - x[i]
            return y[i] + b[i] * dx + c[i] * dx**2 + d[i] * dx**3
    return None

x2 = np.array([0, 1, 3, 4])
y2 = np.array([0, 1, 2, 1])
x_interp2 = 2.5
y_interp2 = cubic_spline(x2, y2, x_interp2)
print(f"Значення функції при кубічному сплайні у x={x_interp2}: {y_interp2}")

# Похибка між лінійним і кубічним сплайнами
x3 = np.array([0, np.pi / 2, np.pi])
y3 = np.sin(x3)

x_dense = np.linspace(0, np.pi, 1000)
y_real = np.sin(x_dense)

y_linear = np.array([linear_spline(x3, y3, xi) for xi in x_dense])

y_cubic = np.array([cubic_spline(x3, y3, xi) for xi in x_dense])

error = np.max(np.abs(y_linear - y_cubic))
print(f"Максимальна похибка між лінійним та кубічним сплайнами: {error}")

plt.figure(figsize=(10, 6))
plt.plot(x_dense, y_real, label="sin(x)", color='black')
plt.plot(x_dense, y_linear, label="Лінійний сплайн", linestyle='--')
plt.plot(x_dense, y_cubic, label="Кубічний сплайн", linestyle=':')
plt.scatter(x3, y3, color='red', zorder=5, label="Вузли інтерполяції")
plt.legend()
plt.xlabel('x')
plt.ylabel('y')
plt.title('Порівняння лінійного та кубічного сплайнів')
plt.show()
