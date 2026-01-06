import numpy as np
import matplotlib.pyplot as plt
from scipy.stats import poisson, chi2
import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

def process_data():
    try:
        result_text.delete(1.0, tk.END)
        
        with open('C:/ТІМС/Lab_2/sample14.txt', 'r') as file:
            data = [int(x) for x in file.read().split()]
        
        n = len(data)
        lambda_auto = np.mean(data)
        
        if lambda_var.get() == "manual":
            lambda_est = float(lambda_entry.get())
            if lambda_est <= 0:
                raise ValueError("λ має бути додатним числом!")
        else:
            lambda_est = lambda_auto
        
        alpha = float(alpha_entry.get())
        
        result_text.insert(tk.END, f"Розмір вибірки: {n}\n")
        result_text.insert(tk.END, f"Оцінене λ (за вибіркою): {lambda_auto:.3f}\n")
        result_text.insert(tk.END, f"Використовується λ = {lambda_est:.3f}\n")
        result_text.insert(tk.END, f"Рівень значущості α: {alpha}\n\n")
        
        values, observed = np.unique(data, return_counts=True)
        probabilities = [poisson.pmf(k, lambda_est) for k in values]  
        expected = [n * p for p in probabilities]  
        
        # Display the initial table
        result_text.insert(tk.END, "Початкова таблиця:\n")
        result_text.insert(tk.END, "x_i\tm_i\tp_i\t n p_i\n")
        result_text.insert(tk.END, "-" * 40 + "\n")
        for v, o, p, e in zip(values, observed, probabilities, expected):
            result_text.insert(tk.END, f"{v:<8}{o:<8}{p:.4f}\t  {e:.2f}\n")
        result_text.insert(tk.END, "\n")
        
        # Adjust the tables
        values_adj = list(values)
        observed_adj = list(observed)
        probabilities_adj = list(probabilities)
        expected_adj = list(expected)
        
        i = 0
        while i < len(expected_adj) - 1:
            if expected_adj[i] < 5 or observed_adj[i] < 5:
                expected_adj[i + 1] += expected_adj[i]
                observed_adj[i + 1] += observed_adj[i]
                probabilities_adj[i + 1] += probabilities_adj[i]
                expected_adj.pop(i)
                observed_adj.pop(i)
                probabilities_adj.pop(i)
                values_adj.pop(i)
            else:
                i += 1
        
        if len(expected_adj) > 1 and (expected_adj[-1] < 5 or observed_adj[-1] < 5):
            expected_adj[-2] += expected_adj[-1]
            observed_adj[-2] += observed_adj[-1]
            probabilities_adj[-2] += probabilities_adj[-1]
            expected_adj.pop()
            observed_adj.pop()
            probabilities_adj.pop()
            values_adj.pop()
        
        # Display the final table
        result_text.insert(tk.END, "Кінцева таблиця (після об'єднання):\n")
        result_text.insert(tk.END, "x_i\tm_i\tp_i\t n p_i\n")
        result_text.insert(tk.END, "-" * 40 + "\n")
        for v, o, p, e in zip(values_adj, observed_adj, probabilities_adj, expected_adj):
            result_text.insert(tk.END, f"{v:<8}{o:<8}{p:.4f}\t  {e:.2f}\n")
        result_text.insert(tk.END, "\n")
        
        for widget in graph_frame.winfo_children():
            widget.destroy()
        fig, ax = plt.subplots(figsize=(6, 4))
        ax.bar(values, observed, width=0.8, alpha=0.7, label='Спостереження', align='center')
        x_range = range(min(data), max(data) + 1)
        ax.plot(x_range, [n * poisson.pmf(x, lambda_est) for x in x_range], 'r-', label='Пуассон')
        ax.set_xlabel('Значення')
        ax.set_ylabel('Частота')
        ax.legend()
        ax.set_title('Порівняння вибірки з розподілом Пуассона')
        canvas = FigureCanvasTkAgg(fig, master=graph_frame)
        canvas.draw()
        canvas.get_tk_widget().pack()
        
        result_text.insert(tk.END, "\nНа основі графіку: вибірка нагадує розподіл Пуассона\n")
        result_text.insert(tk.END, f"H_0: Дані відповідають розподілу Пуассона з λ = {lambda_est:.3f}\n")
        
        chi_squared = sum((o - e) ** 2 / e for o, e in zip(observed_adj, expected_adj))
        df = len(observed_adj) - 1 - 1
        critical_value = chi2.ppf(1 - alpha, df)
        
        result_text.insert(tk.END, f"\nχ²(Емпіричне значення) = {chi_squared:.2f}\n")
        result_text.insert(tk.END, f"df(Ступені свободи): {df}\n")
        result_text.insert(tk.END, f"χ²(Критичне значення) (α = {alpha}; df = {df}): {critical_value:.4f}\n")
        result_text.insert(tk.END, f"Висновок: {f'χ²(Емп) > χ²(Кр) - гіпотезу відхиляємо' if chi_squared > critical_value else f'χ²(Емп) < χ²(Кр) - гіпотезу приймаємо'}\n")
        
    except Exception as e:
        messagebox.showerror("Помилка", str(e))


root = tk.Tk()
root.title("Аналіз розподілу Пуассона")
root.geometry("800x600")

tk.Label(root, text="Вибір параметра λ:").pack()
lambda_var = tk.StringVar(value="auto")
tk.Radiobutton(root, text="Автоматично (середнє вибірки)", variable=lambda_var, value="auto").pack()
tk.Radiobutton(root, text="Вручну", variable=lambda_var, value="manual").pack()
tk.Label(root, text="Введіть λ (якщо вручну):").pack()
lambda_entry = tk.Entry(root, width=10)
lambda_entry.pack()

tk.Label(root, text="Рівень значущості α:").pack()
alpha_entry = tk.Entry(root, width=10)
alpha_entry.insert(0, "0.05")
alpha_entry.pack()

tk.Button(root, text="Обробити дані", command=process_data).pack(pady=10)

graph_frame = tk.Frame(root)
graph_frame.pack(fill=tk.BOTH, expand=True)

result_text = scrolledtext.ScrolledText(root, width=80, height=20)  
result_text.pack(pady=10)

root.mainloop()