import pandas as pd
import tkinter as tk
from tkinter import filedialog, messagebox, ttk
import matplotlib.pyplot as plt

class ProductManager:
    def __init__(self):
        self.data = pd.DataFrame(columns=["Назва", "Категорія", "Кількість", "Ціна"])
    
    def load_from_csv(self, file_path):
        try:
            self.data = pd.read_csv(file_path)
            if not all(col in self.data.columns for col in ["Назва", "Категорія", "Кількість", "Ціна"]):
                raise ValueError("Невірний формат CSV-файлу.")
        except Exception as e:
            messagebox.showerror("Помилка", f"Не вдалося завантажити файл: {e}")
    
    def save_to_csv(self, file_path):
        try:
            self.data.to_csv(file_path, index=False)
        except Exception as e:
            messagebox.showerror("Помилка", f"Не вдалося зберегти файл: {e}")

    def add_product(self, name, category, quantity, price):
        if name in self.data["Назва"].values:
            messagebox.showerror("Помилка", "Товар із такою назвою вже існує.")
            return
        self.data = pd.concat([self.data, pd.DataFrame([{"Назва": name, "Категорія": category, 
                                                         "Кількість": quantity, "Ціна": price}])], ignore_index=True)

    def edit_product(self, name, new_data):
        if name in self.data["Назва"].values:
            self.data.loc[self.data["Назва"] == name, ["Категорія", "Кількість", "Ціна"]] = \
                new_data["Категорія"], new_data["Кількість"], new_data["Ціна"]
        else:
            messagebox.showerror("Помилка", "Товар не знайдено.")

    def delete_product(self, name):
        if name in self.data["Назва"].values:
            self.data = self.data[self.data["Назва"] != name]
        else:
            messagebox.showerror("Помилка", "Товар не знайдено.")

    def total_quantity(self):
        return self.data["Кількість"].sum()

    def category_total_value(self):
        return self.data.groupby("Категорія").apply(lambda x: (x["Кількість"] * x["Ціна"]).sum())

    def most_expensive_and_stocked(self):
        if self.data.empty:
            return None, None
        most_expensive = self.data.loc[self.data["Ціна"].idxmax()]
        most_stocked = self.data.loc[self.data["Кількість"].idxmax()]
        return most_expensive, most_stocked

    def scatter_plot(self):
        if self.data.empty:
            messagebox.showinfo("Інформація", "Дані відсутні.")
            return
        plt.scatter(self.data["Кількість"], self.data["Ціна"])
        plt.xlabel("Кількість")
        plt.ylabel("Ціна за одиницю")
        plt.title("Ціна vs Кількість")
        plt.show()

    def pie_chart(self):
        if self.data.empty:
            messagebox.showinfo("Інформація", "Дані відсутні.")
            return
        category_counts = self.data["Категорія"].value_counts()
        category_counts.plot.pie(autopct="%1.1f%%")
        plt.title("Розподіл товарів за категоріями")
        plt.ylabel("")
        plt.show()

    def histogram(self):
        if self.data.empty:
            messagebox.showinfo("Інформація", "Дані відсутні.")
            return
        self.data["Ціна"].plot.hist(bins=10, alpha=0.7)
        plt.title("Гістограма цін товарів")
        plt.xlabel("Ціна")
        plt.ylabel("Кількість товарів")
        plt.show()

# Tkinter Interface
def main():
    manager = ProductManager()
    
    def load_file():
        file_path = filedialog.askopenfilename(filetypes=[("CSV Files", "*.csv")])
        if file_path:
            manager.load_from_csv(file_path)
            update_table()
    
    def save_file():
        file_path = filedialog.asksaveasfilename(defaultextension=".csv", filetypes=[("CSV Files", "*.csv")])
        if file_path:
            manager.save_to_csv(file_path)

    def add_product():
        try:
            manager.add_product(name_var.get(), category_var.get(), int(quantity_var.get()), float(price_var.get()))
            update_table()
        except ValueError:
            messagebox.showerror("Помилка", "Некоректні дані.")
    
    def edit_product():
        try:
            new_data = {"Категорія": category_var.get(),
                        "Кількість": int(quantity_var.get()), "Ціна": float(price_var.get())}
            manager.edit_product(name_var.get(), new_data)
            update_table()
        except ValueError:
            messagebox.showerror("Помилка", "Некоректні дані.")

    def delete_product():
        manager.delete_product(name_var.get())
        update_table()

    def update_table():
        for row in table.get_children():
            table.delete(row)
        for _, row in manager.data.iterrows():
            table.insert("", "end", values=list(row))

    def show_total_quantity():
        messagebox.showinfo("Загальна кількість", f"Кількість товарів: {manager.total_quantity()}")

    def show_category_values():
        values = manager.category_total_value()
        messagebox.showinfo("Загальна вартість", str(values))

    def show_most_expensive_and_stocked():
        most_expensive, most_stocked = manager.most_expensive_and_stocked()
        if most_expensive is None or most_stocked is None:
            messagebox.showinfo("Інформація", "Дані відсутні.")
            return
        messagebox.showinfo("Найдорожчий і найбільший запас",
                            f"Найдорожчий: {most_expensive['Назва']} за {most_expensive['Ціна']}\n"
                            f"Найбільший запас: {most_stocked['Назва']} ({most_stocked['Кількість']})")

    root = tk.Tk()
    root.title("Менеджер товарів")

    tk.Button(root, text="Завантажити CSV", command=load_file).pack()
    tk.Button(root, text="Зберегти CSV", command=save_file).pack()

    frame = tk.Frame(root)
    frame.pack()

    name_var = tk.StringVar()
    category_var = tk.StringVar()
    quantity_var = tk.StringVar()
    price_var = tk.StringVar()

    tk.Label(frame, text="Назва").grid(row=0, column=0)
    tk.Entry(frame, textvariable=name_var).grid(row=0, column=1)

    tk.Label(frame, text="Категорія").grid(row=1, column=0)
    tk.Entry(frame, textvariable=category_var).grid(row=1, column=1)

    tk.Label(frame, text="Кількість").grid(row=2, column=0)
    tk.Entry(frame, textvariable=quantity_var).grid(row=2, column=1)

    tk.Label(frame, text="Ціна").grid(row=3, column=0)
    tk.Entry(frame, textvariable=price_var).grid(row=3, column=1)

    tk.Button(frame, text="Додати", command=add_product).grid(row=4, column=0)
    tk.Button(frame, text="Редагувати", command=edit_product).grid(row=4, column=1)
    tk.Button(frame, text="Видалити", command=delete_product).grid(row=4, column=2)

    table = ttk.Treeview(root, columns=["Назва", "Категорія", "Кількість", "Ціна"], show="headings")
    table.heading("Назва", text="Назва")
    table.heading("Категорія", text="Категорія")
    table.heading("Кількість", text="Кількість")
    table.heading("Ціна", text="Ціна")
    table.pack()

    tk.Button(root, text="Загальна кількість", command=show_total_quantity).pack()
    tk.Button(root, text="Вартість за категоріями", command=show_category_values).pack()
    tk.Button(root, text="Найдорожчий/Запас", command=show_most_expensive_and_stocked).pack()
    tk.Button(root, text="Графік: Ціна-Кількість", command=manager.scatter_plot).pack()
    tk.Button(root, text="Кругова діаграма", command=manager.pie_chart).pack()
    tk.Button(root, text="Гістограма цін", command=manager.histogram).pack()

    root.mainloop()

if __name__ == "__main__":
    main()
