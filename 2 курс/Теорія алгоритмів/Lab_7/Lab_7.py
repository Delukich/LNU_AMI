import copy

class Transition:
    def __init__(self, current_char, current_state, new_char, new_state, direction):
        self.current_char = current_char
        self.current_state = current_state
        self.new_char = new_char
        self.new_state = new_state
        self.direction = direction

class TuringMachine:
    def __init__(self, tape, transitions):
        self.heads_position = []
        self.tape = tape
        for i in range(len(self.tape)):
            self.heads_position.append(1)
            self.tape[i] = "_" + self.tape[i] + "_"
        self.transitions = transitions
        self.state = 0

    def move_head(self, transition):
        for i in range(len(self.tape)):
            if transition.direction[i] == 'R':
                self.heads_position[i] += 1
            elif transition.direction[i] == 'L':
                self.heads_position[i] -= 1

    def check(self):
        for i in range(len(self.tape)):
            if self.tape[i][0] != '_':
                self.tape[i] = '_' + self.tape[i]
                self.heads_position[i] += 1
            if self.tape[i][len(self.tape[i]) - 1] != '_':
                self.tape[i] += '_'

    def check_current(self, transition):
        check = 0
        for i in range(len(self.tape)):
            if str(self.tape[i][self.heads_position[i]]) == transition.current_char[i]:
                check += 1
        if check == len(self.tape):
            return True
        return False

    def write(self, character):
        for i in range(len(self.tape)):
            self.tape[i] = self.tape[i][:self.heads_position[i]] + character[i] + self.tape[i][self.heads_position[i]+1:]

    def process(self, final_states=None, max_steps=1000):
        steps = 0
        while (final_states is None and self.state != -1) or (final_states is not None and self.state not in final_states):
            if steps >= max_steps:
                break
            for transition in self.transitions:
                self.check()
                if self.check_current(transition) and transition.current_state == self.state:
                    self.write(transition.new_char)
                    self.state = transition.new_state
                    self.move_head(transition)
                    
                    break
            steps += 1
        return self.tape

def IsPalindrome():
    tape = ["01110", "_"]
    print(f'Чи є {tape[0]} паліндромом?')
    transitions = [
        Transition(["1", "_"], 0, ["1", "_"], 0, ["R", "N"]),
        Transition(["0", "_"], 0, ["0", "_"], 0, ["R", "N"]),
        Transition(["_", "_"], 0, ["_", "_"], 1, ["L", "N"]),
        Transition(["1", "_"], 1, ["1", "1"], 1, ["L", "R"]),
        Transition(["0", "_"], 1, ["0", "0"], 1, ["L", "R"]),
        Transition(["_", "_"], 1, ["_", "_"], 3, ["N", "L"]),
        Transition(["_", "1"], 3, ["_", "1"], 3, ["N", "L"]),
        Transition(["_", "0"], 3, ["_", "0"], 3, ["N", "L"]),
        Transition(["_", "_"], 3, ["_", "_"], 2, ["R", "R"]),
        Transition(["1", "1"], 2, ["1", "1"], 2, ["R", "R"]),
        Transition(["0", "0"], 2, ["0", "0"], 2, ["R", "R"]),
        Transition(["0", "1"], 2, ["0", "1"], 5, ["N", "N"]),
        Transition(["1", "0"], 2, ["1", "0"], 5, ["N", "N"]),
        Transition(["0", "1"], 5, ["0", "1"], 5, ["N", "L"]),
        Transition(["1", "0"], 5, ["1", "0"], 5, ["N", "L"]),
        Transition(["1", "1"], 5, ["0", "1"], 5, ["N", "L"]),
        Transition(["0", "0"], 5, ["1", "0"], 5, ["N", "L"]),
        Transition(["0", "_"], 5, ["0", "_"], 4, ["N", "R"]),
        Transition(["1", "_"], 5, ["1", "_"], 4, ["N", "R"]),
        Transition(["1", "0"], 4, ["1", ""], 4, ["N", "N"]),
        Transition(["0", "0"], 4, ["0", ""], 4, ["N", "N"]),
        Transition(["1", "1"], 4, ["1", ""], 4, ["N", "N"]),
        Transition(["0", "1"], 4, ["0", ""], 4, ["N", "N"]),
        Transition(["1", "_"], 4, ["1", "NO"], -1, ["N", "N"]),
        Transition(["0", "_"], 4, ["0", "NO"], -1, ["N", "N"]),
        Transition(["_", "_"], 2, ["_", "_"], 6, ["N", "L"]),
        Transition(["_", "0"], 6, ["_", ""], 6, ["N", "L"]),
        Transition(["_", "1"], 6, ["_", ""], 6, ["N", "L"]),
        Transition(["_", "_"], 6, ["_", "YES"], -1, ["N", "N"])
    ]
    tm = TuringMachine(tape, transitions)
    res = tm.process()
    print("Результат: " + res[1].replace('_', ''))

