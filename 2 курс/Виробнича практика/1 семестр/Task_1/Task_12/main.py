import os

class Receipt:
    def __init__(self, service_name: str, receipt_number: int, amount_paid: float):
        self.service_name = service_name
        self.receipt_number = receipt_number
        self.amount_paid = amount_paid

    def get_service_name(self):
        return self.service_name

    def get_receipt_number(self):
        return self.receipt_number

    def get_amount_paid(self):
        return self.amount_paid

    def __str__(self):
        return f"{self.service_name}, #{self.receipt_number}, {self.amount_paid} UAH"

    @staticmethod
    def read_receipts_from_file(filename):
        receipts = []
        with open(filename, 'r') as file:
            for line in file:
                service_name, receipt_number, amount_paid = line.strip().split(',')
                receipts.append(Receipt(service_name.strip(), int(receipt_number.strip()), float(amount_paid.strip())))
        return receipts

def sort_receipts(receipts, criterion):
    if criterion == "послуга":
        receipts.sort(key=lambda r: r.get_service_name())
    elif criterion == "номер":
        receipts.sort(key=lambda r: r.get_receipt_number())
    elif criterion == "сума":
        receipts.sort(key=lambda r: r.get_amount_paid())
    return receipts

def search_receipt_by_service(receipts, service_name):
    return [receipt for receipt in receipts if receipt.get_service_name().lower() == service_name.lower()]

def menu():
    receipts = []
    filename = 'receipts.txt'
    if os.path.exists(filename):
        receipts = Receipt.read_receipts_from_file(filename)
    else:
        print(f"Файл {filename} не знайдено.")

    while True:
        print("\nМеню: ")
        print("1. Показати всі квитанції")
        print("2. Сортувати квитанції")
        print("3. Шукати квитанцію за назвою послуги")
        print("4. Вихід")

        choice = input("Виберіть опцію: ")

        if choice == "1":
            for receipt in receipts:
                print(receipt)
        elif choice == "2":
            criterion = input("Введіть критерій сортування (послуга, номер, сума): ")
            sorted_receipts = sort_receipts(receipts, criterion)
            for receipt in sorted_receipts:
                print(receipt)
        elif choice == "3":
            service_name = input("Введіть назву послуги для пошуку: ")
            found_receipts = search_receipt_by_service(receipts, service_name)
            if found_receipts:
                for receipt in found_receipts:
                    print(receipt)
            else:
                print("Квитанцію не знайдено.")
        elif choice == "4":
            print("Вихід із програми.")
            break
        else:
            print("Неправильний вибір. Спробуйте ще раз.")

if __name__ == "__main__":
    menu()
