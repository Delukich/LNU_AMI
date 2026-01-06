#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

class Person {
private:
    string lastName;
    int age;
    char gender;

public:

    Person(string last, int a, char g) : lastName(last), age(a), gender(g) {}


    string getLastName() const { return lastName; }
    int getAge() const { return age; }
    char getGender() const { return gender; }


    friend ostream& operator<<(ostream& out, const Person& person);
    friend istream& operator>>(istream& in, Person& person);
};

ostream& operator<<(ostream& out, const Person& person) {
    out << "Last Name: " << person.lastName << ", Age: " << person.age << ", Gender: " << person.gender;
    return out;
}

istream& operator>>(istream& in, Person& person) {
    cout << "Enter your last name: ";
    in >> person.lastName;
    cout << "Enter your age: ";
    in >> person.age;
    cout << "Enter gender: ";
    in >> person.gender;
    return in;
}


bool sortByLastName(const Person& a, const Person& b) {
    return a.getLastName() < b.getLastName();
}


bool sortByAge(const Person& a, const Person& b) {
    return a.getAge() < b.getAge();
}


bool sortByGender(const Person& a, const Person& b) {
    return a.getGender() < b.getGender();
}


Person findPersonByLastName(const vector<Person>& people, const string& lastName) {
    auto it = find_if(people.begin(), people.end(), [lastName](const Person& person) {
        return person.getLastName() == lastName;
        });

    if (it != people.end()) {
        return *it;
    }
    else {
        return Person("", 0, ' ');
    }
}

int main() {
    vector<Person> people;
    ifstream inputFile("C:/Навчальна практика/Project_10.2/people.txt");


    if (inputFile.is_open()) {
        Person person("", 0, ' ');
        while (inputFile >> person) {
            people.push_back(person);
        }
        inputFile.close();
    }
    else {
        cout << "Eror" << endl;
        return 1;
    }

    int choice;
    string searchLastName;

    do {
        cout << "\nMenu:\n";
        cout << "1. Display all records\n";
        cout << "2. Sorting by last name\n";
        cout << "3. Sort by age\n";
        cout << "4. Sort by article\n";
        cout << "5. Search by last name\n";
        cout << "0. Exit\n";
        cout << "Your choice: ";
        cin >> choice;

        switch (choice) {
        case 1:
            cout << "All records:\n";
            for (const auto& person : people) {
                cout << person << endl;
            }
            break;

        case 2:
            sort(people.begin(), people.end(), sortByLastName);
            cout << "Sorted by last name\n";
            break;

        case 3:
            sort(people.begin(), people.end(), sortByAge);
            cout << "Sorted by age\n";
            break;

        case 4:
            sort(people.begin(), people.end(), sortByGender);
            cout << "Sorted by article\n";
            break;

        case 5:
            cout << "Enter a last name to search: ";
            cin >> searchLastName;
            {
                Person foundPerson = findPersonByLastName(people, searchLastName);
                if (foundPerson.getLastName() != "") {
                    cout << "Found: " << foundPerson << endl;
                }
                else {
                    cout << "Record with last name '" << searchLastName << "' not found\n";
                }
            }
            break;

        case 0:
            cout << "Exit the program\n";
            break;

        default:
            cout << "Wrong choice. Try again.\n";
        }

    } while (choice != 0);

    return 0;
}
