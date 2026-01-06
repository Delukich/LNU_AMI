class Flower:
    total_flower_types = 0

    def __init__(self, name, color, quantity, price):
        self.name = name
        self.color = color
        self._quantity = quantity
        self.price = price
        Flower.total_flower_types += 1

    def __str__(self):
        return f"{self.name} ({self.color}), {self._quantity} шт. по ціні {self.price} грн"

    @property
    def quantity(self):
        return self._quantity

    @quantity.setter
    def quantity(self, value):
        if value < 0:
            raise ValueError("Кількість не може бути від'ємною")
        self._quantity = value

    @staticmethod
    def calculate_total_value(flowers):
        total_value = sum(flower.price * flower.quantity for flower in flowers)
        return total_value


class Rose(Flower):
    available_colors = ['червоний', 'білий', 'жовтий', 'рожевий']

    def __init__(self, color, quantity, price):
        if color not in Rose.available_colors:
            raise ValueError(f"Неправильний колір. Доступні кольори для троянди: {', '.join(Rose.available_colors)}")
        super().__init__("Троянда", color, quantity, price)

    def __str__(self):
        return f"Троянда ({self.color}): {self.quantity} шт. по ціні {self.price} грн"


class Tulip(Flower):
    available_colors = ['білий', 'рожевий', 'фіолетовий', 'жовтий']

    def __init__(self, color, quantity, price):
        if color not in Tulip.available_colors:
            raise ValueError(f"Неправильний колір. Доступні кольори для тюльпана: {', '.join(Tulip.available_colors)}")
        super().__init__("Тюльпан", color, quantity, price)

    def __str__(self):
        return f"Тюльпан ({self.color}): {self.quantity} шт. по ціні {self.price} грн"


class Lily(Flower):
    available_colors = ['білий', 'рожевий','жовтий', 'помаранчевий']

    def __init__(self, color, quantity, price):
        if color not in Lily.available_colors:
            raise ValueError(f"Неправильний колір. Доступні кольори для лілії: {', '.join(Lily.available_colors)}")
        super().__init__("Лілія", color, quantity, price)

    def __str__(self):
        return f"Лілія ({self.color}): {self.quantity} шт. по ціні {self.price} грн"


class FlowerShop:
    def __init__(self):
        self.flowers = []

    def add_flower(self, flower):
        self.flowers.append(flower)

    def display_flowers(self):
        for flower in self.flowers:
            print(flower)

    def calculate_inventory_value(self):
        return Flower.calculate_total_value(self.flowers)


def input_quantity(prompt):
    while True:
        try:
            value = int(input(prompt))
            if value >= 0:
                return value
            else:
                print("Кількість не може бути від'ємною")
        except ValueError:
            print("Неправильний ввід. Введіть ціле число")


def input_price(prompt):
    while True:
        try:
            value = float(input(prompt).replace(',', '.'))
            if value >= 0:
                return value
            else:
                print("Ціна не може бути від'ємною")
        except ValueError:
            print("Неправильний ввід. Введіть число")


def choose_color(flower_type):
    if flower_type == "троянда":
        available_colors = Rose.available_colors
    elif flower_type == "тюльпан":
        available_colors = Tulip.available_colors
    elif flower_type == "лілія":
        available_colors = Lily.available_colors
    else:
        return None

    print(f"Доступні кольори для {flower_type}: {', '.join(available_colors)}")

    while True:
        color = input(f"Виберіть колір для {flower_type}: ").lower()
        if color in available_colors:
            return color
        else:
            print(f"Неправильний колір. Доступні кольори: {', '.join(available_colors)}")


def add_flower_to_shop():
    while True:
        flower_type = input("Введіть тип квітки (троянда, тюльпан, лілія): ").lower()
        if flower_type in ["троянда", "тюльпан", "лілія"]:
            color = choose_color(flower_type)
            quantity = input_quantity(f"Введіть кількість для {flower_type}: ")
            price = input_price(f"Введіть ціну за {flower_type}: ")

            if flower_type == "троянда":
                return Rose(color, quantity, price)
            elif flower_type == "тюльпан":
                return Tulip(color, quantity, price)
            elif flower_type == "лілія":
                return Lily(color, quantity, price)
        else:
            print("Неправильний тип квітки. Будь ласка, виберіть: троянда, тюльпан, або лілія")


shop = FlowerShop()

while True:
    action = input("Виберіть дію (додати квітку, показати квіти, загальна вартість, вихід): ").lower()

    if action == "додати квітку" or action == "додати":
        flower = add_flower_to_shop()
        shop.add_flower(flower)

    elif action == "показати квіти" or action == "показати":
        shop.display_flowers()

    elif action == "загальна вартість" or action == "вартість":
        total_value = shop.calculate_inventory_value()
        print(f"Загальна вартість квітів: {total_value} грн")

    elif action == "вихід":
        break

    else:
        print("Неправильний вибір. Виберіть одну з опцій")
