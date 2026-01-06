import networkx as nx
import matplotlib.pyplot as plt

# Завдання 1,2 
def apply_substitution(word):
    if 'xy' in word:
        word = word.replace('xy', '', 1) 
    elif 'yx' in word:
        word = word.replace('yx', 'xy', 1) 
    return word

def process_queue(queue, visited, process_func):
    while queue:
        word = queue.pop(0)
        if word not in visited:
            visited.add(word)
            process_func(word, queue, visited)

def generate_word_sequence(start_word):
    queue = [start_word]
    visited = set()
    word_sequence = []

    def process_func(word, queue, visited):
        word_sequence.append(word)
        next_word = apply_substitution(word)
        if next_word != word and next_word not in visited:
            queue.append(next_word)

    process_queue(queue, visited, process_func)
    return word_sequence

def build_graph(start_word):
    G = nx.DiGraph()
    queue = [start_word]
    visited = set()

    def process_func(word, queue, visited):
        nonlocal previous_word
        if word != start_word:
            G.add_edge(previous_word, word)
        next_word = apply_substitution(word)
        if next_word != word and next_word not in visited:
            queue.append(next_word)
        previous_word = word

    previous_word = start_word
    process_queue(queue, visited, process_func)
    return G

def visualize_graph(G):
    pos = nx.spring_layout(G)
    nx.draw(
        G, pos, with_labels=True, node_size=2000, node_color="blue",
        font_size=10, edge_color="black", arrowsize=20
    )
    plt.show()


start_word = "yxyx"

graph = build_graph(start_word)
visualize_graph(graph)
sequence = generate_word_sequence(start_word)
print(f"\nЗавдання №1 {sequence}")


# Завдання 3 (A - B)
rules_subtraction = [
    ('1#1', '#'), 
    ('#1', ''),   
    ('#', '')     
]

def apply_rules(word, rules):
    steps = [word]  
    while True:
        for pattern, replacement in rules:
            if pattern in word:
                word = word.replace(pattern, replacement, 1)
                steps.append(word)  
                break
        else:
            break
    return steps

a = 5
b = 3

input_word = '1' * a + '#' + '1' * b

print(f"\nВіднімання {a} - {b}:")
steps = apply_rules(input_word, rules_subtraction)
for i, step in enumerate(steps, 1):
    print(f"Крок {i}: {step}")



# Завдання 4 (A + B)
rules_addition = [
    ('1#', '11')  
]

def apply_rules(word, rules):
    steps = [word]  
    while True:
        for pattern, replacement in rules:
            if pattern in word:
                word = word.replace(pattern, replacement, 1)
                steps.append(word)  
                break
        else:
            break
    return steps

a = 4
b = 5

input_word = '1' * a + '#' + '1' * b

print(f"\nДодавання {a} + {b}:")
steps = apply_rules(input_word, rules_addition)
for i, step in enumerate(steps, 1):
    print(f"Крок {i}: {step}")



# Завдання 5: (A * B)  
def markov_multiplication(word):
    print(f"\nПочатковий вираз: {word}")
    
    steps = []
    
    rules = [
        ('*11', 'T*1'),   
        ('*1', 'T'),      
        ('1T', 'T1F'),         
        ('F1', '1F'),
        ('FT', 'TF'),     
        ('T1', 'T'),      
        ('TF', 'F'),      
        ('F', '1'),       
               
    ]
    
    i = 0

    while i < 100: 
        for old, new in rules:
            if old in word:
                word = word.replace(old, new, 1)
                steps.append(word)
                break
        i+=1

    return steps


start_word = '111*1111'

steps = markov_multiplication(start_word)

for i, step in enumerate(steps, 1):
    print(f"Крок {i}: {step}")


# Завдання 6: 
def markov_remove_a(word):
    print(f"\nПочатковий вираз: {word}")
    
    steps = []
    
    first_a_found = False
    
    while 'a' in word or ' ' in word:
        if not first_a_found and 'a' in word:
            word = word.replace('a', ' ', 1)  
            first_a_found = True
        elif 'a' in word:
            word = word.replace('a', '', 1)  
        elif ' ' in word:
            word = word.replace(' ', 'a', 1)  
        
        steps.append(word)

    return steps

start_word = 'aaaaaa'
steps = markov_remove_a(start_word)

for i, step in enumerate(steps, 1):
    print(f"Крок {i}: {step}")


# Завдання 7: 
def markov_remove_duplicate_spaces(word):
    print(f"\nПочатковий вираз: {word}")
    
    steps = []  
    while '  ' in word:  
        word = word.replace('  ', ' ')  
        steps.append(word)  
    return steps


test_string = "a      b  c"

steps = markov_remove_duplicate_spaces(test_string)
for i, step in enumerate(steps, 1):
    print(f"Крок {i}: {step}")









