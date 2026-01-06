import numpy as np

matrix = [
    [1, 2, 3, 4],
    [5, 6, 7, 8],
    [9, 10, 11, 12],
    [13, 14, 15, 16]
]

matrix = np.array(matrix)

rows, cols = matrix.shape
if rows != cols:
    print("Помилка: Матриця не є квадратною!")
else:
    if rows % 2 != 0:
        print("Помилка: Матриця має непарний порядок!")
    else:
        n = rows // 2

        A = matrix[:n, :n]
        B = matrix[:n, n:]
        C = matrix[n:, :n]
        D = matrix[n:, n:]

        new_matrix = np.block([[D, C], [B, A]])

        print("Нова матриця після перестановки блоків:")
        print(new_matrix)