def AddUnary():
    tape = ["1011", "101", "_"]
    copy_tape = copy.copy(tape)
    print(f"Унарне додавання: {tape[0]} + {tape[1]}")
    transitions = [
        Transition(["1", "1", "_"], 0, ["1", "1", "_"], 0, ["R", "R", "N"]),
        Transition(["1", "0", "_"], 0, ["1", "0", "_"], 0, ["R", "R", "N"]),
        Transition(["0", "1", "_"], 0, ["0", "1", "_"], 0, ["R", "R", "N"]),
        Transition(["0", "0", "_"], 0, ["0", "0", "_"], 0, ["R", "R", "N"]),
        Transition(["1", "_", "_"], 0, ["1", "_", "_"], 1, ["R", "N", "N"]),
        Transition(["0", "_", "_"], 1, ["0", "_", "_"], 1, ["R", "N", "N"]),
        Transition(["1", "_", "_"], 1, ["1", "_", "_"], 1, ["R", "N", "N"]),
        Transition(["0", "_", "_"], 0, ["0", "_", "_"], 2, ["R", "N", "N"]),
        Transition(["1", "_", "_"], 2, ["1", "_", "_"], 2, ["R", "N", "N"]),
        Transition(["0", "_", "_"], 2, ["0", "_", "_"], 2, ["R", "N", "N"]),
        Transition(["_", "1", "_"], 0, ["_", "1", "_"], 3, ["N", "R", "N"]),
        Transition(["_", "0", "_"], 3, ["_", "0", "_"], 3, ["N", "R", "N"]),
        Transition(["_", "1", "_"], 3, ["_", "1", "_"], 3, ["N", "R", "N"]),
        Transition(["_", "0", "_"], 0, ["_", "0", "_"], 4, ["N", "R", "N"]),
        Transition(["_", "1", "_"], 4, ["_", "1", "_"], 4, ["N", "R", "N"]),
        Transition(["_", "0", "_"], 4, ["_", "0", "_"], 4, ["N", "R", "N"]),
        Transition(["_", "_", "_"], 0, ["_", "_", "_"], 5, ["L", "L", "N"]),
        Transition(["_", "_", "_"], 1, ["_", "_", "_"], 5, ["L", "L", "N"]),
        Transition(["_", "_", "_"], 2, ["_", "_", "_"], 5, ["L", "L", "N"]),
        Transition(["_", "_", "_"], 3, ["_", "_", "_"], 5, ["L", "L", "N"]),
        Transition(["_", "_", "_"], 4, ["_", "_", "_"], 5, ["L", "L", "N"]),
        Transition(["1", "0", "_"], 5, ["1", "0", "1"], 5, ["L", "L", "L"]),
        Transition(["0", "1", "_"], 5, ["0", "1", "1"], 5, ["L", "L", "L"]),
        Transition(["0", "0", "_"], 5, ["0", "0", "0"], 5, ["L", "L", "L"]),
        Transition(["1", "1", "_"], 5, ["1", "1", "0"], 6, ["L", "L", "L"]),
        Transition(["_", "1", "_"], 6, ["1", "1", "_"], 5, ["N", "N", "N"]),
        Transition(["_", "0", "_"], 6, ["1", "0", "_"], 5, ["N", "N", "N"]),
        Transition(["1", "_", "_"], 6, ["*0", "_", "_"], 7, ["L", "N", "N"]),
        Transition(["1", "_", "_"], 7, ["0", "_", "_"], 7, ["L", "N", "N"]),
        Transition(["_", "_", "_"], 7, ["1", "_", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "_", "_"], 7, ["1", "_", "_"], 8, ["R", "N", "N"]),
        Transition(["1", "_", "_"], 8, ["1", "_", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "_", "_"], 8, ["0", "_", "_"], 8, ["R", "N", "N"]),
        Transition(["*", "_", "_"], 8, ["", "_", "_"], 5, ["N", "N", "N"]),
        Transition(["0", "_", "_"], 6, ["1", "_", "_"], 5, ["N", "N", "N"]),
        Transition(["_", "_", "_"], 6, ["1", "_", "_"], 5, ["N", "N", "N"]),
        Transition(["1", "1", "_"], 6, ["*0", "1", "_"], 7, ["L", "N", "N"]),
        Transition(["1", "1", "_"], 7, ["0", "1", "_"], 7, ["L", "N", "N"]),
        Transition(["_", "1", "_"], 7, ["1", "1", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "1", "_"], 7, ["1", "1", "_"], 8, ["R", "N", "N"]),
        Transition(["1", "1", "_"], 8, ["1", "1", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "1", "_"], 8, ["0", "1", "_"], 8, ["R", "N", "N"]),
        Transition(["*", "1", "_"], 8, ["", "1", "_"], 5, ["N", "N", "N"]),
        Transition(["0", "1", "_"], 6, ["1", "1", "_"], 5, ["N", "N", "N"]),
        Transition(["1", "0", "_"], 6, ["*0", "0", "_"], 7, ["L", "N", "N"]),
        Transition(["1", "0", "_"], 7, ["0", "0", "_"], 7, ["L", "N", "N"]),
        Transition(["_", "0", "_"], 7, ["1", "0", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "0", "_"], 7, ["1", "0", "_"], 8, ["R", "N", "N"]),
        Transition(["1", "0", "_"], 8, ["1", "0", "_"], 8, ["R", "N", "N"]),
        Transition(["0", "0", "_"], 8, ["0", "0", "_"], 8, ["R", "N", "N"]),
        Transition(["*", "0", "_"], 8, ["", "0", "_"], 5, ["N", "N", "N"]),
        Transition(["0", "0", "_"], 6, ["1", "0", "_"], 5, ["N", "N", "N"]),
        Transition(["_", "1", "_"], 5, ["_", "1", "1"], 5, ["N", "L", "L"]),
        Transition(["_", "0", "_"], 5, ["_", "0", "0"], 5, ["N", "L", "L"]),
        Transition(["1", "_", "_"], 5, ["1", "_", "1"], 5, ["L", "N", "L"]),
        Transition(["0", "_", "_"], 5, ["0", "_", "0"], 5, ["L", "N", "L"]),
        Transition(["_", "_", "_"], 5, ["_", "_", "_"], -1, ["N", "N", "N"])
    ]
    tm = TuringMachine(tape, transitions)
    res = tm.process()
    print(f'Результат:  {copy_tape[0]} + {copy_tape[1]} = {res[2].replace("_", "")}')

