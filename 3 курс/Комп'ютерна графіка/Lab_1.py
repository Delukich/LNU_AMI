import pygame
import math
import numpy as np

WIDTH, HEIGHT = 800, 600
FPS = 60
SIDE = 100
LINE_Y = 100

center = np.array([WIDTH // 2, HEIGHT - 100], dtype=float)
theta = 0.0
angular_speed = -0.05 
move_speed = 2
moving = True

pygame.init()
screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("Matrix Transform Square")
clock = pygame.time.Clock()

half = SIDE / 2
local_vertices = np.array([
    [-half, -half, 1],
    [ half, -half, 1],
    [ half,  half, 1],
    [-half,  half, 1]
])

def transform_vertices(vertices, theta, tx, ty):
    R = np.array([
        [ math.cos(theta), -math.sin(theta), 0],
        [ math.sin(theta),  math.cos(theta), 0],
        [0,0,1]
    ])
    T = np.array([
        [1,0,tx],
        [0,1,ty],
        [0,0,1]
    ])
    M = T @ R
    return (M @ vertices.T).T[:, :2]  

running = True
while running:
    for e in pygame.event.get():
        if e.type == pygame.QUIT:
            running = False

    if moving:
        center[1] -= move_speed
        theta += angular_speed

        verts = transform_vertices(local_vertices, theta, *center)
        if min(verts[:,1]) <= LINE_Y:
            moving = False  

    verts = transform_vertices(local_vertices, theta, *center)

    screen.fill((240,240,240))
    pygame.draw.line(screen, (0,128,255), (0,LINE_Y), (WIDTH,LINE_Y),4)
    pygame.draw.line(screen, (128,128,128), (center[0],center[1]), (center[0],LINE_Y),1)
    pygame.draw.polygon(screen, (255,100,100), verts, 0)
    pygame.draw.polygon(screen, (200,0,0), verts, 2)
    pygame.draw.circle(screen, (0,200,0), center.astype(int), 5)

    pygame.display.flip()
    clock.tick(FPS)

pygame.quit()
