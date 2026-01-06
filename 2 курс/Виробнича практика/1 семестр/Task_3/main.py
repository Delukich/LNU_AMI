import math

def find_root(a):
    if a > 0:

        def f(x):
            return 2 * a * x + abs(a - 1)

        if a != 0:
            root = -abs(a - 1) / (2 * a)
        else:
            root = None

    else:
        def f(x):
            return math.exp(x) / math.sqrt(1 + a) - 1

        if 1 + a > 0:
            root = math.log(math.sqrt(1 + a))
        else:
            root = None

    return root

a = float(input("Enter number a: "))
root = find_root(a)
if root is not None:
    print(root)
else:
    print("Error")