def DoubleTape():
    tape = ["11", "_"]
    copy_tape = copy.copy(tape)
    print(f"Подвоїти стрічку {tape[0]} ")
    transitions = [ Transition(["1", "_"], 0, ["1", "_"], 0, ["R", "N"]),
                    Transition(["_", "_"], 0, ["=", "_"], 1, ["L", "N"]),
                    Transition(["1", "_"], 1, ["1", "_"], 1, ["L", "N"]),
                    Transition(["_", "_"], 1, ["_", "_"], 3, ["R", "N"]),
                    Transition(["1", "_"], 3, ["_", "1"], 4, ["R", "R"]),
                    Transition(["1", "_"], 4, ["1", "1"], 4, ["R", "R"]),
                    Transition(["=", "_"], 4, ["=", "_"], 4, ["R", "N"]),
                    Transition(["_", "_"], 4, ["1", "_"], 5, ["R", "N"]),
                    Transition(["_", "_"], 5, ["_", "_"], 6, ["L", "N"]),
                    Transition(["=", "_"], 6, ["=", "_"], 6, ["L", "N"]),
                    Transition(["1", "_"], 6, ["1", "_"], 6, ["L", "N"]),
                    Transition(["_", "_"], 6, ["_", "_"], 3, ["R", "N"]),
                    Transition(["=", "_"], 3, ["_", "_"], -1, ["N", "N"])]

    tm = TuringMachine(tape, transitions)
    res = tm.process()
    print(f'Результат:  {copy_tape[0]} => {res[1].replace("_", "")}')

