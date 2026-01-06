#include <iostream>
#include <string>
#include <algorithm>

using namespace std;

class Country {
protected:
    string name;
    int population;
    double area;

public:
    Country(const string& n, int pop, double a) : name(n), population(pop), area(a) {}

    virtual double getSortValue() const {
        return area;  
    }

    virtual double getSortPol() const  {
        return population;  
    }

    virtual const string& getName() const {
        return name;
    }

    virtual void display() const {
        cout << "Country: " << name << ", Population: " << population << ", Area: " << area << endl;
    }
};

class Republic : public Country {
private:
    string governmentType;

public:
    Republic(const string& n, int pop, double a, const string& govType) : Country(n, pop, a), governmentType(govType) {}

    double getSortValue() const override {
        return area;  
    }

    double getSortPol() const override {
        return population;  
    }

    const string& getName() const override {
        return name;
    }

    void display() const override {
        cout << "Republic: " << name << ", Population: " << population << ", Area: " << area << ", Government: " << governmentType << endl;
    }
};

class Monarchy : public Country {
private:
    string monarch;

public:
    Monarchy(const string& n, int pop, double a, const string& monarchName) : Country(n, pop, a), monarch(monarchName) {}

    double getSortValue() const override {
        return area;  
    }

    double getSortPol() const override {
        return population;  
    }

    const string& getName() const override {
        return name;
    }

    void display() const override {
        cout << "Monarchy: " << name << ", Population: " << population << ", Area: " << area << ", Monarch: " << monarch << endl;
    }
};

bool sortByArea(const Country* a, const Country* b) {
    return a->getSortValue() > b->getSortValue();
}

bool sortByPopulation(const Country* a, const Country* b) {
    return a->getSortPol() > b->getSortPol();
}

bool sortByName(const Country* a, const Country* b) {
    return a->getName() < b->getName();
}

int main() {
    const int arraySize = 3;
    Country* countries[arraySize];

    countries[0] = new Republic("Usa", 10000000, 500000, "Joe Biden");
    countries[1] = new Monarchy("British", 8000000, 300000, "King John");

    string name;
    int population;
    double area;

    cout << "Enter details for Country:" << endl;
    cout << "Name: ";
    cin >> name;
    cout << "Population: ";
    cin >> population;
    cout << "Area: ";
    cin >> area;

    countries[2] = new Country(name, population, area);

    cout << "Sort by (1) Area, (2) Population, or (3) Name: ";
    int choice;
    cin >> choice;

    switch (choice) {
    case 1:
        sort(countries, countries + arraySize, sortByArea);
        break;
    case 2:
        sort(countries, countries + arraySize, sortByPopulation);
        break;
    case 3:
        sort(countries, countries + arraySize, sortByName);
        break;
    default:
        cout << "Invalid choice. Sorting by default (Area descending)." << endl;
        sort(countries, countries + arraySize, sortByArea);
        break;
    }

    for (int i = 0; i < arraySize; ++i) {
        countries[i]->display();
    }

    for (int i = 0; i < arraySize; ++i) {
        delete countries[i];
    }

    return 0;
}
