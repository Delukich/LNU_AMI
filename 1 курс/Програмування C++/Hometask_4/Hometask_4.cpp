//Варіант 2
#include <iostream>
#include <vector>

using namespace std;

//1
class Department {
private:
    string name;
    int foundationYear;

public:
    Department(const string& name, int foundationYear) : name(name), foundationYear(foundationYear) {}

    void displayInfo() const {
        cout << "Department: " << name << ", Foundation Year: " << foundationYear << endl;
    }
};

class University {
private:
    string name;
    vector<Department> departments;

public:
    University(const string& name) : name(name) {}

    void addDepartment(const Department& department) {
        departments.push_back(department);
    }

    void displayInfo() const {
        cout << "University: " << name << endl;
        cout << "Departments:" << endl;
        for (const auto& department : departments) {
            department.displayInfo();
        }
    }
};

//2
class Shape {
public:
    virtual double getArea() const = 0;

    virtual ~Shape() {}
};

class Circle : public Shape {
private:
    double radius;

public:
    Circle(double radius) : radius(radius) {}

    double getArea() const override {
        return 3.14 * radius * radius;
    }
};

class Square : public Shape {
private:
    double side;

public:
    Square(double side) : side(side) {}

    double getArea() const override {
        return side * side;
    }
};

//3
class Matrix {
private:
    vector<vector<int>> mar;

public:
    Matrix(int rows, int cols) : mar(rows, vector<int>(cols, 0)) {}

    vector<int>& operator[](int index) {
        return mar[index];
    }

    void print() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                cout << mar[i][j] << " ";
            }
            cout << endl;
        }
    }
};

//5
class Shape2 {
public:
    virtual void draw() const = 0;
};

class Circle2 : public Shape2 {
public:
    void draw() const override {
        cout << "Drawing Circle" << endl;
    }
};

class Rectangle : public Shape2 {
public:
    void draw() const override {
        cout << "Drawing Rectangle" << endl;
    }
};

void drawShape(const Shape2* shape) {
    if (const Circle2* circle = dynamic_cast<const Circle2*>(shape)) {
        circle->draw();
    }
    else if (const Rectangle* rectangle = dynamic_cast<const Rectangle*>(shape)) {
        rectangle->draw();
    }
    else {
        cout << "Unknown shape" << endl;
    }
}

int main() {
    //1
    Department department1("Computer Science", 1990);
    Department department2("Mathematics", 1985);
    Department department3("Physics", 1978);

    University university("Example University");

    university.addDepartment(department1);
    university.addDepartment(department2);
    university.addDepartment(department3);

    university.displayInfo();

    //2
    Circle circle(5);

    Square square(4);

    cout << "Circle Area: " << circle.getArea() << endl;
    cout << "Square Area: " << square.getArea() << endl;

    //3
    Matrix matrix(3, 3);

    matrix[0][0] = 0;
    matrix[0][1] = 2;
    matrix[0][2] = 3;
    matrix[1][0] = 9;
    matrix[1][1] = 5;
    matrix[1][2] = 6;
    matrix[2][0] = 2;
    matrix[2][1] = 8;
    matrix[2][2] = 9;

    matrix.print();

    //5
    Circle2 circle2;
    Rectangle rectangle;

    drawShape(&circle2);
    drawShape(&rectangle);

    return 0;
}