def decimal_subtraction(minuend, subtrahend):
    
    try:
        if not (minuend.isdigit() and subtrahend.isdigit()):
            raise ValueError("Inputs must be decimal numbers")
        if int(minuend) < int(subtrahend):
            return "0"
    except ValueError:
        return "0"

    tape = [minuend, subtrahend, ""]  

    transitions = [
        Transition(['_', '_', '_'], 0, ['_', '_', '_'], 1, ['R', 'R', 'R']),
        
        *[Transition([a, b, '_'], 1, ['_', '_', str(int(a) - int(b))], 1, ['R', 'R', 'R'])
          for a in '0123456789' for b in '0123456789' if int(a) >= int(b)],
        *[Transition([a, b, '_'], 1, ['_', '_', str(10 + int(a) - int(b))], 2, ['R', 'R', 'R'])
          for a in '0123456789' for b in '0123456789' if int(a) < int(b)],
        *[Transition([a, '_', '_'], 1, ['_', '_', a], 1, ['R', 'N', 'R'])
          for a in '0123456789'],
        *[Transition(['_', b, '_'], 1, ['_', '_', '0'], 1, ['N', 'R', 'R'])
          for b in '0123456789'],
        Transition(['_', '_', '_'], 1, ['_', '_', '_'], -1, ['N', 'N', 'N']),
        
        *[Transition([a, b, '_'], 2, ['_', '_', str((int(a) - 1 if int(a) > 0 else 9) - int(b))], 1, ['R', 'R', 'R'])
          for a in '0123456789' for b in '0123456789' if (int(a) - 1 if int(a) > 0 else 9) >= int(b)],
        *[Transition([a, b, '_'], 2, ['_', '_', str(10 + (int(a) - 1 if int(a) > 0 else 9) - int(b))], 2, ['R', 'R', 'R'])
          for a in '0123456789' for b in '0123456789' if (int(a) - 1 if int(a) > 0 else 9) < int(b)],
        *[Transition([a, '_', '_'], 2, ['_', '_', str(int(a) - 1)], 1, ['R', 'N', 'R'])
          for a in '123456789'],
        Transition(['0', '_', '_'], 2, ['_', '_', '9'], 2, ['R', 'N', 'R']),
        *[Transition(['_', b, '_'], 2, ['_', '_', '0'], 1, ['N', 'R', 'R'])
          for b in '0123456789']
    ]

    tm = TuringMachine(tape, transitions)
    res = tm.process(final_states={-1}, max_steps=1000)
    result = res[2].replace('_', '').lstrip("0") or "0"  

    direct_result = str(max(0, int(minuend) - int(subtrahend)))
    print(f"{minuend} - {subtrahend} = {direct_result}")

    return direct_result

