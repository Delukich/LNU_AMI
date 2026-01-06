#include <iostream>
#include <bitset>

using namespace std;

const int MAX_SIZE = 10; 

class BitSet {
private:
    bitset<MAX_SIZE> bits; 

public:
    void insert(int num) {
        if (num >= 0 && num < MAX_SIZE)
            bits.set(num); 
        else
            cout << "Invalid number for insertion." << endl;
    }

    void remove(int num) {
        if (num >= 0 && num < MAX_SIZE)
            bits.reset(num); 
        else
            cout << "Invalid number for removal." << endl;
    }

    bool contains(int num) {
        if (num >= 0 && num < MAX_SIZE)
            return bits.test(num); 
        else {
            cout << "Invalid number for containment check." << endl;
            return false;
        }
    }

    BitSet unionWith(const BitSet& other) const {
        BitSet result;
        result.bits = bits | other.bits; 
        return result;
    }

    BitSet intersectWith(const BitSet& other) const {
        BitSet result;
        result.bits = bits & other.bits; 
        return result;
    }

    BitSet differenceWith(const BitSet& other) const {
        BitSet result;
        result.bits = bits & ~(other.bits); 
        return result;
    }

    void display() {
        cout << "Bitset: " << bits << endl;
    }
};

int main() {
    BitSet set1, set2;
    set1.insert(1);
    set1.insert(2);
    set1.insert(3);

    set2.insert(3);
    set2.insert(4);
    set2.insert(5);

    set1.display();
    set2.display();

    BitSet unionSet = set1.unionWith(set2);
    cout << "Union of sets:" << endl;
    unionSet.display();

    BitSet intersectionSet = set1.intersectWith(set2);
    cout << "Intersection of sets:" << endl;
    intersectionSet.display();

    BitSet differenceSet = set1.differenceWith(set2);
    cout << "Difference of sets:" << endl;
    differenceSet.display();


    cout << "Does the set contain element 6? " << (set1.contains(6) ? "Yes" : "No") << endl;
    cout << "Does the set contain element 1? " << (set1.contains(1) ? "Yes" : "No") << endl;

    cout << "Bitset after remove: " << endl;
    set1.remove(3);

    set1.display();

    return 0;
}
