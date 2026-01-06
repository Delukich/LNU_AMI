class TuringMachine:
    def __init__(self, tape, rules, start_state, end_states, max_steps=5000):
        self.tape = list(tape)
        self.head = 0
        self.state = start_state
        self.rules = rules
        self.end_states = end_states
        self.max_steps = max_steps

    def step(self):
        symbol = self.tape[self.head]
        rule = (self.state, symbol)
        if rule in self.rules:
            new_symbol, move_direction, new_state = self.rules[rule]
            self.tape[self.head] = new_symbol
            self.state = new_state

            if move_direction == 'R':
                self.head += 1
                if self.head >= len(self.tape):
                    self.tape.append('_')
            elif move_direction == 'L':
                if self.head == 0:
                    self.tape.insert(0, '_')
                else:
                    self.head -= 1
        else:
            return False
        return True

    def execute(self):
        steps = 0
        while self.state not in self.end_states and self.step():
            steps += 1
            if steps >= self.max_steps:
                print("Досягнуто максимальної кількості кроків.")
                break
        return ''.join(self.tape).strip('_')


rules_1 = {
    ('q0', '1'): ('1', 'R', 'q0'),
    ('q0', '+'): ('1', 'R', 'q1'),
    ('q1', '1'): ('1', 'R', 'q1'),
    ('q1', '_'): ('_', 'L', 'q2'),
    ('q2', '1'): ('_', 'R', 'q_end')
}

test_1 = ['11+1111']
for test in test_1:
    turing_machine = TuringMachine(test, rules_1, 'q0', {'q_end'})
    print(f"1. Унарне додавання ({test}) → {turing_machine.execute()}")

rules_2 = {
    ("q0", "1"): ("1", "R", "q0"),
    ("q0", "+"): ("+", "R", "q0"),
    ("q0", "0"): ("0", "R", "q0"),
    ("q0", "_"): ("_", "L", "q1"),
    ("q1", "0"): ("_", "L", "q2"),
    ("q2", "1"): ("1", "L", "q2"),
    ("q2", "0"): ("0", "L", "q2"),
    ("q2", "+"): ("+", "L", "q3"),
    ("q3", "y"): ("y", "L", "q3"),
    ("q3", "x"): ("x", "L", "q3"),
    ("q3", "1"): ("y", "R", "q4"),
    ("q3", "0"): ("x", "R", "q4"),
    ("q3", "_"): ("x", "R", "q4"),
    ("q4", "y"): ("y", "R", "q4"),
    ("q4", "x"): ("x", "R", "q4"),
    ("q4", "+"): ("+", "R", "q0"),
    ("q1", "1"): ("_", "L", "q5"),
    ("q5", "1"): ("1", "L", "q5"),
    ("q5", "0"): ("0", "L", "q5"),
    ("q5", "+"): ("+", "L", "q6"),
    ("q6", "y"): ("y", "L", "q6"),
    ("q6", "x"): ("x", "L", "q6"),
    ("q6", "0"): ("y", "R", "q4"),
    ("q6", "1"): ("x", "L", "q7"),
    ("q6", "_"): ("y", "R", "q4"),
    ("q7", "0"): ("1", "R", "q8"),
    ("q7", "1"): ("0", "L", "q7"),
    ("q7", "_"): ("1", "R", "q8"),
    ("q8", "1"): ("1", "R", "q8"),
    ("q8", "0"): ("0", "R", "q8"),
    ("q8", "y"): ("y", "R", "q4"),
    ("q8", "x"): ("x", "R", "q4"),
    ("q1", "+"): ("_", "L", "q9"),
    ("q9", "y"): ("1", "L", "q9"),
    ("q9", "x"): ("0", "L", "q9"),
    ("q9", "_"): ("_", "R", "q_end"),
    ("q9", "1"): ("1", "R", "q_end"),
    ("q9", "0"): ("0", "R", "q_end"),
}

test_2 = ['101+011']
for test in test_2:
    turing_machine = TuringMachine(test, rules_2, 'q0', {'q_end'})
    print(f"2. Бінарне додавання ({test}) → {turing_machine.execute()}")

rules_3 = {
    ('q0', '1'): ('_', 'R', 'q1'),
    ('q1', '1'): ('1', 'R', 'q1'),
    ('q1', '-'): ('-', 'R', 'q1'),
    ('q1', '_'): ('_', 'L', 'q2'),
    ('q2', '1'): ('_', 'L', 'q3'),
    ('q3', '1'): ('1', 'L', 'q3'),
    ('q3', '-'): ('-', 'L', 'q3'),
    ('q3', '_'): ('_', 'R', 'q0'),
    ('q2', '-'): ('1', 'L', 'q_end')
}

test_3 = ['11111-11']
for test in test_3:
    turing_machine = TuringMachine(test, rules_3, 'q0', {'q_end'})
    print(f"3. Унарне віднімання ({test}) → {turing_machine.execute()}")

