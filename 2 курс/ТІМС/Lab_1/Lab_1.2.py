import math
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from tkinter import *
from tkinter import ttk, messagebox
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

def analyze_intervals(sorted_sample, n, k, max_value):
    num_classes = int(1 + 3.322 * math.log(n, 10))
    min_value = k
    h = (max_value - min_value) / num_classes
    intervals = [(min_value + i * h, min_value + (i + 1) * h) for i in range(num_classes)]
    intervals_counts = []
    
    for i, (lower, upper) in enumerate(intervals):
        if i == num_classes - 1:
            count = np.sum((sorted_sample >= lower) & (sorted_sample <= upper))
        else:
            count = np.sum((sorted_sample >= lower) & (sorted_sample < upper))
        intervals_counts.append(count)
    
    return num_classes, intervals, intervals_counts, h

def create_freq_table(intervals, intervals_counts, num_classes, n):
    table = pd.DataFrame({
        'Клас': [f'[{round(lower, 2)}, {round(upper, 2)})' if i < num_classes - 1 else f'[{round(lower, 2)}, {round(upper, 2)}]' 
                for i, (lower, upper) in enumerate(intervals)],
        'Частота': intervals_counts,
        'Омега': [round(count / num_classes, 1) for count in intervals_counts],
        'Відносна частота': [f'{count}/{n}' for count in intervals_counts]
    })
    table['Кумулятивна частота'] = table['Частота'].cumsum() / n
    return table

def calculate_statistics(sorted_sample, intervals, intervals_counts, n, h, num_classes):
    midpoints = [(lower + upper) / 2 for lower, upper in intervals]
    mean = sum(f * x for f, x in zip(intervals_counts, midpoints)) / n
    
    cumulative_freq = 0
    median_class_idx = 0
    for i, freq in enumerate(intervals_counts):
        cumulative_freq += freq
        if cumulative_freq >= n / 2:
            median_class_idx = i
            break
    l_median = intervals[median_class_idx][0]
    f_median = intervals_counts[median_class_idx]
    F_median = sum(intervals_counts[:median_class_idx])
    median = l_median + ((n / 2 - F_median) / f_median) * h if f_median != 0 else l_median

    max_freq_idx = np.argmax(intervals_counts)
    l_mode = intervals[max_freq_idx][0]
    f_m = intervals_counts[max_freq_idx]
    f_m1 = intervals_counts[max_freq_idx - 1] if max_freq_idx > 0 else 0
    f_m2 = intervals_counts[max_freq_idx + 1] if max_freq_idx < len(intervals_counts) - 1 else 0
    mode = l_mode + ((f_m - f_m1) / ((f_m - f_m1) + (f_m - f_m2))) * h if f_m > 0 and (f_m - f_m1) + (f_m - f_m2) != 0 else l_mode

    range_val = max(sorted_sample) - min(sorted_sample)
    deviation = sum(f * (x - mean) ** 2 for f, x in zip(intervals_counts, midpoints))
    variance = deviation / (n - 1)  
    standart = math.sqrt(variance)
    variation = standart / mean
    dispersion = deviation / n  
    sqrt_dispersion = math.sqrt(dispersion)
    
    initial_moment_1 = sum(f * x for f, x in zip(intervals_counts, midpoints)) / n
    central_moment_2 = sum(f * (x - mean) ** 2 for f, x in zip(intervals_counts, midpoints)) / n
    central_moment_3 = sum(f * (x - mean) ** 3 for f, x in zip(intervals_counts, midpoints)) / n
    central_moment_4 = sum(f * (x - mean) ** 4 for f, x in zip(intervals_counts, midpoints)) / n
    
    skewness = central_moment_3 / (central_moment_2 ** (3/2)) if central_moment_2 != 0 else 0
    kurtosis = central_moment_4 / (central_moment_2 ** 2) - 3 if central_moment_2 != 0 else 0

    result = (
        f"Кількість класів за формулою Стерджеса: {num_classes}\n"
        f"Середнє: {mean:.2f}\n"
        f"Медіана: {median:.2f}\n"
        f"Мода: {mode:.2f}\n"
        f"\n"
        f"Девіація: {deviation:.2f}\n"
        f"Розмах: {range_val:.2f}\n"  
        f"Варіанса: {variance:.2f}\n"
        f"Стандарт: {standart:.2f}\n"
        f"Коефіцієнт варіації: {variation:.2f}\n"
        f"\n"
        f"Дисперсія: {dispersion:.2f}\n"
        f"Середнє квадратичне: {sqrt_dispersion:.2f}\n"
        f"\n"
        f"Початковий момент 1: {initial_moment_1:.2f}\n"
        f"Центральний момент 2: {central_moment_2:.2f}\n"
        f"Центральний момент 3: {central_moment_3:.2f}\n"
        f"Центральний момент 4: {central_moment_4:.2f}\n"
        f"\n"
        f"Асиметрія: {skewness:.2f}\n"
        f"Ексцес: {kurtosis:.2f}\n"
    )
    return result

def plot_histogram(freq_table):
    fig = plt.figure(figsize=(5, 4))
    ax = fig.add_subplot(111)
    ax.bar(freq_table['Клас'], freq_table['Омега'], color='blue', edgecolor='black', width=1)
    ax.set_xlabel('Інтервали')
    ax.set_ylabel('Омега')
    ax.set_title('Гістограма (Інтервальне)')
    ax.tick_params(axis='x', rotation=10)
    ax.grid(True)
    return fig

