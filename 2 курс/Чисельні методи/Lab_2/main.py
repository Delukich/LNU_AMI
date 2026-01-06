#Завдання №14. Метод Зейделя. Лук'янчук Денис
A = [[76, 21, 6, -34],
     [12, -114, 8, 9],
     [16, 24, -100, -35],
     [23, -8, 5, -75]]

b = [-142, 83, -121, 85]

epsilon = 0.0001

x = [0, 0, 0, 0]

def check(A):
    for i in range(len(A)):
        row_sum = sum(abs(A[i][j]) for j in range(len(A[i])) if i != j)
        if abs(A[i][i]) <= row_sum:
            return False
    return True

def zeidel(A, b, epsilon):
    n = len(A)
    x_new = x.copy()
    for iteration in range(1, 10000):
        x_old = x_new.copy()
        for i in range(n):
            sum1 = sum(A[i][j] * x_new[j] for j in range(i))
            sum2 = sum(A[i][j] * x_old[j] for j in range(i + 1, n))
            x_new[i] = (b[i] - sum1 - sum2) / A[i][i]

        if max(abs(x_new[i] - x_old[i]) for i in range(n)) < epsilon:
            print(f'Iteration: {iteration}')
            return x_new

        print(f'Iteration: {iteration}  Result: {x_new}')

    raise Exception('The method does not match')

if check(A):
    result = zeidel(A, b, epsilon)
    print(f'Result: {result}')
else:
    print('The convergence condition is not met')