def find_first_term(x, e):
    a_n_minus_1 = x
    n = 0

    while True:

        a_n = a_n_minus_1 + x / (4 + a_n_minus_1 ** 2)

        if abs(a_n - a_n_minus_1) < e:
            return n, a_n

        a_n_minus_1 = a_n
        print(f"Root: {a_n}, Iterations: {n}")
        n += 1

x = float(input("Enter a x: "))
e = float(input("Enter a e: "))

iteration, root = find_first_term(x, e)
print(f"\nResult: root: {root}, Iterations: {iteration}")