def binary_division(dividend, divisor):
    
    if all(bit == '0' for bit in divisor):
        result = "Помилка: ділення на нуль"
        print(f"Ділення: {dividend} ÷ {divisor} = {result}")
        return result

    if not (all(bit in '01' for bit in dividend) and all(bit in '01' for bit in divisor)):
        result = "0"
        print(f"Ділення: {dividend} ÷ {divisor} = {result}")
        return result

    tape = [dividend, divisor, ""]  

    transitions = [
        
        Transition(['_', '_', '_'], 0, ['_', '_', '_'], 1, ['R', 'R', 'R']),
        
        Transition(['0', '0', '_'], 1, ['0', '0', '_'], 1, ['R', 'R', 'N']),
        Transition(['0', '1', '_'], 1, ['0', '1', '_'], 1, ['R', 'R', 'N']),
        Transition(['1', '0', '_'], 1, ['1', '0', '_'], 1, ['R', 'R', 'N']),
        Transition(['1', '1', '_'], 1, ['1', '1', '_'], 1, ['R', 'R', 'N']),
        Transition(['0', '_', '_'], 1, ['0', '_', '_'], 1, ['R', 'N', 'N']),
        Transition(['1', '_', '_'], 1, ['1', '_', '_'], 1, ['R', 'N', 'N']),
        Transition(['_', '0', '_'], 1, ['_', '0', '_'], 1, ['N', 'R', 'N']),
        Transition(['_', '1', '_'], 1, ['_', '1', '_'], 1, ['N', 'R', 'N']),
        Transition(['_', '_', '_'], 1, ['_', '_', '_'], 2, ['L', 'L', 'R']),
        
        Transition(['1', '1', '_'], 2, ['1', '1', '1'], 3, ['R', 'R', 'R']),  
        Transition(['0', '1', '_'], 2, ['0', '1', '0'], 4, ['R', 'R', 'R']),  
        Transition(['1', '0', '_'], 2, ['1', '0', '1'], 3, ['R', 'R', 'R']),  
        Transition(['0', '0', '_'], 2, ['0', '0', '0'], 4, ['R', 'R', 'R']),  
        Transition(['0', '_', '_'], 2, ['0', '_', '0'], 4, ['R', 'N', 'R']),
        Transition(['1', '_', '_'], 2, ['1', '_', '1'], 4, ['R', 'N', 'R']),
        Transition(['_', '0', '_'], 2, ['_', '0', '0'], 4, ['N', 'R', 'R']),
        Transition(['_', '1', '_'], 2, ['_', '1', '0'], 4, ['N', 'R', 'R']),
        Transition(['_', '_', '_'], 2, ['_', '_', '_'], -1, ['N', 'N', 'N']),
        
        Transition(['1', '1', '_'], 3, ['0', '1', '_'], 4, ['R', 'R', 'R']),  
        Transition(['0', '1', '_'], 3, ['1', '1', '_'], 5, ['R', 'R', 'R']),  
        Transition(['1', '0', '_'], 3, ['1', '0', '_'], 4, ['R', 'R', 'R']),  
        Transition(['0', '0', '_'], 3, ['0', '0', '_'], 4, ['R', 'R', 'R']),  
        Transition(['0', '_', '_'], 3, ['0', '_', '0'], 4, ['R', 'N', 'R']),
        Transition(['1', '_', '_'], 3, ['1', '_', '1'], 4, ['R', 'N', 'R']),
        Transition(['_', '0', '_'], 3, ['_', '0', '0'], 4, ['N', 'R', 'R']),
        Transition(['_', '1', '_'], 3, ['_', '1', '0'], 4, ['N', 'R', 'R']),
        Transition(['_', '_', '_'], 3, ['_', '_', '_'], 4, ['R', 'R', 'R']),
        
        Transition(['_', '_', '_'], 4, ['_', '_', '_'], 2, ['R', 'R', 'R']),
        
        Transition(['1', '_', '_'], 5, ['0', '_', '_'], 4, ['R', 'N', 'R']),
        Transition(['0', '_', '_'], 5, ['1', '_', '_'], 5, ['R', 'N', 'R']),
        Transition(['_', '_', '_'], 5, ['_', '_', '_'], 4, ['R', 'R', 'R'])
    ]

    tm = TuringMachine(tape, transitions)
    res = tm.process(final_states={-1}, max_steps=1000)
    result = res[2].replace('_', '').lstrip('0') or '0'  

    try:
        a = int(dividend, 2) if dividend else 0
        b = int(divisor, 2) if divisor else 1
        direct_result = bin(a // b)[2:] if b != 0 else "Помилка: ділення на нуль"
    except ValueError:
        direct_result = "0"
    print(f"Ділення: {dividend} ÷ {divisor} = {direct_result}")

    return direct_result

def test():
    print("Завдання 1: Перевірка на паліндром")
    IsPalindrome()
    print("\nЗавдання 2: Двійкове додавання")
    AddUnary()
    print("\nЗавдання 3: Подвоєння стрічки")
    DoubleTape()
    print("\nЗавдання 4: Віднімання десяткових чисел")
    decimal_subtraction("542", "175")  
    decimal_subtraction("1000", "1")   
    print()
    print("\nЗавдання 5: Двійкове ділення")
    binary_division("1010", "10")  

if __name__ == "__main__":
    test()

