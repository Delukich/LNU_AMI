import numpy as np

class Matrix:
    def __init__(self, rows, cols, data=None):
        self.rows = rows
        self.cols = cols
        if data is None:
            self.matrix = np.zeros((rows, cols))
        else:
            self.matrix = np.array(data).reshape(rows, cols)

    def __add__(self, other):
        if self.rows == other.rows and self.cols == other.cols:
            return Matrix(self.rows, self.cols, self.matrix + other.matrix)
        else:
            raise ValueError("Розміри матриць повинні бути однаковими для додавання")

    def __sub__(self, other):
        if self.rows == other.rows and self.cols == other.cols:
            return Matrix(self.rows, self.cols, self.matrix - other.matrix)
        else:
            raise ValueError("Розміри матриць повинні бути однаковими для віднімання")

    def __mul__(self, other):
        if isinstance(other, Matrix):
            if self.cols == other.rows:
                return Matrix(self.rows, other.cols, np.dot(self.matrix, other.matrix))
            else:
                raise ValueError("Несумісні розміри матриць для множення")
        elif isinstance(other, (int, float)):
            return Matrix(self.rows, self.cols, self.matrix * other)
        else:
            raise ValueError("Непідтримуване множення")

    def __eq__(self, other):
        return np.array_equal(self.matrix, other.matrix)

    def __str__(self):
        result = ""
        for row in self.matrix:
            result += " ".join([f"{elem:.2f}" for elem in row]) + "\n"
        return result

    @staticmethod
    def input_matrix():
        while True:
            try:
                rows = int(input("Введіть кількість рядків: "))
                cols = int(input("Введіть кількість стовпців: "))
                if rows <= 0 or cols <= 0:
                    raise ValueError("Кількість рядків і стовпців повинна бути додатною")
                data = []
                print(f"Введіть елементи матриці {rows}x{cols} по рядках: ")
                for i in range(rows):
                    row = list(map(float, input(f"Рядок {i + 1}: ").split()))
                    if len(row) != cols:
                        raise ValueError(f"Рядок {i + 1} має неправильну кількість елементів. Має бути {cols}")
                    data.append(row)
                return Matrix(rows, cols, data)
            except ValueError as e:
                print(f"Помилка: {e}. Спробуйте ще раз")

    def multiply_vector(self, vector):
        vector = np.array(vector)
        if self.cols == len(vector):
            return np.dot(self.matrix, vector)
        else:
            raise ValueError("Кількість стовпців матриці повинна відповідати довжині вектора")

    def print_matrix(self):
        for row in self.matrix:
            print(" ".join([f"{elem:.2f}" for elem in row]))

if __name__ == "__main__":
    try:
        A = Matrix.input_matrix()
        B = Matrix.input_matrix()

        while True:
            try:
                x = list(map(float, input(f"Введіть вектор x (довжина повинна бути {A.cols}): ").split()))
                if len(x) != A.cols:
                    raise ValueError(f"Вектор x повинен мати довжину {A.cols}")
                break
            except ValueError as e:
                print(f"Помилка: {e} Спробуйте ще раз")

        if A == B:
            result = 4 * A.multiply_vector(x)
        else:
            result = (A + B).multiply_vector(x)

        print("\nСума матриць A та B:")
        (A + B).print_matrix()

        print("\nРезультат обчислення: ")
        print(result)

    except Exception as e:
        print(f"Сталася помилка: {e}")