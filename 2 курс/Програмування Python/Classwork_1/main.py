# 1
lst = [10, 20, 30, 40, 50]
lst.append(60)
lst.remove(20)
for n in lst:
    print(n)

# 2
tpn = ('apple', 'banana', 'cherry')
print(f"Кількість елементів у кортежі: {len(tpn)}")
print(f"Чи є banana у кортежі: {'banana' in tpn}")

# 3
students = {'John': 85, 'Alice': 92, 'Bob': 78}
students['Emma'] = 90
print(f"Оцінка Alice: {students['Alice']}")
for name, grade in students.items():
    print(f"Студент: {name}, Оцінка: {grade}")

# 4
numbers_set = {1, 2, 3}
numbers_set.add(4)
other_set = {3, 4, 5, 6}
union_set = numbers_set.union(other_set)
print(f"Чи є число 5 у об'єднаній множині: {5 in union_set}")

# 5
squares = [x**2 for x in range(1, 10)]
print(f"Список квадратів чисел від 1 до 10: {squares}")