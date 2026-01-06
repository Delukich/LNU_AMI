import socket

HOST = input("Введіть IP сервера: ")
PORT = 9090

try:
    print("[Клієнт] Створюю сокет...")
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    print("[Клієнт] Підключаюсь до сервера...")
    s.connect((HOST, PORT))
    print("[Клієнт] Підключення встановлено!\n")

    msg = s.recv(1024).decode()
    print("[Сервер]:", msg, end="")
    login = input()
    s.send(login.encode())
    print("[Клієнт] Логін відправлено!")

    msg = s.recv(1024).decode()
    print("[Сервер]:", msg, end="")
    password = input()
    s.send(password.encode())
    print("[Клієнт] Пароль відправлено!")

    print("[Клієнт] Очікую відповідь від сервера...")
    reply = s.recv(1024).decode()
    print("\n[Сервер]:", reply)

except Exception as e:
    print("[Помилка:]", e)

finally:
    print("[Клієнт] З’єднання закрито.")
    s.close()
