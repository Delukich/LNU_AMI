// Завдання #14
#include <iostream>
#include <string>

#define CATCH_CONFIG_MAIN
#include "catch.hpp"

using namespace std;

class Bank {
private:
    string name;
    string location;

public:
    Bank(const string& name, const string& location):name(name), location(location) {}

    bool Local() const {
        return location == "Ukraine";
    }

    string getName() const {
        return name;
    }

    string getLocation() const {
        return location;
    }

    void display() const {
        cout << "Bank name: " << name << endl;
        cout << "Location: " << location << endl;

    }
};

    TEST_CASE("Bank is local if located in Ukraine") {

        Bank bank("LocalBank", "Ukraine");

        REQUIRE(bank.Local() == true);

        CHECK(bank.getName() == "LocalBank");
        CHECK(bank.getLocation() == "Ukraine");
    }

    TEST_CASE("Bank is not local if located outside Ukraine") {

        Bank bank("OtherBank", "OtherCountry");

        REQUIRE(bank.Local() == false);

        CHECK(bank.getName() == "OtherBank");
        CHECK(bank.getLocation() == "OtherCountry");
    }

    int main() {

        string name;
        string location;

        cout << "Enter bank name: ";
        cin >> name;
        cout << "Enter location: ";
        cin >> location;
        cout << endl;

        Bank bank1(name, location);
        Bank bank2("Monobank", "Ukraine");
        Bank bank3("Bank of Poland", "Poland");
        Bank bank4("PrivateBank", "Ukraine");

        cout << "Bank 1 is local: " << (bank1.Local() ? "Yes" : "No") << endl;
        cout << "Bank 2 is local: " << (bank2.Local() ? "Yes" : "No") << endl;
        cout << "Bank 3 is local: " << (bank3.Local() ? "Yes" : "No") << endl;
        cout << "Bank 4 is local: " << (bank4.Local() ? "Yes" : "No") << endl;

        bank1.display();
        bank2.display();
        bank3.display();
        bank4.display();

        return 0;
    }