import numpy as np
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import tkinter as tk
from tkinter import ttk, scrolledtext
from scipy.stats import f, t

# Дані
x_values = np.array([2, 3, 5, 6, 8, 10, 12])
y_values = np.array([2, 3, 5, 7, 12, 13])
n_ij = np.array([
    [0, 0, 0, 0, 0, 22, 2],
    [0, 0, 0, 4, 13, 0, 0],
    [0, 2, 3, 14, 5, 0, 0],
    [0, 4, 21, 0, 0, 0, 0],
    [3, 14, 0, 0, 0, 0, 0],
    [12, 0, 0, 0, 0, 0, 0]
])
n_i = np.array([15, 20, 24, 18, 18, 22, 2])
n = np.sum(n_i)

# Створення головного вікна
root = tk.Tk()
root.title("Регресійний аналіз")
root.state('zoomed')  
root.grid_rowconfigure(0, weight=2)
root.grid_rowconfigure(1, weight=3)
root.grid_rowconfigure(2, weight=1)
root.grid_rowconfigure(3, weight=1)
root.grid_columnconfigure(0, weight=1)
root.grid_columnconfigure(1, weight=1)

# Таблиця
frame_table = ttk.LabelFrame(root, text="Кореляційна таблиця")
frame_table.grid(row=0, column=0, padx=5, pady=5, sticky="nsew")

tree = ttk.Treeview(frame_table, height=len(y_values)+1)
tree["columns"] = ["Y \\ X"] + list(x_values) + ["m_j"]
tree.column("#0", width=0, stretch=tk.NO)
tree.column("Y \\ X", width=50)
for x in x_values:
    tree.column(x, width=50)
tree.column("m_j", width=50)
tree.heading("Y \\ X", text="Y \\ X")
for x in x_values:
    tree.heading(x, text=str(x))
tree.heading("m_j", text="m_j")

for i, y in enumerate(y_values):
    row = [y] + list(n_ij[i, :]) + [np.sum(n_ij[i, :])]
    tree.insert("", tk.END, values=row)
tree.insert("", tk.END, values=["n_i"] + list(n_i) + [n])
tree.grid(row=0, column=0, padx=5, pady=5)

# Результати
frame_results = ttk.LabelFrame(root, text="Результати")
frame_results.grid(row=0, column=1, padx=5, pady=5, sticky="nsew")
text_results = scrolledtext.ScrolledText(frame_results, width=60, height=20)
text_results.grid(row=0, column=0, padx=5, pady=5)

