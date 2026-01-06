#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

class Product {
protected:
    string name;
    string manufacturer;

public:
    Product(string _name, string _manufacturer) : name(_name), manufacturer(_manufacturer) {}

    virtual double getPrice() const = 0;

    string getName() const {
        return name;
    }

    string getManufacturer() const {
        return manufacturer;
    }
};

class Food : public Product {
private:
    double weight;
    double pricePerUnit;

public:
    Food(string _name, string _manufacturer, double _weight, double _pricePerUnit)
        : Product(_name, _manufacturer), weight(_weight), pricePerUnit(_pricePerUnit) {}

    double getPrice() const override {
        return weight * pricePerUnit;
    }
};

class Toy : public Product {
private:
    double price;
    int ageRange;

public:
    Toy(string _name, string _manufacturer, double _price, int _ageRange)
        : Product(_name, _manufacturer), price(_price), ageRange(_ageRange) {}

    double getPrice() const override {
        return price + (price * 0.2); 
    }
};

bool compareByPrice(const Product* a, const Product* b) {
    return a->getPrice() > b->getPrice();
}

int main() {
    vector<Product*> products;

    ifstream foodFile("C:/Навчальна практика/Task_2/food.txt");
    ifstream toyFile("C:/Навчальна практика/Task_2/toys.txt");

    string name, manufacturer;
    double weight, pricePerUnit, price;
    int ageRange;

    while (foodFile >> name >> manufacturer >> weight >> pricePerUnit) {
        products.push_back(new Food(name, manufacturer, weight, pricePerUnit));
    }

    while (toyFile >> name >> manufacturer >> price >> ageRange) {
        products.push_back(new Toy(name, manufacturer, price, ageRange));
    }

    sort(products.begin(), products.end(), compareByPrice);

    ofstream outputFile("C:/Навчальна практика/Task_2/sorted.txt");
    for (const auto& product : products) {
        outputFile << product->getName() << " " << product->getManufacturer() << " " << product->getPrice() << endl;
    }
    outputFile.close();

    string targetManufacturer = "ToyCompany"; 
    double totalToyPrice = 0.0;
    for (const auto& product : products) {
        Toy* toy = dynamic_cast<Toy*>(product); 
        if (toy && toy->getManufacturer() == targetManufacturer) {
            totalToyPrice += toy->getPrice();
        }
    }
    cout << "Total price of toys from manufacturer " << targetManufacturer << ": " << totalToyPrice << endl;

    for (auto& product : products) {
        delete product;
    }

    return 0;
}
