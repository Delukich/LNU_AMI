#include <iostream>
#include <iomanip>
#include <vector>

using namespace std;

class Country {
private:
    string name;
    string capital;
    long area;
    long population;

public:
    Country(string name, string capital, long area, long population)
        : name(name), capital(capital), area(area), population(population) {}

    void print() const {
        cout << setw(15) << left << name
            << setw(15) << left << capital
            << setw(15) << left << area
            << setw(15) << left << population << endl;
    }
};



int main() {

    vector<Country> countries;

    countries.push_back(Country("Ukraine", "Kyiv", 603700, 43700000));
    countries.push_back(Country("USA", "Washington", 9834000, 332000000));
    countries.push_back(Country("United Kingdom", "London", 243600, 67300000));
    countries.push_back(Country("Germany", "Berlin", 357600, 83200000));


    cout << setw(15) << left << "Country"
        << setw(15) << left << "Capital"
        << setw(15) << left << "Area"
        << setw(15) << left << "Population" << endl;

    for (size_t i = 0; i < countries.size(); ++i) {
        countries[i].print();
    }

    return 0;
}

