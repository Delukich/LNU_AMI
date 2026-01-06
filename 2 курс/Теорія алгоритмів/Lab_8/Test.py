import sys

class RAM:
    def __init__(self):
        self.registers = [0] * 100
        self.memory = [0] * 1000
        self.pc = 0
        self.input_buffer = []
        self.output = []
        self.debug = False

    def read_input(self, prompt="Введіть значення: "):
        if self.input_buffer:
            return self.input_buffer.pop(0)
        try:
            value = input(prompt)
            return value
        except EOFError:
            raise ValueError("Помилка: ввід завершено передчасно")

    def write_output(self, value):
        self.output.append(str(value))

    def execute(self, program):
        self.pc = 0
        while 0 <= self.pc < len(program):
            instr = program[self.pc]
            if self.debug:
                print(f"PC={self.pc}, Instr={instr}, Registers={self.registers[:10]}, Memory={self.memory[:10]}")
            self.pc += 1
            self._execute_instruction(instr)

    def _execute_instruction(self, instr):
        op, *args = instr
        if op == "READ":
            reg = args[0]
            try:
                self.registers[reg] = int(self.read_input())
            except ValueError:
                raise ValueError(f"Помилка: очікувалося ціле число для READ у регістр R{reg}")
        elif op == "WRITE":
            reg = args[0]
            self.write_output(self.registers[reg])
        elif op == "LOAD":
            reg, value = args
            self.registers[reg] = value
        elif op == "LOADM":
            reg, mem = args
            self.registers[reg] = self.memory[self.registers[mem]]
        elif op == "STOREM":
            mem, reg = args
            self.memory[self.registers[mem]] = self.registers[reg]
        elif op == "ADD":
            reg1, reg2, reg3 = args
            self.registers[reg1] = self.registers[reg2] + self.registers[reg3]
        elif op == "SUB":
            reg1, reg2, reg3 = args
            self.registers[reg1] = self.registers[reg2] - self.registers[reg3]
        elif op == "MUL":
            reg1, reg2, reg3 = args
            self.registers[reg1] = self.registers[reg2] * self.registers[reg3]
        elif op == "JMP":
            label = args[0]
            self.pc = label
        elif op == "JGT":
            reg, label = args
            if self.registers[reg] > 0:
                self.pc = label
        elif op == "JEQ":
            reg1, reg2, label = args
            if self.registers[reg1] == self.registers[reg2]:
                self.pc = label

class RASP(RAM):
    def __init__(self):
        super().__init__()

    def _execute_instruction(self, instr):
        op, *args = instr
        if op == "JLT":
            reg, label = args
            if self.registers[reg] < 0:
                self.pc = label
        else:
            super()._execute_instruction(instr)

def task1_program():
    program = [
        ("READ", 0),
        ("JGT", 0, 2),
        ("JMP", 999),
        ("LOAD", 1, 0),
        ("SUB", 2, 0, 1),
        ("JGT", 2, 6),
        ("JMP", 12),
        ("READ", 3),
        ("JGT", 3, 9),
        ("JMP", 999),
        ("STOREM", 1, 3),
        ("LOAD", 4, 1),
        ("ADD", 1, 1, 4),
        ("JMP", 4),
        ("LOAD", 1, 1),
        ("SUB", 2, 0, 1),
        ("JGT", 2, 15),
        ("JMP", 31),
        ("LOADM", 3, 1),
        ("SUB", 4, 1, 5),
        ("LOAD", 5, 1),
        ("JGT", 4, 19),
        ("JMP", 26),
        ("LOADM", 6, 4),
        ("SUB", 7, 6, 3),
        ("JGT", 7, 23),
        ("JMP", 26),
        ("ADD", 8, 4, 5),
        ("STOREM", 8, 6),
        ("SUB", 4, 4, 5),
        ("JGT", 4, 19),
        ("ADD", 8, 4, 5),
        ("STOREM", 8, 3),
        ("ADD", 1, 1, 5),
        ("SUB", 2, 0, 1),
        ("JGT", 2, 15),
        ("LOAD", 1, 0),
        ("SUB", 2, 0, 1),
        ("JGT", 2, 34),
        ("JMP", 999),
        ("LOADM", 3, 1),
        ("WRITE", 3),
        ("ADD", 1, 1, 5),
        ("JMP", 32),
    ]
    return program

print("Завдання 1: Сортування чисел (РАМ-реалізація)")
inputs = [
    [3, 0, 4, 1, 2],
    [3, -1, 4, 1, 2],
    [4, 130, 48, 10, 2],
    [1, 5],
    [0],
    ["abc"],
]
for i, input_data in enumerate(inputs, 1):
    print(f"\nТест {i}: {input_data}")
    ram = RAM()
    ram.input_buffer = [str(x) for x in input_data]
    ram.output = []
    ram.memory = [0] * 1000
    try:
        ram.execute(task1_program())
        print(f"Відсортований масив: {' '.join(ram.output)}")
    except ValueError as e:
        print(f"Помилка: {str(e)}")

