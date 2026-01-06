class BankAccount:
    def __init__(self, account_number, owner, balance = 0):
        self.account_number = account_number
        self.owner = owner
        self.balance = balance

    def deposit(self, amount):
        if amount > 0:
            self.balance += amount
            print(f"Додано {amount} до рахунку {self.account_number}. Ваш баланс: {self.balance}")
        else:
            print("Сума повинна бути більша за 0")

    def withdrawal(self, amount):
        if 0 < amount <= self.balance:
            self.balance -= amount
            print(f"Знято {amount} з рахунку {self.account_number}. Ваш баланс: {self.balance}")
        else:
            print("Недостатньо коштів. Поповніть рахунок або введіть меншу суму")

    def check_balance(self):
        print(f"Баланс рахунку {self.account_number}: {self.balance}")
        return self.balance

    def get_info(self):
        return f"Номер рахунку: {self.account_number}, Власник: {self.owner}, Баланс: {self.balance}"

    def __add__(self, amount):
        self.deposit(amount)
        return self


class SavingsAccount(BankAccount):
    def __init__(self, account_number, owner, balance = 0, interest_rate = 0):
        BankAccount.__init__(self, account_number, owner, balance)
        if interest_rate < 0:
            raise ValueError("Відсоткова ставка не може бути меншою за 0%")
        self.interest_rate = interest_rate / 100

    def interest(self):
        interest = self.balance * self.interest_rate
        self.balance += interest
        print(f"Нараховано відсотки {interest}. Новий баланс: {self.balance}")
        return self

    def get_info(self):
        return f"Номер рахунку: {self.account_number}, Власник: {self.owner}, Баланс: {self.balance}, Відсоткова ставка: {self.interest_rate * 100}%"


def input_owner(prompt):
    while True:
        value = input(prompt)
        if value.isalpha():
            return value
        else:
            print("Неправильний ввід. Введіть тільки букви без символів і цифр")


def input_account_number(prompt):
    while True:
        value = input(prompt)
        if value.isalnum() and ' ' not in value:
            return value
        else:
            print("Неправильний ввід. Введіть тільки букви або цифри без пробілів і символів")


account_number = input_account_number("Введіть номер рахунку: ")
owner = input_owner("Введіть ім'я власника: ")

while True:
    account_type = input("Виберіть тип рахунку (звичайний або ощадний): ").lower()
    if account_type == "звичайний":
        account1 = BankAccount(account_number, owner)
        break
    elif account_type == "ощадний":
        while True:
            try:
                interest_rate = float(input("Введіть відсоткову ставку для ощадного рахунку: ").replace(',', '.'))
                account1 = SavingsAccount(account_number, owner, interest_rate = interest_rate)
                break
            except ValueError:
                print("Неправильний ввід відсоткової ставки. Введіть число")
        break
    else:
        print("Неправильний вибір. Будь ласка, виберіть звичайний або ощадний")

while True:
    action = input("Виберіть одну з опцій (поповнення, зняття, перевірка, відсоток, вихід): ").lower()
    if action in ["поповнення", "поповнення коштів"]:
        while True:
            try:
                amount = float(input("Введіть суму для поповнення: ").replace(',', '.'))
                account1.deposit(amount)
                break
            except ValueError:
                print("Неправильний ввід суми. Будь ласка, введіть число.")
    elif action in ["зняття", "зняття коштів"]:
        while True:
            try:
                amount = float(input("Введіть суму для зняття: ").replace(',', '.'))
                account1.withdrawal(amount)
                break
            except ValueError:
                print("Неправильний ввід суми. Будь ласка, введіть число.")
    elif action in ["перевірка", "перевірка балансу"]:
        account1.check_balance()
    elif action in ["відсоток", "відсоткова ставка"]:
        if isinstance(account1, SavingsAccount):
            account1.interest()
        else:
            print("Ця операція доступна лише для ощадних рахунків.")
    elif action in ["вихід", "exit"]:
        break
    else:
        print("Неправильна опція. Виберіть одну з доступних опцій.")

print("\nІнформація про рахунок:")
print(account1.get_info())