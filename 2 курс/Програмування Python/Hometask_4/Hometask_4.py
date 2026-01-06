import pandas as pd
import matplotlib.pyplot as plt

file1 = 'C:/Програмування/Hometask_4/water_data1.csv'
file2 = 'C:/Програмування/Hometask_4/water_data2.csv'
output_file = 'merged_file.csv'

data1 = pd.read_csv(file1)
data2 = pd.read_csv(file2)

# Об'єднання даних
merged_data = pd.concat([data1, data2], ignore_index=True)
merged_data.to_csv(output_file, index=False)
print("Об'єднані дані:")
print(merged_data)

# Перевірка на коректність показників лічильника
if (merged_data['current_reading'] >= merged_data['previous_reading']).all():
    print("Усі поточні показники лічильника не менші за попередні")
else:
    print("Помилка: деякі поточні показники лічильника менші за попередні")

# Розрахунок спожитої води
merged_data['consumed_water'] = merged_data['current_reading'] - merged_data['previous_reading']

# Функція для виведення даних за конкретний місяць
def show_data_for_month(month):
    month_data = merged_data[merged_data['month'] == month]
    return month_data[['month', 'apartment_number', 'consumed_water']]

month = 3 
print(f"\nДані за місяць {month}:")
print(show_data_for_month(month))

month_data = merged_data[merged_data['month'] == month]

if not month_data.empty:
    plt.figure(figsize=(8, 8))
    plt.pie(
        month_data['consumed_water'],
        labels=month_data['apartment_number'],
        autopct='%1.1f%%',
        startangle=140,
    )
    plt.title(f'Розподіл споживання води по квартирах за місяць {month}')
    plt.show()
else:
    print(f"Немає даних для місяця {month}")


# Розрахунок загального споживання води за весь час
total_consumption = merged_data.groupby('apartment_number')['consumed_water'].sum().reset_index()
print(f"Загальна кількість спожитої води в будинку за весь час: {total_consumption['consumed_water'].sum()} м³")

colors = ['blue', 'green', 'red', 'orange', 'purple', 'brown', 'pink', 'gray']
plt.figure(figsize=(8, 8))
plt.pie(total_consumption['consumed_water'], 
        labels=total_consumption['apartment_number'], 
        autopct='%1.1f%%', startangle=140, colors=colors)
plt.title('Загальний розподіл споживання води за весь час по квартирах')
plt.show()


# Розрахунок загальної вартості води по кварталах
water_price = 10.0
merged_data['quarter'] = (merged_data['month'] - 1) // 3 + 1
merged_data['water_cost'] = merged_data['consumed_water'] * water_price

quarter_cost = merged_data.groupby('quarter')['water_cost'].sum().reset_index()
print("\nТаблиця вартості води за кожний квартал:")
print(quarter_cost)

plt.figure(figsize=(8, 6))
plt.bar(quarter_cost['quarter'], quarter_cost['water_cost'], color=colors)
plt.xlabel('Квартали')
plt.ylabel('Загальна вартість води')
plt.title('Загальна вартість води за кожний квартал')
plt.xticks(quarter_cost['quarter'])
plt.grid(axis='y', linestyle='--', alpha=0.7)
plt.show()


# Знаходження місяця з найбільшим споживанням для кожної квартири
max_usage_data = merged_data.loc[merged_data.groupby('apartment_number')['consumed_water'].idxmax(), ['apartment_number', 'month', 'consumed_water']]
print("\nНайбільше місячне споживання і номери місяців, у яких споживання було найбільше:")
print(max_usage_data)

plt.figure(figsize=(10, 6))
bars = plt.bar(max_usage_data['apartment_number'], max_usage_data['consumed_water'], color=colors)
plt.xlabel('Номера квартир')
plt.ylabel('Максимальне споживання води за місяць')
plt.title('Максимальне споживання води за місяць по квартирах')

for bar, month in zip(bars, max_usage_data['month']):
    plt.text(bar.get_x() + bar.get_width() / 2, bar.get_height(), f'за {month} місяць', 
             ha='center', va='bottom', fontsize=10)

plt.show()
