array = list(map(int, input("Введіть елементи масиву: ").split()))

if len(array) % 2 != 0:
    print("Масив повинен містити парну кількість елементів.")
else:
    n = len(array) // 2
    array = array[n:] + array[:n]

    print(array)
