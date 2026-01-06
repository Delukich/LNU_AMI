import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D  
from matplotlib.widgets import TextBox, Button, Slider

plt.rcParams['font.family'] = 'DejaVu Sans'
plt.rcParams['font.size'] = 10

class BilinearSurfaceGUI:

    def __init__(self):
        
        self.default_points = {
            'P00': np.array([1.0, 1.0, 2.0]),
            'P10': np.array([4.0, 1.0, 3.0]),
            'P01': np.array([1.0, 4.0, 1.0]),
            'P11': np.array([4.0, 4.0, 4.0])
        }
        self.default_n_points = 20
        
        self.points = {k: v.copy() for k, v in self.default_points.items()}
        self.n_points = self.default_n_points
        
        self._textboxes = {}
        
        self.create_gui()

    def Q(self, u, v):
        P00 = self.points['P00']
        P10 = self.points['P10']
        P01 = self.points['P01']
        P11 = self.points['P11']
        
        return ((1 - u) * (1 - v) * P00 +
                u * (1 - v) * P10 +
                (1 - u) * v * P01 +
                u * v * P11)

    def generate_surface(self):
        u_vec = np.linspace(0, 1, self.n_points)
        v_vec = np.linspace(0, 1, self.n_points)
        U, V = np.meshgrid(u_vec, v_vec)
        
        U = U[..., np.newaxis]
        V = V[..., np.newaxis]
        
        Q_matrix = self.Q(U, V)
        
        X = Q_matrix[..., 0]
        Y = Q_matrix[..., 1]
        Z = Q_matrix[..., 2]
        
        return X, Y, Z

    def create_gui(self):
        self.fig = plt.figure(figsize=(16, 10))
        self.fig.suptitle('Білінійна інтерполююча поверхня Q(u,v) — Інтерактивний режим',
                          fontsize=14, fontweight='bold')
        
        control_rect = [0.02, 0.05, 0.18, 0.9]
        control_ax = self.fig.add_axes(control_rect)
        control_ax.axis('off')
        
        y_top = 0.94      
        dy_field = 0.05   
        dy_group_gap = 0.02 
        
        point_names = ['P00', 'P10', 'P01', 'P11']
        coords = ['X', 'Y', 'Z']
        
        current_y = y_top
        
        for p_name in point_names:
            self._textboxes[p_name] = {}
            current_point = self.points[p_name]
            
            for j, coord_name in enumerate(coords):
                rect = [0.04, current_y - j * dy_field, 0.12, 0.045]
                label = f'{p_name} {coord_name}'
                initial_val = str(current_point[j])
                
                tb = self.create_textbox(rect, label, initial_val)
                self._textboxes[p_name][coord_name] = tb
            
            current_y -= (len(coords) * dy_field + dy_group_gap)
            
        
        slider_rect = [0.04, current_y, 0.12, 0.03]
        slider_ax = self.fig.add_axes(slider_rect)
        self.slider_resolution = Slider(slider_ax, 'Сітка', 10, 40, 
                                        valinit=self.n_points, valstep=2)
        self.slider_resolution.on_changed(self.on_slider_change)
        
        text_y = current_y - 0.04
        self.resolution_text_ax = self.fig.text(0.10, text_y, f'Сітка: {self.n_points}x{self.n_points}',
                                              ha='center', fontsize=9)
        
        button_y = text_y - 0.07
        button_rect = [0.04, button_y, 0.12, 0.05]
        button_ax = self.fig.add_axes(button_rect)
        self.button_update = Button(button_ax, 'Оновити графіки', color='lightgreen', hovercolor='green')
        self.button_update.on_clicked(self.update_plot)
        
        reset_y = button_y - 0.07
        reset_rect = [0.04, reset_y, 0.12, 0.05]
        button_reset_ax = self.fig.add_axes(reset_rect)
        self.button_reset = Button(button_reset_ax, 'Скинути', color='lightyellow', hovercolor='yellow')
        self.button_reset.on_clicked(self.reset_values)
        
        self.ax_3d = self.fig.add_subplot(2, 2, 1, projection='3d', position=[0.22, 0.52, 0.35, 0.42])
        self.ax_xy = self.fig.add_subplot(2, 2, 2, position=[0.60, 0.52, 0.35, 0.42])
        self.ax_xz = self.fig.add_subplot(2, 2, 3, position=[0.22, 0.06, 0.35, 0.42])
        self.ax_yz = self.fig.add_subplot(2, 2, 4, position=[0.60, 0.06, 0.35, 0.42])
        
        self.update_plot(None)

    def create_textbox(self, rect, label, initial):
        ax = self.fig.add_axes(rect)
        textbox = TextBox(ax, label, initial=initial)
        return textbox

    def on_slider_change(self, val):
        self.n_points = int(val)
        self.resolution_text_ax.set_text(f'Сітка: {self.n_points}x{self.n_points}')
        self.fig.canvas.draw_idle() 

    def update_plot(self, event):
        try:
            for p_name, tbs in self._textboxes.items():
                coords = [
                    float(tbs['X'].text),
                    float(tbs['Y'].text),
                    float(tbs['Z'].text)
                ]
                self.points[p_name] = np.array(coords)
                
            X, Y, Z = self.generate_surface()
            
            self.ax_3d.clear()
            self.ax_xy.clear()
            self.ax_xz.clear()
            self.ax_yz.clear()
            
            corners = np.array([self.points[p] for p in ['P00', 'P10', 'P01', 'P11']])
            
            self.plot_3d(X, Y, Z, corners)
            self.plot_projection_xy(X, Y, Z, corners)
            self.plot_projection_xz(X, Y, Z, corners)
            self.plot_projection_yz(X, Y, Z, corners)
            
            self.fig.canvas.draw_idle()
            
        except ValueError:
            # Мовчки ігноруємо помилку, якщо введено не число
            pass

    def reset_values(self, event):
        self.points = {k: v.copy() for k, v in self.default_points.items()}
        
        for p_name, tbs in self._textboxes.items():
            point = self.points[p_name]
            tbs['X'].set_val(str(point[0]))
            tbs['Y'].set_val(str(point[1]))
            tbs['Z'].set_val(str(point[2]))
            
        self.slider_resolution.set_val(self.default_n_points)
        self.update_plot(None)

    def plot_3d(self, X, Y, Z, corners):
        self.ax_3d.set_title('3D Візуалізація', fontweight='bold')
        
        step = max(1, self.n_points // 10) 
        
        for i in range(0, self.n_points, step):
            self.ax_3d.plot(X[i, :], Y[i, :], Z[i, :], color='r', linewidth=1.0, alpha=0.7)
        for j in range(0, self.n_points, step):
            self.ax_3d.plot(X[:, j], Y[:, j], Z[:, j], color='b', linewidth=1.0, alpha=0.7)
            
        self.ax_3d.scatter(corners[:, 0], corners[:, 1], corners[:, 2],
                           c='yellow', s=80, marker='o', edgecolors='black', 
                           linewidth=1.2, zorder=5)
        
        labels = ['P00', 'P10', 'P01', 'P11']
        for i, label in enumerate(labels):
            self.ax_3d.text(corners[i, 0], corners[i, 1], corners[i, 2], f'  {label}', 
                            fontsize=9, fontweight='bold', zorder=6)
            
        self.ax_3d.set_xlabel('X')
        self.ax_3d.set_ylabel('Y')
        self.ax_3d.set_zlabel('Z')
        self.ax_3d.grid(True, linestyle='--', alpha=0.4)

    def _plot_projection(self, ax, data_1, data_2, corners_1, corners_2, title, xlabel, ylabel):
        ax.set_title(title, fontweight='bold')
        ax.set_xlabel(xlabel)
        ax.set_ylabel(ylabel)
        ax.grid(True, linestyle='--', alpha=0.4)
        ax.set_aspect('equal', adjustable='box')
        
        step = max(1, self.n_points // 10)
        
        for i in range(0, self.n_points, step):
            ax.plot(data_1[i, :], data_2[i, :], 'r-', linewidth=1.0, alpha=0.7)
        for j in range(0, self.n_points, step):
            ax.plot(data_1[:, j], data_2[:, j], 'b-', linewidth=1.0, alpha=0.7)
            
        ax.scatter(corners_1, corners_2, c='yellow', s=60, marker='o', 
                   edgecolors='black', linewidth=1, zorder=5)

    def plot_projection_xy(self, X, Y, Z, corners):
        self._plot_projection(self.ax_xy, X, Y, corners[:, 0], corners[:, 1],
                              'Проєкція XY (z=0)', 'X', 'Y')

    def plot_projection_xz(self, X, Y, Z, corners):
        self._plot_projection(self.ax_xz, X, Z, corners[:, 0], corners[:, 2],
                              'Проєкція XZ (y=0)', 'X', 'Z')

    def plot_projection_yz(self, X, Y, Z, corners):
        self._plot_projection(self.ax_yz, Y, Z, corners[:, 1], corners[:, 2],
                              'Проєкція YZ (x=0)', 'Y', 'Z')

    def show(self):
        plt.show()

def main():
    app = BilinearSurfaceGUI()
    app.show()

if __name__ == "__main__":
    main()