def task2_program(s):
    def preprocess_input(s):
        return [(ord(c) - ord('0')) for c in s] if s else []
    
    input_data = preprocess_input(s)
    program = [
        ("LOAD", 0, 0),
        ("LOAD", 1, 0),
        ("LOAD", 2, 0),
        ("LOAD", 3, len(input_data)),
        ("SUB", 4, 3, 0),
        ("JGT", 4, 6),
        ("JMP", 14),
        ("LOADM", 5, 0),
        ("LOAD", 6, 1),
        ("JEQ", 5, 6, 10),
        ("JMP", 14),
        ("ADD", 1, 1, 6),
        ("ADD", 0, 0, 6),
        ("JMP", 4),
        ("SUB", 4, 3, 0),
        ("JGT", 4, 15),
        ("JMP", 22),
        ("LOADM", 5, 0),
        ("LOAD", 6, 2),
        ("JEQ", 5, 6, 19),
        ("JMP", 22),
        ("ADD", 2, 2, 6),
        ("ADD", 0, 0, 6),
        ("JMP", 14),
        ("SUB", 4, 3, 0),
        ("LOAD", 6, 1),
        ("JEQ", 4, 6, 25),
        ("JMP", 29),
        ("LOADM", 5, 0),
        ("LOAD", 6, 0),
        ("JEQ", 5, 6, 27),
        ("JMP", 29),
        ("JEQ", 1, 2, 30),
        ("JMP", 29),
        ("LOAD", 7, "Invalid"),
        ("WRITE", 7),
        ("JMP", 999),
        ("LOAD", 7, "Valid"),
        ("WRITE", 7),
    ]
    return program, input_data

print("\nЗавдання 2: Перевірка 1^n 2^n^2 0")
test_strings = ["1122220", "11220", "110", "111222220", "1112220", "", "123"]
for i, s in enumerate(test_strings, 1):
    print(f"\nТест {i}: {s}")
    ram = RAM()
    ram.input_buffer = []
    ram.output = []
    try:
        program, input_data = task2_program(s)
        ram.memory[:len(input_data)] = input_data
        ram.execute(program)
        print(f"Результат: {ram.output[0] if ram.output else 'Invalid'}")
    except ValueError as e:
        print(f"Помилка: {str(e)}")

def task3_program():
    program = [
        ("READ", 0),
        ("LOAD", 1, 1),
        ("LOAD", 2, 0),
        ("LOAD", 3, 2),
        ("JGT", 3, 5),
        ("JMP", 14),
        ("LOAD", 4, 1),
        ("SUB", 5, 3, 4),
        ("LOAD", 6, 2),
        ("MUL", 7, 5, 6),
        ("JEQ", 7, 5, 10),
        ("JMP", 11),
        ("MUL", 1, 1, 2),
        ("MUL", 2, 2, 2),
        ("LOAD", 8, 2),
        ("SUB", 3, 3, 8),
        ("JGT", 3, 5),
        ("MUL", 9, 1, 0),
        ("WRITE", 9),
    ]
    return program

print("\nЗавдання 3: Обчислення n^n")
test_inputs = [5, 0, 3, -1, 10, "abc"]
for i, n in enumerate(test_inputs, 1):
    print(f"\nТест {i}: n={n}")
    ram = RAM()
    ram.input_buffer = [str(n)]
    ram.output = []
    try:
        ram.execute(task3_program())
        print(f"n^3 = {ram.output[0]}")
    except ValueError as e:
        print(f"Помилка: {str(e)}")

def task4_program():
    program = [
        ("READ", 0),
        ("LOAD", 1, 0),
        ("LOAD", 2, 0),
        ("SUB", 3, 0, 2),
        ("JGT", 3, 5),
        ("JMP", 11),
        ("READ", 4),
        ("STOREM", 1, 4),
        ("LOAD", 5, 1),
        ("ADD", 1, 1, 5),
        ("ADD", 2, 2, 5),
        ("JMP", 3),
        ("LOAD", 2, 0),
        ("SUB", 3, 0, 2),
        ("JGT", 3, 14),
        ("JMP", 999),
        ("LOADM", 4, 2),
        ("WRITE", 4),
        ("ADD", 2, 2, 5),
        ("JMP", 12),
    ]
    return program

print("\nЗавдання 4: Введення масиву з непрямою адресацією")
inputs = [
    [4, 10, 20, 30, 40],
    [3, 11, 22, 33],
    [0],
    [3, 17, 89, 70],
    [2, -5, 10],
    ["abc"],
]
for i, input_data in enumerate(inputs, 1):
    print(f"\nТест {i}: {input_data}")
    ram = RAM()
    ram.input_buffer = [str(x) for x in input_data]
    ram.output = []
    ram.memory = [0] * 1000
    try:
        ram.execute(task4_program())
        print(f"Введений масив: {' '.join(ram.output)}")
    except ValueError as e:
        print(f"Помилка: {str(e)}")

def task5_program():
    program = [
        ("READ", 0),
        ("LOAD", 1, 0),
        ("LOAD", 2, 0),
        ("SUB", 3, 0, 2),
        ("JGT", 3, 5),
        ("JMP", 11),
        ("READ", 4),
        ("JLT", 4, 8),
        ("JMP", 9),
        ("LOAD", 5, 1),
        ("ADD", 1, 1, 5),
        ("ADD", 2, 2, 5),
        ("JMP", 3),
        ("WRITE", 1),
    ]
    return program

print("\nЗавдання 5: Підрахунок від’ємних чисел (РАСП)")
inputs = [
    [6, 1, -2, 3, -4, -5, 6],
    [6, 0, 7, 89, 6, 1, 4],
    [6, 0, -2, -8, 6, 1, 4],
    [0],
    [3, -1, -2, -3],
    [2, "abc", 5],
]
for i, input_data in enumerate(inputs, 1):
    print(f"\nТест {i}: {input_data}")
    rasp = RASP()
    rasp.input_buffer = [str(x) for x in input_data]
    rasp.output = []
    try:
        rasp.execute(task5_program())
        print(f"Кількість від’ємних чисел: {rasp.output[0]}")
    except ValueError as e:
        print(f"Помилка: {str(e)}")