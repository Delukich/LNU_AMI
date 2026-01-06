import math

def is_tangent_defined(z_radians):
    z_normalized = z_radians % math.pi
    return not math.isclose(z_normalized, math.pi / 2, abs_tol=1e-9)

x = float(input("Введіть значення x: "))
y = float(input("Введіть значення y: "))
z_degrees = float(input("Введіть значення z (у градусах): "))

z_radians = math.radians(z_degrees)

if not is_tangent_defined(z_radians):
    print("Тангенс не визначений для цього значення z")
else:
    a = (3 + math.exp(y - 1)) / (1 + x**2 * abs(y - math.log(z_radians)))

    delta = abs(y - x)
    b = 1 + delta + (delta ** 2) / 2 + (delta ** 3) / 3

    print(f"Значення a: {a}")
    print(f"Значення b: {b}")
