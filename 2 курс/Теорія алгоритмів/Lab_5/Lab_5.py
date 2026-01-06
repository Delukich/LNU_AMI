#завдання 1
def elementary_function(x1, x2, x3, x4):
    return x1 * (x2 + x3) + x2 * x4

x1, x2, x3, x4 = 2, 3, 4, 5
result = elementary_function(x1, x2, x3, x4)
print("1. Результат:", result)


#завдання 2
def primitive_recursive_add(x, y):
    if y == 0:
        return x  
    else:
        return primitive_recursive_add(x, y - 1) + 1  

x, y = 3, 4
result = primitive_recursive_add(x, y)
print("2. Результат:", result)


#завдання 3
def primitive_recursive_multiply(x, y):
    if y == 0:
        return 0  
    else:
        return primitive_recursive_multiply(x, y - 1) + x  

x, y = 3, 4
result = primitive_recursive_multiply(x, y)
print("3. Результат:", result)


#завдання 4
def merge_sort_recursive(arr):
    if len(arr) <= 1:
        return arr
    
    mid = len(arr) // 2
    left = merge_sort_recursive(arr[:mid])
    right = merge_sort_recursive(arr[mid:])
    
    return merge(left, right)

def merge_sort_iterative(arr):
    width = 1
    n = len(arr)
    while width < n:
        left = 0
        while left < n:
            right = min(left + 2 * width, n)
            mid = min(left + width, n)
            arr[left:right] = merge(arr[left:mid], arr[mid:right])
            left += 2 * width
        width *= 2
    return arr

def merge(left, right):
    sorted_arr = []
    i = j = 0
    
    while i < len(left) and j < len(right):
        if left[i] < right[j]:
            sorted_arr.append(left[i])
            i += 1
        else:
            sorted_arr.append(right[j])
            j += 1
    
    sorted_arr.extend(left[i:])
    sorted_arr.extend(right[j:])
    
    return sorted_arr

arr = [22, 27, 49, 6, 9, 93, 18]
print("4. Початковий масив:", arr)
print("   Нерекурсивне сортування:", merge_sort_iterative(arr[:]))
print("   Рекурсивне сортування:", merge_sort_recursive(arr[:]))



#завдання 5
def fib_recursive(n):
    if n <= 1:
        return n
    return fib_recursive(n - 1) + fib_recursive(n - 2)

def fib_iterative(n):
    if n <= 1:
        return n
    a, b = 0, 1
    for _ in range(2, n + 1):
        a, b = b, a + b
    return b

print("5. Нерекурсивна послідовність Фібоначчі:", end=" ")
for i in range(10):
    print(fib_iterative(i), end=" ")  
print()

print("   Рекурсивна послідовність Фібоначчі:", end=" ")
for i in range(10):
    print(fib_recursive(i), end=" ")  
print()