# Графіки
frame_plot = ttk.LabelFrame(root, text="Графіки")
frame_plot.grid(row=1, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
fig, ax = plt.subplots(figsize=(10, 6))
canvas = FigureCanvasTkAgg(fig, master=frame_plot)
canvas.get_tk_widget().grid(row=0, column=0, padx=5, pady=5)

# Прогноз
frame_predict = ttk.LabelFrame(root, text="Прогноз")
frame_predict.grid(row=2, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
tk.Label(frame_predict, text="Введіть x*:").grid(row=0, column=0, padx=5)
entry_x_star = ttk.Entry(frame_predict, width=10)
entry_x_star.grid(row=0, column=1, padx=5)
entry_x_star.insert(0, "12")
tk.Label(frame_predict, text="Рівень значущості α (0-1):").grid(row=0, column=2, padx=5)
entry_alpha = ttk.Entry(frame_predict, width=10)
entry_alpha.grid(row=0, column=3, padx=5)
entry_alpha.insert(0, "0.05")  # Значення за замовчуванням
predict_button = ttk.Button(frame_predict, text="Прогнозувати")
predict_button.grid(row=0, column=4, padx=5)

# Глобальні змінні
y_bar_x = None
a_linear, b_linear = None, None
R2_linear, F_emp_linear, Q_o_linear = None, None, None
r = None
a_nonlin, b_nonlin, c_nonlin = None, None, None
R2_nonlin, F_emp_nonlin, Q_o_nonlin = None, None, None
a_hyper, b_hyper = None, None
R2_hyper, F_emp_hyper, Q_o_hyper = None, None, None
a_exp, b_exp = None, None
R2_exp, F_emp_exp, Q_o_exp = None, None, None
a_sqrt, b_sqrt = None, None
R2_sqrt, F_emp_sqrt, Q_o_sqrt = None, None, None
alpha = 0.05  # Значення за замовчуванням

def display_results(message):
    text_results.delete(1.0, tk.END)
    text_results.insert(tk.END, message)

# Умовні середні
def compute_conditional_means():
    global y_bar_x
    y_bar_x = np.zeros(len(x_values))
    for i in range(len(x_values)):
        if n_i[i] == 0:
            display_results("Помилка: n_i не може бути 0")
            return
        y_bar_x[i] = np.sum(y_values * n_ij[:, i]) / n_i[i]
    result = "Умовні середні y_bar_x:\n" + str(y_bar_x) + "\n"
    result += "Тип регресії: спадна криволінійна (на основі поля кореляції)\n"
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    ax.plot(x_values, y_bar_x, color='green', linestyle='--', label='Емпірична лінія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Лінійна регресія
def compute_linear_regression():
    global y_bar_x, a_linear, b_linear, R2_linear, F_emp_linear, Q_o_linear, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    A = np.array([
        [np.sum(x_values**2 * n_i), np.sum(x_values * n_i)],
        [np.sum(x_values * n_i), np.sum(n_i)]
    ])
    B = np.array([
        np.sum(x_values * n_i * y_bar_x),
        np.sum(n_i * y_bar_x)
    ])
    a_linear, b_linear = np.linalg.solve(A, B)
    
    y_star = a_linear * x_values + b_linear
    y_mean = np.sum(n_i * y_bar_x) / n
    Q = np.sum(n_i * (y_bar_x - y_mean)**2)
    Q_p = np.sum(n_i * (y_star - y_mean)**2)
    Q_o_linear = np.sum(n_i * (y_bar_x - y_star)**2)
    R2_linear = Q_p / Q if Q > 0 else 0
    if R2_linear > 1 or R2_linear < 0:
        R2_linear = max(0, min(1, R2_linear))
        display_results("Попередження: R^2 скориговано до [0; 1]")
    
    m = 2
    F_emp_linear = (Q_p * (n - m)) / (Q_o_linear * (m - 1)) if Q_o_linear > 0 else float('inf')
    F_crit = f.ppf(1 - alpha, m-1, n-m)
    
    delta = sum(n_ij[i, j] * (y_values[i] - (a_linear * x_values[j] + b_linear))**2 
                for i in range(len(y_values)) for j in range(len(x_values)))
    sigma2 = delta / n
    delta2 = Q_o_linear
    
    result = (f"Лінійна регресія: y = {a_linear:.4f}x + {b_linear:.4f}\n"
              f"Q: {Q:.4f}, Q_p: {Q_p:.4f}, Q_o: {Q_o_linear:.4f}\n"
              f"R^2: {R2_linear:.4f}\n"
              f"F емпіричне: {F_emp_linear:.4f}, F критичне (α={alpha}): {F_crit:.4f}\n"
              f"Модель {'адекватна' if F_emp_linear > F_crit else 'неадекватна'}\n"
              f"Дисперсія: σ^2 = {sigma2:.4f}\n"
              f"Сума квадратів відхилень: δ^2 = {delta2:.4f}\n")
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    ax.plot(x_values, y_star, color='red', label='Лінійна регресія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Коефіцієнт кореляції
def compute_correlation():
    global r, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    y_mean = np.sum(n_i * y_bar_x) / n
    x_mean = np.sum(x_values * n_i) / n
    cov_xy = np.sum([n_ij[i, j] * (x_values[j] - x_mean) * (y_values[i] - y_mean) 
                     for i in range(len(y_values)) for j in range(len(x_values))])
    var_x = np.sum(n_i * (x_values - x_mean)**2)
    var_y = np.sum([np.sum(n_ij[i, :]) * (y_values[i] - y_mean)**2 for i in range(len(y_values))])
    r = cov_xy / np.sqrt(var_x * var_y) if var_x * var_y > 0 else 0
    
    t_emp = r * np.sqrt(n - 2) / np.sqrt(1 - r**2) if (1 - r**2) > 0 else float('inf')
    t_crit = t.ppf(1 - alpha/2, n - 2)  # Двосторонній критерій
    
    result = (f"Коефіцієнт кореляції r: {r:.4f}\n"
              f"t емпіричне: {t_emp:.4f}, t критичне (α={alpha}): {t_crit:.4f}\n"
              f"Коефіцієнт {'значущий' if abs(t_emp) > t_crit else 'незначущий'}\n")
    display_results(result)

# Параболічна регресія
def compute_nonlinear_regression():
    global y_bar_x, a_nonlin, b_nonlin, c_nonlin, R2_nonlin, F_emp_nonlin, Q_o_nonlin, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    A_nonlin = np.array([
        [np.sum(n_i * x_values**4), np.sum(n_i * x_values**3), np.sum(n_i * x_values**2)],
        [np.sum(n_i * x_values**3), np.sum(n_i * x_values**2), np.sum(n_i * x_values)],
        [np.sum(n_i * x_values**2), np.sum(n_i * x_values), n]
    ])
    B_nonlin = np.array([
        np.sum(n_i * y_bar_x * x_values**2),
        np.sum(n_i * y_bar_x * x_values),
        np.sum(n_i * y_bar_x)
    ])
    a_nonlin, b_nonlin, c_nonlin = np.linalg.solve(A_nonlin, B_nonlin)
    
    y_star_nonlin = a_nonlin * x_values**2 + b_nonlin * x_values + c_nonlin
    y_mean = np.sum(n_i * y_bar_x) / n
    Q = np.sum(n_i * (y_bar_x - y_mean)**2)
    Q_p_nonlin = np.sum(n_i * (y_star_nonlin - y_mean)**2)
    Q_o_nonlin = np.sum(n_i * (y_bar_x - y_star_nonlin)**2)
    R2_nonlin = Q_p_nonlin / Q if Q > 0 else 0
    if R2_nonlin > 1 or R2_nonlin < 0:
        R2_nonlin = max(0, min(1, R2_nonlin))
        display_results("Попередження: R^2 скориговано до [0; 1]")
    
    m_nonlin = 3
    F_emp_nonlin = (Q_p_nonlin * (n - m_nonlin)) / (Q_o_nonlin * (m_nonlin - 1)) if Q_o_nonlin > 0 else float('inf')
    F_crit_nonlin = f.ppf(1 - alpha, m_nonlin-1, n-m_nonlin)
    
    delta = sum(n_ij[i, j] * (y_values[i] - (a_nonlin * x_values[j]**2 + b_nonlin * x_values[j] + c_nonlin))**2 
                for i in range(len(y_values)) for j in range(len(x_values)))
    sigma2 = delta / n
    delta2 = Q_o_nonlin
    
    result = (f"Параболічна регресія: y = {a_nonlin:.4f}x^2 + {b_nonlin:.4f}x + {c_nonlin:.4f}\n"
              f"Q: {Q:.4f}, Q_p: {Q_p_nonlin:.4f}, Q_o: {Q_o_nonlin:.4f}\n"
              f"R^2: {R2_nonlin:.4f}\n"
              f"F емпіричне: {F_emp_nonlin:.4f}, F критичне (α={alpha}): {F_crit_nonlin:.4f}\n"
              f"Модель {'адекватна' if F_emp_nonlin > F_crit_nonlin else 'неадекватна'}\n"
              f"Дисперсія: σ^2 = {sigma2:.4f}\n"
              f"Сума квадратів відхилень: δ^2 = {delta2:.4f}\n")
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    x_smooth = np.linspace(min(x_values), max(x_values), 100)
    y_smooth = a_nonlin * x_smooth**2 + b_nonlin * x_smooth + c_nonlin
    ax.plot(x_smooth, y_smooth, color='purple', label='Параболічна регресія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Гіперболічна регресія
def compute_hyperbolic_regression():
    global y_bar_x, a_hyper, b_hyper, R2_hyper, F_emp_hyper, Q_o_hyper, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    A_hyper = np.array([
        [np.sum(n_i / x_values**2), np.sum(n_i / x_values)],
        [np.sum(n_i / x_values), n]
    ])
    B_hyper = np.array([
        np.sum(n_i * y_bar_x / x_values),
        np.sum(n_i * y_bar_x)
    ])
    a_hyper, b_hyper = np.linalg.solve(A_hyper, B_hyper)
    
    y_star_hyper = a_hyper / x_values + b_hyper
    y_mean = np.sum(n_i * y_bar_x) / n
    Q = np.sum(n_i * (y_bar_x - y_mean)**2)
    Q_p_hyper = np.sum(n_i * (y_star_hyper - y_mean)**2)
    Q_o_hyper = np.sum(n_i * (y_bar_x - y_star_hyper)**2)
    R2_hyper = Q_p_hyper / Q if Q > 0 else 0
    if R2_hyper > 1 or R2_hyper < 0:
        R2_hyper = max(0, min(1, R2_hyper))
        display_results("Попередження: R^2 скориговано до [0; 1]")
    
    m = 2
    F_emp_hyper = (Q_p_hyper * (n - m)) / (Q_o_hyper * (m - 1)) if Q_o_hyper > 0 else float('inf')
    F_crit = f.ppf(1 - alpha, m-1, n-m)
    
    delta = sum(n_ij[i, j] * (y_values[i] - (a_hyper / x_values[j] + b_hyper))**2 
                for i in range(len(y_values)) for j in range(len(x_values)))
    sigma2 = delta / n
    delta2 = Q_o_hyper
    
    result = (f"Гіперболічна регресія: y = {a_hyper:.4f}/x + {b_hyper:.4f}\n"
              f"Q: {Q:.4f}, Q_p: {Q_p_hyper:.4f}, Q_o: {Q_o_hyper:.4f}\n"
              f"R^2: {R2_hyper:.4f}\n"
              f"F емпіричне: {F_emp_hyper:.4f}, F критичне (α={alpha}): {F_crit:.4f}\n"
              f"Модель {'адекватна' if F_emp_hyper > F_crit else 'неадекватна'}\n"
              f"Дисперсія: σ^2 = {sigma2:.4f}\n"
              f"Сума квадратів відхилень: δ^2 = {delta2:.4f}\n")
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    x_smooth = np.linspace(min(x_values), max(x_values), 100)
    y_smooth = a_hyper / x_smooth + b_hyper
    ax.plot(x_smooth, y_smooth, color='orange', label='Гіперболічна регресія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Показникова регресія
def compute_exponential_regression():
    global y_bar_x, a_exp, b_exp, R2_exp, F_emp_exp, Q_o_exp, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    A_exp = np.array([
        [np.sum(x_values**2 * n_i), np.sum(x_values * n_i)],
        [np.sum(x_values * n_i), np.sum(n_i)]
    ])
    B_exp = np.array([
        np.sum(n_i * x_values * np.log(y_bar_x)),
        np.sum(n_i * np.log(y_bar_x))
    ])
    b_exp, ln_a = np.linalg.solve(A_exp, B_exp)
    a_exp = np.exp(ln_a)
    
    y_star_exp = a_exp * np.exp(b_exp * x_values)
    y_mean = np.sum(n_i * y_bar_x) / n
    Q = np.sum(n_i * (y_bar_x - y_mean)**2)
    Q_p_exp = np.sum(n_i * (y_star_exp - y_mean)**2)
    Q_o_exp = np.sum(n_i * (y_bar_x - y_star_exp)**2)
    R2_exp = Q_p_exp / Q if Q > 0 else 0
    if R2_exp > 1 or R2_exp < 0:
        R2_exp = max(0, min(1, R2_exp))
        display_results("Попередження: R^2 скориговано до [0; 1]")
    
    m = 2
    F_emp_exp = (Q_p_exp * (n - m)) / (Q_o_exp * (m - 1)) if Q_o_exp > 0 else float('inf')
    F_crit = f.ppf(1 - alpha, m-1, n-m)
    
    delta = sum(n_ij[i, j] * (y_values[i] - (a_exp * np.exp(b_exp * x_values[j])))**2 
                for i in range(len(y_values)) for j in range(len(x_values)))
    sigma2 = delta / n
    delta2 = Q_o_exp
    
    result = (f"Показникова регресія: y = {a_exp:.4f}e^({b_exp:.4f}x)\n"
              f"Q: {Q:.4f}, Q_p: {Q_p_exp:.4f}, Q_o: {Q_o_exp:.4f}\n"
              f"R^2: {R2_exp:.4f}\n"
              f"F емпіричне: {F_emp_exp:.4f}, F критичне (α={alpha}): {F_crit:.4f}\n"
              f"Модель {'адекватна' if F_emp_exp > F_crit else 'неадекватна'}\n"
              f"Дисперсія: σ^2 = {sigma2:.4f}\n"
              f"Сума квадратів відхилень: δ^2 = {delta2:.4f}\n")
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    x_smooth = np.linspace(min(x_values), max(x_values), 100)
    y_smooth = a_exp * np.exp(b_exp * x_smooth)
    ax.plot(x_smooth, y_smooth, color='cyan', label='Показникова регресія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Коренева регресія
def compute_sqrt_regression():
    global y_bar_x, a_sqrt, b_sqrt, R2_sqrt, F_emp_sqrt, Q_o_sqrt, alpha
    if y_bar_x is None:
        compute_conditional_means()
    
    A_sqrt = np.array([
        [np.sum(n_i * np.sqrt(x_values)**2), np.sum(n_i * np.sqrt(x_values))],
        [np.sum(n_i * np.sqrt(x_values)), np.sum(n_i)]
    ])
    B_sqrt = np.array([
        np.sum(n_i * np.sqrt(x_values) * y_bar_x),
        np.sum(n_i * y_bar_x)
    ])
    a_sqrt, b_sqrt = np.linalg.solve(A_sqrt, B_sqrt)
    
    y_star_sqrt = a_sqrt * np.sqrt(x_values) + b_sqrt
    y_mean = np.sum(n_i * y_bar_x) / n
    Q = np.sum(n_i * (y_bar_x - y_mean)**2)
    Q_p_sqrt = np.sum(n_i * (y_star_sqrt - y_mean)**2)
    Q_o_sqrt = np.sum(n_i * (y_bar_x - y_star_sqrt)**2)
    R2_sqrt = Q_p_sqrt / Q if Q > 0 else 0
    if R2_sqrt > 1 or R2_sqrt < 0:
        R2_sqrt = max(0, min(1, R2_sqrt))
        display_results("Попередження: R^2 скориговано до [0; 1]")
    
    m = 2
    F_emp_sqrt = (Q_p_sqrt * (n - m)) / (Q_o_sqrt * (m - 1)) if Q_o_sqrt > 0 else float('inf')
    F_crit = f.ppf(1 - alpha, m-1, n-m)
    
    delta = sum(n_ij[i, j] * (y_values[i] - (a_sqrt * np.sqrt(x_values[j]) + b_sqrt))**2 
                for i in range(len(y_values)) for j in range(len(x_values)))
    sigma2 = delta / n
    delta2 = Q_o_sqrt
    
    result = (f"Коренева регресія: y = {a_sqrt:.4f}√x + {b_sqrt:.4f}\n"
              f"Q: {Q:.4f}, Q_p: {Q_p_sqrt:.4f}, Q_o: {Q_o_sqrt:.4f}\n"
              f"R^2: {R2_sqrt:.4f}\n"
              f"F емпіричне: {F_emp_sqrt:.4f}, F критичне (α={alpha}): {F_crit:.4f}\n"
              f"Модель {'адекватна' if F_emp_sqrt > F_crit else 'неадекватна'}\n"
              f"Дисперсія: σ^2 = {sigma2:.4f}\n"
              f"Сума квадратів відхилень: δ^2 = {delta2:.4f}\n")
    display_results(result)
    
    ax.clear()
    ax.scatter(x_values, y_bar_x, color='blue', label='Поле кореляції', s=100)
    x_smooth = np.linspace(min(x_values), max(x_values), 100)
    y_smooth = a_sqrt * np.sqrt(x_smooth) + b_sqrt
    ax.plot(x_smooth, y_smooth, color='magenta', label='Коренева регресія')
    ax.set_xlabel('X', fontsize=12)
    ax.set_ylabel('Y', fontsize=12)
    ax.legend(fontsize=10)
    ax.grid(True)
    canvas.draw()

# Прогноз
def predict():
    global a_linear, b_linear, a_nonlin, b_nonlin, c_nonlin, a_hyper, b_hyper
    global a_exp, b_exp, a_sqrt, b_sqrt
    global R2_linear, R2_nonlin, R2_hyper, R2_exp, R2_sqrt, Q_o_linear, Q_o_nonlin, Q_o_hyper, Q_o_exp, Q_o_sqrt
    global alpha
    try:
        x_star = float(entry_x_star.get())
        alpha_input = float(entry_alpha.get())
        if not 0 < alpha_input < 1:
            display_results("Помилка: рівень значущості α має бути в межах (0, 1)")
            return
        alpha = alpha_input
    except ValueError:
        display_results("Помилка: введіть числові значення для x* та α")
        return
    
    y_pred_linear = "Не обчислено"
    y_pred_nonlin = "Не обчислено"
    y_pred_hyper = "Не обчислено"
    y_pred_exp = "Не обчислено"
    y_pred_sqrt = "Не обчислено"
    chosen_model = "Не визначено"
    chosen_R2 = None

    if a_linear is not None and b_linear is not None:
        y_pred_linear = a_linear * x_star + b_linear
    if a_nonlin is not None and b_nonlin is not None and c_nonlin is not None:
        y_pred_nonlin = a_nonlin * x_star**2 + b_nonlin * x_star + c_nonlin
    if a_hyper is not None and b_hyper is not None:
        y_pred_hyper = a_hyper / x_star + b_hyper
    if a_exp is not None and b_exp is not None:
        y_pred_exp = a_exp * np.exp(b_exp * x_star)
    if a_sqrt is not None and b_sqrt is not None:
        y_pred_sqrt = a_sqrt * np.sqrt(x_star) + b_sqrt

    R2_values = []
    Q_o_values = []
    models = []
    if R2_linear is not None:
        R2_values.append(R2_linear)
        Q_o_values.append(Q_o_linear if 'Q_o_linear' in globals() and Q_o_linear is not None else float('inf'))
        models.append("лінійна")
    if R2_nonlin is not None:
        R2_values.append(R2_nonlin)
        Q_o_values.append(Q_o_nonlin if 'Q_o_nonlin' in globals() and Q_o_nonlin is not None else float('inf'))
        models.append("параболічна")
    if R2_hyper is not None:
        R2_values.append(R2_hyper)
        Q_o_values.append(Q_o_hyper if 'Q_o_hyper' in globals() and Q_o_hyper is not None else float('inf'))
        models.append("гіперболічна")
    if R2_exp is not None:
        R2_values.append(R2_exp)
        Q_o_values.append(Q_o_exp if 'Q_o_exp' in globals() and Q_o_exp is not None else float('inf'))
        models.append("показникова")
    if R2_sqrt is not None:
        R2_values.append(R2_sqrt)
        Q_o_values.append(Q_o_sqrt if 'Q_o_sqrt' in globals() and Q_o_sqrt is not None else float('inf'))
        models.append("коренева")
    
    if R2_values:
        max_R2_index = np.argmax(R2_values)
        chosen_R2 = R2_values[max_R2_index]
        chosen_model = models[max_R2_index]

        # Якщо різниця між максимальним R^2 і іншими мала (менше 0.01)
        max_R2 = max(R2_values)
        if any(abs(max_R2 - r) < 0.01 for r in R2_values if r != max_R2):
            # Вибираємо модель із найменшим Q_o
            valid_indices = [i for i, r in enumerate(R2_values) if abs(max_R2 - r) < 0.01]
            min_Q_o_index = valid_indices[np.argmin([Q_o_values[i] for i in valid_indices])]
            chosen_model = models[min_Q_o_index]
            chosen_R2 = R2_values[min_Q_o_index]
    
    result = f"Прогноз для x* = {x_star} (α = {alpha}):\n"
    result += f"Лінійна модель: y* = {y_pred_linear if isinstance(y_pred_linear, str) else f'{y_pred_linear:.4f}'}\n"
    result += f"Параболічна модель: y* = {y_pred_nonlin if isinstance(y_pred_nonlin, str) else f'{y_pred_nonlin:.4f}'}\n"
    result += f"Гіперболічна модель: y* = {y_pred_hyper if isinstance(y_pred_hyper, str) else f'{y_pred_hyper:.4f}'}\n"
    result += f"Показникова модель: y* = {y_pred_exp if isinstance(y_pred_exp, str) else f'{y_pred_exp:.4f}'}\n"
    result += f"Коренева модель: y* = {y_pred_sqrt if isinstance(y_pred_sqrt, str) else f'{y_pred_sqrt:.4f}'}\n"
    result += f"Вибрана модель: {chosen_model}"
    result += f" (R^2 = {chosen_R2:.4f})" if chosen_R2 is not None else " (R^2 = не визначено)\n"
    
    display_results(result)

# Кнопки
frame_buttons = ttk.LabelFrame(root, text="Дії")
frame_buttons.grid(row=3, column=0, columnspan=2, padx=5, pady=5, sticky="nsew")
ttk.Button(frame_buttons, text="Умовні середні", command=compute_conditional_means).grid(row=0, column=0, padx=5)
ttk.Button(frame_buttons, text="Лінійна регресія", command=compute_linear_regression).grid(row=0, column=1, padx=5)
ttk.Button(frame_buttons, text="Коефіцієнт кореляції", command=compute_correlation).grid(row=0, column=2, padx=5)
ttk.Button(frame_buttons, text="Параболічна регресія", command=compute_nonlinear_regression).grid(row=0, column=3, padx=5)
ttk.Button(frame_buttons, text="Гіперболічна регресія", command=compute_hyperbolic_regression).grid(row=0, column=4, padx=5)
ttk.Button(frame_buttons, text="Показникова регресія", command=compute_exponential_regression).grid(row=0, column=5, padx=5)
ttk.Button(frame_buttons, text="Коренева регресія", command=compute_sqrt_regression).grid(row=0, column=6, padx=5)
predict_button.configure(command=predict)

root.mainloop()