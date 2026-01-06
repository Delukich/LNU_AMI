import socket
import threading
import sys

HOST = '0.0.0.0'
PORT = 9090


def handle_client(conn, addr):
    print(f"[Підключено] {addr}")

    try:
        print(f"[Сервер] Надсилаю запит логіна → {addr}")
        conn.send("Введіть логін: ".encode())

        login = conn.recv(1024).decode().strip()
        print(f"[Сервер] Логін отримано від {addr}: {login}")

        print(f"[Сервер] Надсилаю запит пароля → {addr}")
        conn.send("Введіть пароль: ".encode())

        password = conn.recv(1024).decode().strip()
        print(f"[Сервер] Пароль отримано від {addr}")

        print(f"[Сервер] Перевірка даних для {addr}...")

        if login == "Denys" and password == "123456":
            conn.send("Успішний вхід!\n".encode())
            print(f"[Сервер] Авторизація успішна → {addr}")
        else:
            conn.send("Помилка: неправильний логін або пароль.\n".encode())
            print(f"[Сервер] Авторизація НЕуспішна → {addr}")

    except Exception as e:
        print(f"[Помилка] {addr}: {e}")

    conn.close()
    print(f"[Відключено] {addr}")


def run_sequential():
    print("=== Послідовний режим ===")
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"Сервер запущено на {HOST}:{PORT}")

    while True:
        conn, addr = server.accept()
        handle_client(conn, addr)


def run_parallel():
    print("=== Паралельний режим ===")
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    print(f"Сервер запущено на {HOST}:{PORT}")

    while True:
        conn, addr = server.accept()
        thread = threading.Thread(target=handle_client, args=(conn, addr))
        thread.start()
        print(f"[Потік створено] Активних потоків: {threading.active_count() - 1}")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Вкажіть режим: seq або par")
        sys.exit(0)

    mode = sys.argv[1]

    if mode == "seq":
        run_sequential()
    elif mode == "par":
        run_parallel()
    else:
        print("Невідомий режим! Використовуйте: seq або par")
