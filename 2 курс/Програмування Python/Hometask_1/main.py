#Задання №4. Лук'янчук Денис
tasks = []
name = input("Enter your name: ")
print(f"Hello, {name}!")

while True:
    task = input("Enter a task(type 'exit' to exit): ")
    if task == "exit":
        break
    tasks.append(task)

print("Your tasks:")
index = 1
for task in tasks:
    print(f"{index}) {task}")
    index += 1

while tasks:
    try:
        user_input = int(input("What task did you complete? (type '0' to exit): "))
        if 1 <= user_input <= len(tasks):
            completed_task = user_input - 1
            del tasks[completed_task]
            print("Task removed")
        elif user_input == 0:
            break
        else:
            print("Invalid task number!")

        print("Your updated tasks:")
        index = 1
        for task in tasks:
            print(f"{index}) {task}")
            index += 1

    except ValueError:
        print("Enter the correct task number!")

with open("tasks.txt", "w") as file:
    file.write(f"My name: {name}\n" + "My tasks:\n")
    index = 1
    for task in tasks:
        file.write(f"{index}) {task}\n")
        index += 1