def plot_ecdf(intervals, freq_table, k, max_value):
    fig = plt.figure(figsize=(5, 4))
    ax = fig.add_subplot(111)
    x_values = [lower for lower, upper in intervals]
    y_values = freq_table['Кумулятивна частота']
    ax.plot(x_values, y_values, label='Емпіричний розподіл', marker='o', linestyle='-', color='b')
    ax.set_xlabel('Інтервали')
    ax.set_ylabel('Кумулятивна частота')
    ax.set_title('Емпірична функція (Інтервальне)')
    ax.tick_params(axis='x')
    ax.grid(True)
    ax.legend()
    return fig

global_data = {
    "freq_table": None,
    "intervals": None,
    "k": None,
    "max_value": None,
    "canvas": None
}

def clear_graph_frame():
    for widget in graph_frame.winfo_children():
        widget.destroy()
    global_data["canvas"] = None

def show_histogram():
    clear_graph_frame()
    hist_fig = plot_histogram(global_data["freq_table"])
    canvas = FigureCanvasTkAgg(hist_fig, master=graph_frame)
    canvas.draw()
    canvas.get_tk_widget().pack(side=TOP, fill=BOTH, expand=1)
    global_data["canvas"] = canvas

def show_ecdf():
    clear_graph_frame()
    ecdf_fig = plot_ecdf(global_data["intervals"], global_data["freq_table"], global_data["k"], global_data["max_value"])
    canvas = FigureCanvasTkAgg(ecdf_fig, master=graph_frame)
    canvas.draw()
    canvas.get_tk_widget().pack(side=TOP, fill=BOTH, expand=1)
    global_data["canvas"] = canvas

def run_analysis():
    try:
        n = int(entry_n.get())
        k = float(entry_k.get())
        max_value = float(entry_max.get())
        
        if n < 50:
            messagebox.showerror("Помилка", "Розмір вибірки має бути не меншим за 50!")
            return
        if n <= 0:
            messagebox.showerror("Помилка", "Розмір вибірки має бути додатнім числом!")
            return
        if max_value <= k:
            messagebox.showerror("Помилка", "Максимальний елемент має бути більшим за мінімальний!")
            return
        
        sample = np.round(np.random.uniform(k, max_value, n), 1)
        sorted_sample = np.sort(sample)

        num_classes, intervals, intervals_counts, h = analyze_intervals(sorted_sample, n, k, max_value)
        freq_table = create_freq_table(intervals, intervals_counts, num_classes, n)
        stats = calculate_statistics(sorted_sample, intervals, intervals_counts, n, h, num_classes)
        
        result_text.delete(1.0, END)
        result_text.insert(END, "Згенерована вибірка:\n")
        result_text.insert(END, ", ".join([f"{x:.1f}" for x in sample]) + "\n\n")
        result_text.insert(END, "Посортований варіаційний ряд:\n")
        result_text.insert(END, ", ".join([f"{x:.1f}" for x in sorted_sample]) + "\n\n")
        result_text.insert(END, "Частотна таблиця:\n")
        result_text.insert(END, freq_table.to_string() + "\n\n")
        result_text.insert(END, "Статистичні характеристики:\n")
        result_text.insert(END, stats)
        
        global_data["freq_table"] = freq_table
        global_data["intervals"] = intervals
        global_data["k"] = k
        global_data["max_value"] = max_value
        
        show_histogram()
        
    except ValueError:
        messagebox.showerror("Помилка", "Будь ласка, введіть коректні числові значення!")

root = Tk()
root.title("Аналіз вибірки")
root.geometry("1200x700")

input_frame = Frame(root)
input_frame.pack(side=TOP, fill=X, padx=10, pady=10)

Label(input_frame, text="Розмір вибірки (n):").grid(row=0, column=0, padx=5, pady=5)
entry_n = Entry(input_frame)
entry_n.grid(row=0, column=1, padx=5, pady=5)

Label(input_frame, text="Мінімальний елемент (k):").grid(row=1, column=0, padx=5, pady=5)
entry_k = Entry(input_frame)
entry_k.grid(row=1, column=1, padx=5, pady=5)

Label(input_frame, text="Максимальний елемент:").grid(row=2, column=0, padx=5, pady=5)
entry_max = Entry(input_frame)
entry_max.grid(row=2, column=1, padx=5, pady=5)

Button(input_frame, text="Виконати аналіз", command=run_analysis).grid(row=3, column=0, columnspan=2, pady=10)

graph_buttons_frame = Frame(root)
graph_buttons_frame.pack(side=TOP, fill=X, padx=10, pady=5)

Button(graph_buttons_frame, text="Показати гістограму", command=show_histogram).pack(side=LEFT, padx=5)
Button(graph_buttons_frame, text="Показати емпіричну функцію", command=show_ecdf).pack(side=LEFT, padx=5)

main_frame = Frame(root)
main_frame.pack(fill=BOTH, expand=True, padx=10, pady=10)

left_frame = Frame(main_frame)
left_frame.pack(side=LEFT, fill=Y, padx=5)

result_text = Text(left_frame, height=60, width=80)
result_text.pack(fill=Y)

graph_frame = Frame(main_frame)
graph_frame.pack(side=RIGHT, fill=BOTH, expand=True, padx=5)

root.mainloop()