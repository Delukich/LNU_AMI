import math as mat

x = float(input("Enter a x: "))
x1 = mat.radians(x)

y = float(input("Enter a y: "))
y1 = mat.radians(y)

z1 = mat.sin(x)**2 + mat.cos(y)**4 + 1/4 * mat.sin(2*x)**2 - 1

z2 = mat.cos(3*x)/mat.sin(3*x) + 6.4*y**3

print("z1 =", z1)
print("z2 =", z2)
