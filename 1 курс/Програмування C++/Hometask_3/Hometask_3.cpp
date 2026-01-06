//Варіант 2
#include <iostream>
#include <string>

using namespace std;

class Triangle {
private:
    double side;
    double height;

public:
    Triangle(double b, double h) : side(b), height(h) {}

    double calculateArea() {
        return 0.5 * side * height;
    }
};


class Author {
private:
    string name;
    int yearBirthday;

public:
    Author(const string& n, int b) : name(n), yearBirthday(b) {}

    void displayInfo() {
        cout << "Author: " << name << ", Birthday: " << yearBirthday << endl;
    }
};


class Vehicle {
public:
    void start() {
        cout << "Vehicle started" << endl;
    }
};


class Employee {
public:
    virtual double calculateSalary() = 0;  
};

class Employee_2 : public Employee {
public:
    double calculateSalary() override {
        return 32000;
    }
};


class Vector2D {
public:
    double x, y;

    Vector2D(double x, double y) : x(x), y(y) {}

    Vector2D operator+(const Vector2D& other) const {
        return Vector2D(x + other.x, y + other.y);
    }

    Vector2D operator-(const Vector2D& other) const {
        return Vector2D(x - other.x, y - other.y);
    }
};

int main() {

    Triangle triangle(6, 8);
    cout << "Triangle area: " << triangle.calculateArea() << endl;

    Author author("Ivan Franko", 1856);
    author.displayInfo();

    Vehicle vehicle;
    vehicle.start();

    Employee_2 employee;
    cout << "Salary: " << employee.calculateSalary() << endl;

    Vector2D v1(1, 3);
    Vector2D v2(5, 4);
    Vector2D sum = v1 + v2;
    Vector2D diff = v1 - v2;

    cout << "Sum: (" << sum.x << ", " << sum.y << ")" << endl;
    cout << "Difference: (" << diff.x << ", " << diff.y << ")" << endl;

    return 0;
}
