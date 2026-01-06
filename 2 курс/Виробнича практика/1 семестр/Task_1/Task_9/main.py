with open('numbers.txt', 'r') as file:
    content = ' '.join(file.readlines())

    L = list(map(int, content.replace(',', ' ').split()))

print("Список:", ' '.join(map(str, L)))

if L:
    L.remove(max(L))

print("Список після видалення максимального елемента:", ' '.join(map(str, L)))


