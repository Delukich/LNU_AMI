//Варіант 14
#define CATCH_CONFIG_MAIN 
#include <catch.hpp>
#include <iostream>
#include <vector>
#include <string>

using namespace std;

class Employee {
private:
    string name;
    string position;

public:
   
    Employee(const string& name, const string& position) : name(name), position(position) {}

    string getName() const {
        return name;
    }

    string getPosition() const {
        return position;
    }
};

class Company {
private:
    vector<Employee> employees;

public:
   
    void addEmployee(const Employee& emp) {
        employees.push_back(emp);
    }

    void removeEmployee(int index) {
        if (index >= 0 && index < employees.size()) {
            employees.erase(employees.begin() + index);
        }
    }

    void removeEmployeeByName(const string& name) {
        for (auto it = employees.begin(); it != employees.end(); ++it) {
            if (it->getName() == name) {
                employees.erase(it);
                break;
            }
        }
    }

    int getEmployeeCount() const {
        return employees.size();
    }

    void displayEmployees() const {
        for (const auto& emp : employees) {
            cout << "Name: " << emp.getName() << ", Position: " << emp.getPosition() << endl;
        }
    }
};

TEST_CASE("Testing Company class functionality") {
    Company company;

    Employee emp1("John Doe", "Manager");
    Employee emp2("Jane Smith", "Engineer");

    SECTION("Adding employees") {
        company.addEmployee(emp1);
        company.addEmployee(emp2);

        REQUIRE(company.getEmployeeCount() == 2);
    }

    SECTION("Removing an employee by index") {
        company.addEmployee(emp1);
        company.addEmployee(emp2);

        company.removeEmployee(1);
        REQUIRE(company.getEmployeeCount() == 1);
    }

    SECTION("Removing an employee by name") {
        company.addEmployee(emp1);
        company.addEmployee(emp2);

        company.removeEmployeeByName("John Doe");
        REQUIRE(company.getEmployeeCount() == 1);
    }
}

int main(int argc, char* argv[]) {
    Employee emp1("John Doe", "Manager");
    Employee emp2("Jane Smith", "Engineer");
    Employee emp3("Mike Johnson", "Analyst");

    Company company;

    company.addEmployee(emp1);
    company.addEmployee(emp2);
    company.addEmployee(emp3);

    cout << "Employees in the company:" << endl;
    company.displayEmployees();

    company.removeEmployee(1);

    cout << "\nAfter removing an employee:" << endl;
    company.displayEmployees();

    company.removeEmployeeByName("John Doe");

    cout << "\nAfter removing another employee:" << endl;
    company.displayEmployees();

    return Catch::Session().run(argc, argv);
}
