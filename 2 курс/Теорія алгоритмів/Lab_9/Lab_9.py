def can_partition(numbers):
    # Обчислюємо загальну суму
    total_sum = sum(numbers)
    
    # Якщо сума непарна, розбиття неможливе
    if total_sum % 2 != 0:
        return False, None, None
    
    target = total_sum // 2
    n = len(numbers)
    
    # Ініціалізація масиву dp: dp[i][s] означає, чи можна скласти суму s першими i числами
    dp = [[False] * (target + 1) for _ in range(n + 1)]
    dp[0][0] = True
    
    # Для відстеження чисел у підмножині
    subset = [[[] for _ in range(target + 1)] for _ in range(n + 1)]
    
    # Динамічне програмування
    for i in range(1, n + 1):
        num = numbers[i - 1]
        for s in range(target + 1):
            # Не беремо поточне число
            dp[i][s] = dp[i - 1][s]
            subset[i][s] = subset[i - 1][s]
            
            # Беремо поточне число, якщо можливо
            if s >= num and dp[i - 1][s - num]:
                dp[i][s] = True
                subset[i][s] = subset[i - 1][s - num] + [num]
    
    # Якщо не можна скласти target, повертаємо False
    if not dp[n][target]:
        return False, None, None
    
    # Перша підмножина
    subset1 = subset[n][target]
    
    # Друга підмножина: беремо всі числа, які не увійшли до першої
    remaining = numbers.copy()
    for num in subset1:
        remaining.remove(num)
    subset2 = remaining
    
    return True, subset1, subset2

# Приклад використання
numbers = [1, 5, 11, 5]  # Приклад чисел i_1, i_2, ..., i_k
possible, subset1, subset2 = can_partition(numbers)

if possible:
    print(f"Розбиття можливе!")
    print(f"Перша підмножина: {subset1}, сума: {sum(subset1)}")
    print(f"Друга підмножина: {subset2}, сума: {sum(subset2)}")
else:
    print("Розбиття неможливе.")