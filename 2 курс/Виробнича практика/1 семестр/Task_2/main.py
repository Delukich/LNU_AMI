from datetime import datetime
import calendar

year = int(input("Enter year: "))
month = int(input("Enter month: "))
day = int(input("Enter day: "))

if not (1 <= month <= 12):
    print("Error month!")
else:
    if not (1 <= day <= calendar.monthrange(year, month)[1]):
        print("Error day!")
    else:
        if year < 0:
            print("Error year!")
        else:
            try:
                dt = datetime(year, month, day)

                day_of_week = dt.isoweekday()

                days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
                print(days[day_of_week - 1])

            except ValueError as e:
                print("Error date!")