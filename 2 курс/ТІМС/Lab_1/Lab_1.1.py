import numpy as np
import matplotlib.pyplot as plt
from collections import Counter
import tkinter as tk
from tkinter import scrolledtext, messagebox

class StatisticalAnalyzer:
    """Клас для статистичного аналізу випадкової вибірки"""
    
    def __init__(self):
        self.sample = None
        self.values = None
        self.frequencies = None
        self.rel_freqs = None
        self.bins = None
        self.hist = None
        self.bin_edges = None
        self.rel_hist = None

    def calculate_mode(self, freq_dict):
        """Обчислює моду для дискретного розподілу"""
        if not freq_dict:
            return None
        max_freq = max(freq_dict.values())
        modes = [int(val) for val, freq in freq_dict.items() if freq == max_freq]  # Конвертація в int
        return modes if len(modes) > 1 else modes[0]

    def calculate_grouped_mode(self):
        """Обчислює моду для інтервальни даних"""
        if self.hist is None or not self.hist.any():
            return None
        max_idx = np.argmax(self.hist)
        L = self.bin_edges[max_idx]
        h = self.bin_edges[1] - self.bin_edges[0]
        f_m = self.hist[max_idx]
        f_prev = self.hist[max_idx - 1] if max_idx > 0 else 0
        f_next = self.hist[max_idx + 1] if max_idx < len(self.hist) - 1 else 0
        denominator = (f_m - f_prev) + (f_m - f_next)
        return L + (f_m - f_prev) / denominator * h if denominator != 0 else L

    def calculate_cumulative_frequencies(self):
        """Обчислює кумулятивні частоти"""
        return np.cumsum(self.frequencies) / sum(self.frequencies) if self.frequencies else np.array([])

    def calculate_grouped_quartile(self, n, quartile):
        """Обчислює медіану для інтервальних даних"""
        if self.hist is None or not self.hist.any():
            return None
        target = n * quartile
        cum_hist = np.cumsum(self.hist)
        idx = np.searchsorted(cum_hist, target)
        if idx == 0:
            return self.bin_edges[0]
        L = self.bin_edges[idx]
        h = self.bin_edges[1] - self.bin_edges[0]
        F_prev = cum_hist[idx - 1] if idx > 0 else 0
        f = self.hist[idx]
        return L + (target - F_prev) / f * h if f != 0 else L

    def get_quantiles(self, n, sorted_sample):
        """Обчислює квантилі для дискретного розподілу"""
        valid_divisors = [4, 8, 10, 100, 1000]
        if not any(n % d == 0 for d in valid_divisors):
            return "Квантилі для цього обсягу вибірки не існують"
        quantiles = {}
        if n % 4 == 0:
            quantiles["quartiles"] = [int(sorted_sample[n * i // 4 - 1]) for i in range(1, 4)]
            quantiles["interquartile_range"] = max(quantiles["quartiles"]) - min(quantiles["quartiles"])
        if n % 8 == 0:
            quantiles["octiles"] = [int(sorted_sample[n * i // 8 - 1]) for i in range(1, 8)]
            quantiles["interoctile_range"] = max(quantiles["octiles"]) - min(quantiles["octiles"])
        if n % 10 == 0:
            quantiles["deciles"] = [int(sorted_sample[n * i // 10 - 1]) for i in range(1, 10)]
            quantiles["interdeciles_range"] = max(quantiles["deciles"]) - min(quantiles["deciles"])
        if n % 100 == 0:
            quantiles["centella"] = [int(sorted_sample[n * i // 100 - 1]) for i in range(1, 100)]
            quantiles["interdeciles_range"] = max(quantiles["centella"]) - min(quantiles["centella"])
        if n % 1000 == 0:
            quantiles["milliles"] = [int(sorted_sample[n * i // 1000 - 1]) for i in range(1, 1000)]
            quantiles["intermilliles_range"] = max(quantiles["milliles"]) - min(quantiles["milliles"])
        return quantiles

    def format_quantiles(self, quantiles):
        """Форматує квантилі для виведення"""
        if isinstance(quantiles, str):
            return quantiles + "\n"
        result = "Квантилі:\n"
        for q_type, values in quantiles.items():
            if q_type.startswith("inter"):
                result += f"  Між{q_type.split('_')[1][:-1]}ний розмах: {values:.2f}\n"
            else:
                result += f"  {q_type.capitalize()}: {values}\n"
        return result

    def analyze(self, k, n, result_text):
        """Виконує основний аналіз вибірки"""
        try:
            k = int(k) 
            n = int(n)  
            if n < 50:
                raise ValueError("Обсяг вибірки має бути не менше 50!")
            
            self.__init__()
            result_text.delete(1.0, tk.END)

            # Генерація вибірки
            interval = (k, k + 10)
            np.random.seed(42)
            self.sample = np.random.randint(interval[0], interval[1] + 1, size=n)
            result_text.insert(tk.END, f"Вибірка: {self.sample}\n\n")

            # Варіаційний ряд
            variation_series = np.sort(self.sample)
            result_text.insert(tk.END, f"Варіаційний ряд: {variation_series}\n\n")

            # Частотна таблиця
            freq_dict = Counter(self.sample)
            self.values = sorted(freq_dict.keys())
            self.frequencies = [freq_dict[val] for val in self.values]
            self.rel_freqs = [f / n for f in self.frequencies]
            result_text.insert(tk.END, "Частотна таблиця:\n")
            result_text.insert(tk.END, "Значення | Частота | Відн. частота\n")
            result_text.insert(tk.END, "-" * 40 + "\n")
            for val, freq, rel_freq in zip(self.values, self.frequencies, self.rel_freqs):
                result_text.insert(tk.END, f"{val:^8} | {freq:^9} | {rel_freq:^13.3f}\n")

            # Числові характеристики дискретного розподілу
            mean = np.mean(self.sample)  # Середнє
            quantiles = self.get_quantiles(n, variation_series) 
            median = quantiles["quartiles"][1] if isinstance(quantiles, dict) and "quartiles" in quantiles else np.median(self.sample)  # Медіана
            mode = self.calculate_mode(freq_dict)  # Мода
            std = np.std(self.sample, ddof=1)  # Стандартне відхилення вибірки
            sigma = np.std(self.sample)  # Сигма (загальне стандартне відхилення)
            variance = np.var(self.sample, ddof=1)  # Варіанса 
            variation_coeff = (std / mean) * 100 if mean != 0 else 0  # Варіація 
            dispersion = np.var(self.sample, ddof=1) # Дисперсія
            range_val = np.max(self.sample) - np.min(self.sample)  # Розмах
            asymmetry = np.sum([(x - mean) ** 3 * freq_dict[x] for x in self.values]) / (n * std ** 3) if std != 0 else 0  # Асиметрія
            kurtosis = np.sum([(x - mean) ** 4 * freq_dict[x] for x in self.values]) / (n * std ** 4) - 3 if std != 0 else 0  # Ексцес

            # Виведення характеристик дискретного розподілу без груп
            result_text.insert(tk.END, "\nЧислові характеристики (дискретний розподіл):\n")
            characteristics = {
                "Середнє": mean,
                "Медіана": median,
                "Мода": mode if mode is not None else "N/A",
                "Розмах": range_val,
                "Варіанса": variance,
                "Стандартне відхилення": std,
                "Коефіцієнт варіації (%)": variation_coeff,
                "Сигма": sigma,
                "Дисперсія": dispersion,
                "Асиметрія": asymmetry,
                "Ексцес": kurtosis
            }
            for key, value in characteristics.items():
                if isinstance(value, (int, float)):
                    result_text.insert(tk.END, f"  {key}: {value:.2f}\n")
                else:
                    result_text.insert(tk.END, f"  {key}: {value}\n")

            result_text.insert(tk.END, self.format_quantiles(quantiles))

            # Інтервальний розподіл
            self.bins = np.arange(k, k + 11, 2)
            self.hist, self.bin_edges = np.histogram(self.sample, bins=self.bins)
            bin_mids = (self.bin_edges[:-1] + self.bin_edges[1:]) / 2
            self.rel_hist = self.hist / n

            result_text.insert(tk.END, "\nІнтервальна частотна таблиця:\n")
            result_text.insert(tk.END, "Інтервал      | Частота  | Відн. частота | Середина\n")
            result_text.insert(tk.END, "-" * 50 + "\n")
            for i in range(len(self.hist)):
                result_text.insert(tk.END, f"[{self.bin_edges[i]:>2}-{self.bin_edges[i+1]:>2})    | "
                                          f"{self.hist[i]:^9} | {self.rel_hist[i]:^13.3f} | {bin_mids[i]:^8.2f}\n")

            # Числові характеристики згрупованих даних
            grouped_mean = np.sum(bin_mids * self.hist) / n
            grouped_q2 = self.calculate_grouped_quartile(n, 0.5)
            grouped_mode = self.calculate_grouped_mode()
            grouped_variance = np.sum(self.hist * (bin_mids - grouped_mean) ** 2) / (n - 1) if n > 1 else 0  # Вибіркова дисперсія
            grouped_std = np.sqrt(grouped_variance)  # Стандартне відхилення вибірки
            grouped_sigma = np.sqrt(np.sum(self.hist * (bin_mids - grouped_mean) ** 2) / n)  # Загальне стандартне відхилення (сигма)
            grouped_variation_coeff = (grouped_std / grouped_mean) * 100 if grouped_mean != 0 and n > 1 else 0  # Вариація (коефіцієнт варіації)
            grouped_range = self.bin_edges[-1] - self.bin_edges[0]  # Розмах
            grouped_asymmetry = np.sum(self.hist * (bin_mids - grouped_mean) ** 3) / (n * grouped_std ** 3) if grouped_std != 0 else 0  # Асиметрія
            grouped_kurtosis = np.sum(self.hist * (bin_mids - grouped_mean) ** 4) / (n * grouped_std ** 4) - 3 if grouped_std != 0 else 0  # Ексцес
            grouped_dispersion = np.var(self.sample, ddof=1)
            # Виведення характеристик згрупованих даних без груп
            result_text.insert(tk.END, "\nЧислові характеристики (інтервальний розподілу):\n")
            grouped_characteristics = {
                "Середнє": grouped_mean,
                "Медіана": grouped_q2 if grouped_q2 is not None else "N/A",
                "Мода": grouped_mode if grouped_mode is not None else "N/A",
                "Розмах": grouped_range,
                "Варіанса": grouped_variance,
                "Стандартне відхилення": grouped_std,
                "Коефіцієнт варіації (%)": grouped_variation_coeff,
                "Сигма": grouped_sigma,
                "Дисперсія": grouped_dispersion,
                "Асиметрія": grouped_asymmetry,
                "Ексцес": grouped_kurtosis
            }
            for key, value in grouped_characteristics.items():
                if isinstance(value, (int, float)):
                    result_text.insert(tk.END, f"  {key}: {value:.2f}\n")
                else:
                    result_text.insert(tk.END, f"  {key}: {value}\n")

        except ValueError as e:
            messagebox.showerror("Помилка", str(e) if str(e) else "Введіть цілі числа для k і n!")
            self.__init__()

    def plot_frequency_polygon(self):
        """Малює полігон частот"""
        if self.values is None or self.frequencies is None:
            messagebox.showwarning("Попередження", "Спочатку виконайте розрахунки!")
            return
        plt.figure(figsize=(8, 6))
        plt.plot(self.values, self.frequencies, marker='o', linestyle='-', color='b')
        plt.title("Полігон частот", fontsize=12)
        plt.xlabel("Значення", fontsize=10)
        plt.ylabel("Частота", fontsize=10)
        plt.grid(True, linestyle='--', alpha=0.7)
        plt.show()

    def plot_empirical_discrete(self):
        """Малює емпіричну функцію розподілу для дискретних даних"""
        if self.values is None or self.frequencies is None:
            messagebox.showwarning("Попередження", "Спочатку виконайте розрахунки!")
            return
        cumulative_frequencies = self.calculate_cumulative_frequencies()
        if not cumulative_frequencies.size:
            messagebox.showwarning("Попередження", "Немає даних для побудови!")
            return
        
        plt.figure(figsize=(10, 6))
        plt.title("Емпірична функція розподілу (дискретні дані)", fontsize=12)
        plt.xlabel("x", fontsize=10)
        plt.ylabel("F*(x)", fontsize=10)
        plt.grid(True, linestyle='--', alpha=0.7)
        
        prev_x, prev_y = self.values[0] - 1, 0
        for x, y in zip(self.values, cumulative_frequencies):
            plt.plot([prev_x, x], [prev_y, prev_y], 'k-', linewidth=2)
            plt.plot([x, x], [prev_y, y], 'k--', linewidth=1)
            plt.scatter([x], [y], color='black', zorder=3)
            prev_x, prev_y = x, y
        plt.plot([prev_x, prev_x + 1], [prev_y, prev_y], 'k-', linewidth=2)
        plt.show()

    def plot_histogram(self):
        """Малює гістограму"""
        if self.sample is None or self.bins is None:
            messagebox.showwarning("Попередження", "Спочатку виконайте розрахунки!")
            return
        plt.figure(figsize=(8, 6))
        plt.hist(self.sample, bins=self.bins, edgecolor='black', color='skyblue')
        plt.title("Гістограма", fontsize=12)
        plt.xlabel("Інтервали", fontsize=10)
        plt.ylabel("Частота", fontsize=10)
        plt.grid(True, linestyle='--', alpha=0.7)
        plt.show()

    def plot_empirical_grouped(self):
        """Малює емпіричну функцію розподілу для згрупованих даних"""
        if self.bin_edges is None or self.rel_hist is None:
            messagebox.showwarning("Попередження", "Спочатку виконайте розрахунки!")
            return
        cumulative_rel_hist = np.cumsum(self.rel_hist)
        
        plt.figure(figsize=(10, 6))
        plt.title("Емпірична функція розподілу (інтервальні дані)", fontsize=12)
        plt.xlabel("x", fontsize=10)
        plt.ylabel("F*(x)", fontsize=10)
        plt.grid(True, linestyle='--', alpha=0.7)
        
        prev_x = self.bin_edges[0] - (self.bin_edges[1] - self.bin_edges[0])
        prev_y = 0
        for x, y in zip(self.bin_edges[:-1], cumulative_rel_hist):
            plt.plot([prev_x, x], [prev_y, prev_y], 'k-', linewidth=2)
            plt.plot([x, x], [prev_y, y], 'k--', linewidth=1)
            plt.scatter([x], [y], color='black', zorder=3)
            prev_x, prev_y = x, y
        plt.plot([prev_x, prev_x + (self.bin_edges[1] - self.bin_edges[0])], [prev_y, prev_y], 'k-', linewidth=2)
        plt.show()

def main():
    analyzer = StatisticalAnalyzer()
    
    root = tk.Tk()
    root.title("Статистичний аналіз")
    root.geometry("700x600")

    tk.Label(root, text="Номер варіанту (k):", font=("Arial", 10)).grid(row=0, column=0, padx=5, pady=5)
    entry_k = tk.Entry(root)
    entry_k.grid(row=0, column=1, padx=5, pady=5)
    entry_k.insert(0, "5")

    tk.Label(root, text="Обсяг вибірки (n):", font=("Arial", 10)).grid(row=1, column=0, padx=5, pady=5)
    entry_n = tk.Entry(root)
    entry_n.grid(row=1, column=1, padx=5, pady=5)
    entry_n.insert(0, "50")

    tk.Button(root, text="Розрахувати", 
              command=lambda: analyzer.analyze(entry_k.get(), entry_n.get(), result_text), 
              bg="lightgreen", font=("Arial", 10)).grid(row=2, column=0, columnspan=2, pady=10)

    tk.Button(root, text="Полігон частот", command=analyzer.plot_frequency_polygon, font=("Arial", 10)).grid(row=3, column=0, padx=5, pady=5)
    tk.Button(root, text="Емп. ф-ція (дискр.)", command=analyzer.plot_empirical_discrete, font=("Arial", 10)).grid(row=3, column=1, padx=5, pady=5)
    tk.Button(root, text="Гістограма", command=analyzer.plot_histogram, font=("Arial", 10)).grid(row=4, column=0, padx=5, pady=5)
    tk.Button(root, text="Емп. ф-ція (інтер.)", command=analyzer.plot_empirical_grouped, font=("Arial", 10)).grid(row=4, column=1, padx=5, pady=5)

    result_text = scrolledtext.ScrolledText(root, width=80, height=25, font=("Courier", 10))
    result_text.grid(row=5, column=0, columnspan=2, padx=5, pady=5)

    root.mainloop()

if __name__ == "__main__":
    main()