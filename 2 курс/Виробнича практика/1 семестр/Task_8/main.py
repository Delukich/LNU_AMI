def count_word_occurrences():
    sequence = input("Введіть послідовність слів: ")
    words = sequence.rstrip('.').split(',')

    while True:
        n = int(input("Введіть номер n-го слова: "))
        if 0 < n <= len(words):
            break
        print("Неправильне значення n, спробуйте ще раз.")

    target_word = words[n - 1].strip()
    count = sum(1 for word in words if word.strip() == target_word)

    return count

if __name__ == "__main__":
    result = count_word_occurrences()
    print(f"Кількість повторів слів: {result}")