rules_4 = {
    ('q0', '1'): ('1', 'R', 'q0'),
    ('q0', '*'): ('*', 'R', 'q0'),
    ('q0', '_'): ('0', 'L', 'q1'),
    ('q1', '1'): ('1', 'L', 'q1'),
    ('q1', '*'): ('*', 'L', 'q1'),
    ('q1', '0'): ('0', 'L', 'q1'),
    ('q1', '_'): ('_', 'R', 'q2'),
    ('q2', '1'): ('_', 'R', 'q3'),
    ('q2', '*'): ('_', 'R', 'q8'),
    ('q3', '1'): ('1', 'R', 'q3'),
    ('q3', '*'): ('*', 'R', 'q4'),
    ('q4', '1'): ('x', 'R', 'q5'),
    ('q4', 'x'): ('x', 'R', 'q4'),
    ('q4', '0'): ('0', 'L', 'q7'),
    ('q5', '1'): ('1', 'R', 'q5'),
    ('q5', '0'): ('0', 'R', 'q5'),
    ('q5', '_'): ('1', 'L', 'q6'),
    ('q6', '0'): ('0', 'L', 'q6'),
    ('q6', '1'): ('1', 'L', 'q6'),
    ('q6', 'x'): ('x', 'R', 'q4'),
    ('q7', 'x'): ('1', 'L', 'q7'),
    ('q7', '*'): ('*', 'L', 'q7'),
    ('q7', '1'): ('1', 'L', 'q7'),
    ('q7', '_'): ('_', 'R', 'q2'),
    ('q8', '1'): ('_', 'R', 'q8'),
    ('q8', '0'): ('_', 'R', 'q_end'),
}

test_4 = ['111*111']
for test in test_4:
    turing_machine = TuringMachine(test, rules_4, 'q0', {'q_end'})
    print(f"4. Унарне множення ({test}) → {turing_machine.execute()}")

rules_5 = {
    ('q0', '1'): ('1', 'L', 'q0'),
    ('q0', '_'): ('0', 'R', 'q1'),
    ('q1', '1'): ('1', 'R', 'q1'),
    ('q1', '_'): ('_', 'L', 'q2'),
    ('q1', '0'): ('0', 'R', 'q1'),
    ('q1', '2'): ('2', 'R', 'q1'),
    ('q1', '3'): ('3', 'R', 'q1'),
    ('q1', '4'): ('4', 'R', 'q1'),
    ('q1', '5'): ('5', 'R', 'q1'),
    ('q1', '6'): ('6', 'R', 'q1'),
    ('q1', '7'): ('7', 'R', 'q1'),
    ('q1', '8'): ('8', 'R', 'q1'),
    ('q1', '9'): ('9', 'R', 'q1'),
    ('q2', '1'): ('_', 'L', 'q3'),
    ('q2', '0'): ('_', 'R', 'q_end'),
    ('q3', '1'): ('1', 'L', 'q3'),
    ('q3', '0'): ('0', 'L', 'q4'),
    ('q4', '_'): ('1', 'R', 'q1'),
    ('q4', '0'): ('1', 'R', 'q1'),
    ('q4', '1'): ('2', 'R', 'q1'),
    ('q4', '2'): ('3', 'R', 'q1'),
    ('q4', '3'): ('4', 'R', 'q1'),
    ('q4', '4'): ('5', 'R', 'q1'),
    ('q4', '5'): ('6', 'R', 'q1'),
    ('q4', '6'): ('7', 'R', 'q1'),
    ('q4', '7'): ('8', 'R', 'q1'),
    ('q4', '8'): ('9', 'R', 'q1'),
    ('q4', '9'): ('0', 'L', 'q4'),
}

test_5 = ['1111111']
for test in test_5:
    turing_machine = TuringMachine(test, rules_5, 'q0', {'q_end'})
    print(f"5. Унарне в десяткову ({test}) → {turing_machine.execute()}")

import unittest


class TestTuringMachine(unittest.TestCase):

    def test_unary_addition(self):
        rules = rules_1
        machine = TuringMachine('11+1111', rules, 'q0', {'q_end'})
        result = machine.execute()
        self.assertEqual(result, '111111')

    def test_binary_addition(self):
        rules = rules_2
        machine = TuringMachine('101+011', rules, 'q0', {'q_end'})
        result = machine.execute()
        self.assertEqual(result, '1000')

    def test_unary_subtraction(self):
        rules = rules_3
        machine = TuringMachine('11111-11', rules, 'q0', {'q_end'})
        result = machine.execute()
        self.assertEqual(result, '111')

    def test_unary_multiplication(self):
        rules = rules_4
        machine = TuringMachine('111*111', rules, 'q0', {'q_end'})
        result = machine.execute()
        self.assertEqual(result, '111111111')

    def test_unary_to_decimal(self):
        rules = rules_5
        machine = TuringMachine('1111111', rules, 'q0', {'q_end'})
        result = machine.execute()
        self.assertEqual(result, '7')

if __name__ == '__main__':
    unittest.main()
