import csv
from datetime import datetime
import matplotlib.pyplot as plt

def load_orders(csv_file):
    orders = []
    try:
        with open(csv_file, mode='r', encoding="utf-8") as file:
            reader = csv.DictReader(file)
            for row in reader:
                row['Сума замовлення'] = float(row['Сума замовлення'])
                row['Дата замовлення'] = datetime.strptime(row['Дата замовлення'], '%Y-%м-%d')
                orders.append(row)
    except FileNotFoundError:
        print("CSV файл не знайдено")
    except Exception as e:
        print(f"Помилка завантаження CSV-файлу: {e}")
    return orders

def save_orders(csv_file, orders):
    try:
        with open(csv_file, mode='w', encoding="utf-8", newline='') as file:
            fieldnames = ["Ім'я клієнта", 'Номер замовлення', 'Дата замовлення', 'Сума замовлення', 'Статус']
            writer = csv.DictWriter(file, fieldnames=fieldnames)
            writer.writeheader()
            for order in orders:
                order['Дата замовлення'] = order['Дата замовлення'].strftime('%Y-%м-%d')
                writer.writerow(order)
    except Exception as e:
        print(f"Помилка збереження CSV-файлу: {e}")

def add_order(orders, client_name, order_number, order_date, order_amount, status):
    new_order = {
        "Ім'я клієнта": client_name,
        "Номер замовлення": order_number,
        "Дата замовлення": datetime.strptime(order_date, '%Y-%м-%d'),
        "Сума замовлення": float(order_amount),
        "Статус": status
    }
    orders.append(new_order)
    return orders

def delete_order(orders, order_number):
    orders = [order for order in orders if order['Номер замовлення'] != order_number]
    return orders

def list_orders(orders):
    if not orders:
        print("Немає замовлень для відображення.")
        return

    headers = ["Ім'я клієнта", 'Номер замовлення', 'Дата замовлення', 'Сума замовлення', 'Статус']
    print(f"{headers[0]:<20} {headers[1]:<15} {headers[2]:<15} {headers[3]:<15} {headers[4]:<10}")
    print("-" * 75)
    for order in orders:
        print(f"{order["Ім'я клієнта"]:<20} {order['Номер замовлення']:<15} {order['Дата замовлення'].strftime('%Y-%м-%d'):<15} {order['Сума замовлення']:<15.2f} {order['Статус']:<10}")

def total_orders(orders):
    total_orders = len(orders)
    total_amount = sum(order['Сума замовлення'] for order in orders)
    return total_orders, total_amount

def orders_by_status(orders):
    status_count = {'виконано': 0, 'в процесі': 0}
    for order in orders:
        if order['Статус'] in status_count:
            status_count[order['Статус']] += 1
    return status_count

def largest_order(orders):
    if not orders:
        return None
    return max(orders, key=lambda x: x['Сума замовлення'])

def plot_status(orders):
    status_count = orders_by_status(orders)
    labels = status_count.keys()
    sizes = status_count.values()
    plt.pie(sizes, labels=labels, autopct='%1.1f%%')
    plt.title('Частка виконаних і невиконаних замовлень')
    plt.show()

def plot_histogram(orders):
    dates = [order['Дата замовлення'] for order in orders]
    plt.hist(dates, bins=10)
    plt.xlabel('Дата замовлення')
    plt.ylabel('Кількість замовлень')
    plt.title('Гістограма кількості замовлень за датами')
    plt.show()

csv_file = 'C:/Програмування/Module_5.csv'
orders = load_orders(csv_file)
orders = add_order(orders, "Лук'янчук Денис", '2024', '2024-10-01', 3000.0, 'в процесі')
orders = delete_order(orders, '2024')
save_orders(csv_file, orders)
list_orders(orders)
