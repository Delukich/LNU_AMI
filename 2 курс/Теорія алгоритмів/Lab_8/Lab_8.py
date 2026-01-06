def task1_programs(memory):
    # Перевірка на невід'ємність
    for num in memory:
        if num < 0:
            print("Число має бути невід'ємним!")
            return
    
    # Сортування вставками
    for i in range(1, len(memory)):
        key = memory[i]
        j = i - 1
        while j >= 0 and memory[j] > key:
            memory[j + 1] = memory[j]
            j -= 1
        memory[j + 1] = key
    
    print("Відсортовані числа:", memory)


def task2_programs(s):
    # Регістри
    count_1 = 0
    count_2 = 0
    i = 0
    
    # Підрахунок 1
    while i < len(s) and s[i] == '1':
        count_1 += 1
        i += 1
    
    # Підрахунок 2
    while i < len(s) and s[i] == '2':
        count_2 += 1
        i += 1
    
    # Перевірка формату
    if i == len(s) - 1 and s[i] == '0' and count_2 == count_1 ** 2:
        print("Послідовність відповідає формату 1^n 2^(n^2) 0")
    else:
        print("Послідовність НЕ відповідає формату 1^n 2^(n^2) 0")


def task3_programs(n):
    # Швидке піднесення до степеня
    def fast_power(base, exp):
        if exp < 0:  # Якщо показник степеня від'ємний
            base = 1 / base
            exp = -exp
        result = 1
        while exp > 0:
            if exp % 2 == 1:
                result *= base
            base *= base
            exp //= 2
        return result
    
    # Обчислення n^n
    if n == 0:
        print(f"{n}^{n} = 1")  # Визначення 0^0 як 1
    else:
        n_power_n = fast_power(n, n)  # n^n
        print(f"{n}^{n} = {n_power_n}")


def task4_programs(input_values):
    # Імітація пам'яті
    n = len(input_values)
    memory = [0] * n
    pointer = 0  # Регістр-вказівник
    
    # Збереження через "непряму адресацію"
    for value in input_values:
        memory[pointer] = value
        pointer += 1
    
    print("Введений масив:", memory)


def task5_programs(numbers):
    # Лічильник
    count = 0
    
    # Підрахунок від’ємних
    for num in numbers:
        if num < 0:
            count += 1
    
    print("Кількість від’ємних чисел:", count)