import unittest
import time

class Time:
    def __init__(self, func):
        self.func = func

    @staticmethod
    def log_time(func):
        def wrapper(*args, **kwargs):
            start_time = time.time()
            result = func(*args, **kwargs)
            end_time = time.time()
            print(f"Time: {end_time - start_time}")
            return result

        return wrapper

def generate(n):
    a, b = 0, 1
    for i in range(n):
        yield a
        a, b = b, a + b

with open('output.txt', 'w') as file:
    for i in generate(10):
        file.write(str(i) + '\n')

@Time.log_time
def test_generate(n, filename):
    with open(filename, 'w') as file:
        for i in generate(n):
            file.write(str(i) + '\n')

test_generate(10, 'output.txt')

class TestGenerate(unittest.TestCase):
    def test_generate(self):
        self.assertEqual(list(generate(10)), [0, 1, 1, 2, 3, 5, 8, 13, 21, 34])
        print("Все працює!")

    def test_one_appears_twice(self):
        sequence = list(generate(10))
        self.assertEqual(sequence.count(1), 2)
        print("Все добре!")

