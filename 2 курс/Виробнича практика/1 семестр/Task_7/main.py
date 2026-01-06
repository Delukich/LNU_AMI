def ln_series_approx(x, epsilon):
    assert abs(x) > 1

    sum_ln = 0
    n = 0

    while True:
        term = 1 / (x ** (2 * n + 1) * (2 * n + 1))
        sum_ln += term
        if abs(term) < epsilon:
            break
        n += 1

    return 2 * sum_ln


def input_x():
    while True:
        x = float(input("Введіть значення x: "))
        if abs(x) > 1:
            return x
        else:
            print("Помилка: значення x має бути більше за 1 за модулем")


def input_epsilon():
    while True:
        epsilon = float(input("Введіть значення epsilon: "))
        if epsilon > 0:
            return epsilon
        else:
            print("Помилка: epsilon має бути більше за 0")


x = input_x()
epsilon = input_epsilon()

ln_approx = ln_series_approx(x, epsilon)
print(f"Відповідь: {ln_approx}")
