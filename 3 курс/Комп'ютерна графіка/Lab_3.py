import numpy as np
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from matplotlib.figure import Figure
import tkinter as tk
from tkinter import ttk, scrolledtext
import math


class PseudoRaster:
    
    def __init__(self, size=32):
        self.size = size
        self.buffer = [0] * (size * size)
    
    def set_pixel(self, x, y):
        if 0 <= x < self.size and 0 <= y < self.size:
            index = y * self.size + x
            self.buffer[index] = 1
    
    def get_pixel(self, x, y):
        if 0 <= x < self.size and 0 <= y < self.size:
            index = y * self.size + x
            return self.buffer[index]
        return 0
    
    def clear(self):
        self.buffer = [0] * (self.size * self.size)
    
    def get_filled_pixels(self):
        filled = []
        for i, val in enumerate(self.buffer):
            if val == 1:
                row = i // self.size
                col = i % self.size
                filled.append((row, col))
        return filled
    
    def to_matrix(self):
        return np.array(self.buffer).reshape(self.size, self.size)


def bresenham_circle(raster, cx, cy, radius):
    x = 0
    y = radius
    d = 3 - 2 * radius
    
    def draw_circle_points(xc, yc, x, y):
        points = [
            (xc + x, yc + y), (xc - x, yc + y),
            (xc + x, yc - y), (xc - x, yc - y),
            (xc + y, yc + x), (xc - y, yc + x),
            (xc + y, yc - x), (xc - y, yc - x)
        ]
        for px, py in points:
            raster.set_pixel(px, py)
    
    draw_circle_points(cx, cy, x, y)
    
    while y >= x:
        x += 1
        if d > 0:
            y -= 1
            d = d + 4 * (x - y) + 10
        else:
            d = d + 4 * x + 6
        draw_circle_points(cx, cy, x, y)


def draw_line_bresenham(raster, x1, y1, x2, y2):
    dx = abs(x2 - x1)
    dy = abs(y2 - y1)
    sx = 1 if x1 < x2 else -1
    sy = 1 if y1 < y2 else -1
    err = dx - dy
    
    x, y = x1, y1
    
    while True:
        raster.set_pixel(x, y)
        
        if x == x2 and y == y2:
            break
        
        e2 = 2 * err
        if e2 > -dy:
            err -= dy
            x += sx
        if e2 < dx:
            err += dx
            y += sy


def polygon_approximation_circle(raster, cx, cy, radius, num_sides):
    if num_sides < 3:
        num_sides = 3
    
    vertices = []
    for i in range(num_sides):
        angle = 2 * math.pi * i / num_sides
        x = cx + int(radius * math.cos(angle))
        y = cy + int(radius * math.sin(angle))
        vertices.append((x, y))
    
    for i in range(num_sides):
        x1, y1 = vertices[i]
        x2, y2 = vertices[(i + 1) % num_sides]
        draw_line_bresenham(raster, x1, y1, x2, y2)


class CircleDrawingGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("Алгоритми креслення кола - Псевдорастр 32x32")
        self.root.geometry("1400x800")
        
        self.SIZE = 32
        
        main_frame = ttk.Frame(root, padding="10")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        
        control_frame = ttk.LabelFrame(main_frame, text="Параметри", padding="10")
        control_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S), padx=5, pady=5)
        
        ttk.Label(control_frame, text="Центр X:").grid(row=0, column=0, sticky=tk.W, pady=5)
        self.center_x_var = tk.IntVar(value=16)
        self.center_x_scale = ttk.Scale(control_frame, from_=0, to=31, 
                                        variable=self.center_x_var, 
                                        orient=tk.HORIZONTAL, length=200,
                                        command=self.update_plot)
        self.center_x_scale.grid(row=0, column=1, pady=5)
        self.center_x_label = ttk.Label(control_frame, text="16")
        self.center_x_label.grid(row=0, column=2, padx=5)
        
        ttk.Label(control_frame, text="Центр Y:").grid(row=1, column=0, sticky=tk.W, pady=5)
        self.center_y_var = tk.IntVar(value=16)
        self.center_y_scale = ttk.Scale(control_frame, from_=0, to=31, 
                                        variable=self.center_y_var, 
                                        orient=tk.HORIZONTAL, length=200,
                                        command=self.update_plot)
        self.center_y_scale.grid(row=1, column=1, pady=5)
        self.center_y_label = ttk.Label(control_frame, text="16")
        self.center_y_label.grid(row=1, column=2, padx=5)
        
        ttk.Label(control_frame, text="Радіус:").grid(row=2, column=0, sticky=tk.W, pady=5)
        self.radius_var = tk.IntVar(value=12)
        self.radius_scale = ttk.Scale(control_frame, from_=1, to=15, 
                                      variable=self.radius_var, 
                                      orient=tk.HORIZONTAL, length=200,
                                      command=self.update_plot)
        self.radius_scale.grid(row=2, column=1, pady=5)
        self.radius_label = ttk.Label(control_frame, text="12")
        self.radius_label.grid(row=2, column=2, padx=5)
        
        ttk.Label(control_frame, text="Сторони:").grid(row=3, column=0, sticky=tk.W, pady=5)
        self.sides_var = tk.IntVar(value=8)
        self.sides_scale = ttk.Scale(control_frame, from_=3, to=24, 
                                     variable=self.sides_var, 
                                     orient=tk.HORIZONTAL, length=200,
                                     command=self.update_plot)
        self.sides_scale.grid(row=3, column=1, pady=5)
        self.sides_label = ttk.Label(control_frame, text="8")
        self.sides_label.grid(row=3, column=2, padx=5)
        
        # Чекбокси для відображення
        ttk.Separator(control_frame, orient=tk.HORIZONTAL).grid(row=4, column=0, 
                                                                columnspan=3, 
                                                                sticky=(tk.W, tk.E), 
                                                                pady=10)
        
        ttk.Label(control_frame, text="Відображення:").grid(row=5, column=0, 
                                                            columnspan=3, sticky=tk.W)
        
        self.show_bresenham = tk.BooleanVar(value=True)
        ttk.Checkbutton(control_frame, text="Алгоритм Брезенхема", 
                       variable=self.show_bresenham,
                       command=self.update_plot).grid(row=6, column=0, 
                                                      columnspan=3, sticky=tk.W, pady=2)
        
        self.show_polygon = tk.BooleanVar(value=True)
        ttk.Checkbutton(control_frame, text="Многокутник", 
                       variable=self.show_polygon,
                       command=self.update_plot).grid(row=7, column=0, 
                                                      columnspan=3, sticky=tk.W, pady=2)
        
        ttk.Button(control_frame, text="Оновити", 
                  command=self.update_plot).grid(row=8, column=0, 
                                                columnspan=3, pady=10)
        
        stats_frame = ttk.LabelFrame(control_frame, text="Статистика", padding="10")
        stats_frame.grid(row=9, column=0, columnspan=3, sticky=(tk.W, tk.E), pady=10)
        
        self.stats_text = tk.Text(stats_frame, height=4, width=30)
        self.stats_text.pack()
        
        pixel_frame = ttk.LabelFrame(main_frame, text="Список пікселів", padding="10")
        pixel_frame.grid(row=1, column=0, sticky=(tk.W, tk.E, tk.N, tk.S), padx=5, pady=5)
        
        self.pixel_text = scrolledtext.ScrolledText(pixel_frame, height=15, width=40)
        self.pixel_text.pack(fill=tk.BOTH, expand=True)
        
        plot_frame = ttk.Frame(main_frame)
        plot_frame.grid(row=0, column=1, rowspan=2, sticky=(tk.W, tk.E, tk.N, tk.S), padx=5, pady=5)
        
        self.fig = Figure(figsize=(10, 10), dpi=80)
        self.ax = self.fig.add_subplot(111)
        
        self.canvas = FigureCanvasTkAgg(self.fig, master=plot_frame)
        self.canvas.get_tk_widget().pack(fill=tk.BOTH, expand=True)
        
        self.update_plot()
    
    def update_plot(self, event=None):
        cx = int(self.center_x_var.get())
        cy = int(self.center_y_var.get())
        radius = int(self.radius_var.get())
        sides = int(self.sides_var.get())
        
        self.center_x_label.config(text=str(cx))
        self.center_y_label.config(text=str(cy))
        self.radius_label.config(text=str(radius))
        self.sides_label.config(text=str(sides))
        
        raster_bresenham = PseudoRaster(self.SIZE)
        raster_polygon = PseudoRaster(self.SIZE)
        
        bresenham_circle(raster_bresenham, cx, cy, radius)
        polygon_approximation_circle(raster_polygon, cx, cy, radius, sides)
        
        bres_pixels = raster_bresenham.get_filled_pixels()
        poly_pixels = raster_polygon.get_filled_pixels()
        
        self.stats_text.delete(1.0, tk.END)
        stats = f"Брезенхем: {len(bres_pixels)} пікселів\n"
        stats += f"Многокутник: {len(poly_pixels)} пікселів\n"
        stats += f"Центр: ({cx}, {cy})\n"
        stats += f"Радіус: {radius}"
        self.stats_text.insert(1.0, stats)
        
        self.pixel_text.delete(1.0, tk.END)
        
        if self.show_bresenham.get():
            self.pixel_text.insert(tk.END, "=== АЛГОРИТМ БРЕЗЕНХЕМА ===\n")
            self.pixel_text.insert(tk.END, f"Загально: {len(bres_pixels)} пікселів\n\n")
            for i, (row, col) in enumerate(bres_pixels[:50]):
                self.pixel_text.insert(tk.END, f"[{i+1}] Рядок: {row:2d}, Колонка: {col:2d}\n")
            if len(bres_pixels) > 50:
                self.pixel_text.insert(tk.END, f"\n... та ще {len(bres_pixels) - 50} пікселів\n")
            self.pixel_text.insert(tk.END, "\n")
        
        if self.show_polygon.get():
            self.pixel_text.insert(tk.END, f"=== МНОГОКУТНИК ({sides} СТОРІН) ===\n")
            self.pixel_text.insert(tk.END, f"Загально: {len(poly_pixels)} пікселів\n\n")
            for i, (row, col) in enumerate(poly_pixels[:50]):
                self.pixel_text.insert(tk.END, f"[{i+1}] Рядок: {row:2d}, Колонка: {col:2d}\n")
            if len(poly_pixels) > 50:
                self.pixel_text.insert(tk.END, f"\n... та ще {len(poly_pixels) - 50} пікселів\n")
        
        self.ax.clear()
        
        overlay = np.zeros((self.SIZE, self.SIZE, 3))
        bresenham_matrix = raster_bresenham.to_matrix()
        polygon_matrix = raster_polygon.to_matrix()
        
        if self.show_bresenham.get() and self.show_polygon.get():
            overlay[:, :, 0] = bresenham_matrix * (1 - polygon_matrix)
            overlay[:, :, 2] = polygon_matrix * (1 - bresenham_matrix)
            overlay[:, :, 1] = bresenham_matrix * polygon_matrix
        elif self.show_bresenham.get():
            overlay[:, :, 0] = bresenham_matrix
        elif self.show_polygon.get():
            overlay[:, :, 2] = polygon_matrix
        
        self.ax.imshow(overlay, interpolation='nearest')
        self.ax.set_title(f'Коло: Центр=({cx},{cy}), Радіус={radius}\n' + 
                         f'🔴 Брезенхем | 🔵 Многокутник ({sides} сторін) | 🟢 Обидва',
                         fontsize=12, pad=10)
        self.ax.grid(True, alpha=0.3)
        self.ax.set_xlabel('Колонка')
        self.ax.set_ylabel('Рядок')
        self.ax.set_xticks(range(0, self.SIZE, 4))
        self.ax.set_yticks(range(0, self.SIZE, 4))
        
        self.canvas.draw()


def main():
    root = tk.Tk()
    app = CircleDrawingGUI(root)
    root.mainloop()


if __name__ == "__main__":
    main()