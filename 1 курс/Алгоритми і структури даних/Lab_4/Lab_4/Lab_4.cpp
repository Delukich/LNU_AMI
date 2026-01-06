#include <iostream>
#include <vector>
#include <list>

using namespace std;

class HashTable {
private:
    vector<list<pair<int, string>>> table;
    int size;

public:
    HashTable(int size) : size(size) {
        table.resize(size);
    }

    int hashFunction(int key) {
        return key % size; 
    }

    bool contains(int key) {
        int index = hashFunction(key);
        for (const auto& entry : table[index]) {
            if (entry.first == key) {
                return true;
            }
        }
        return false;
    }

    void insert(int key, const string& value) {
      
        int index = hashFunction(key);
        table[index].push_back(make_pair(key, value));
    }

    string search(int key) {
        int index = hashFunction(key);
        for (const auto& entry : table[index]) {
            if (entry.first == key) {
                return entry.second;
            }
        }
        return "Key not found";
    }

    void remove(int key) {
        int index = hashFunction(key);
        for (auto it = table[index].begin(); it != table[index].end(); ++it) {
            if (it->first == key) {
                table[index].erase(it);
                return;
            }
        }
    }
};

int main() {
    int size;
    cout << "Enter size of the hash table: ";
    cin >> size;

    HashTable ht(size); 

    int key;
    string value;

    cout << "Enter key and value pairs (enter -1 to stop):\n";
    while (true) {
        cout << "Enter key (-1 to stop): ";
        cin >> key;
        if (key == -1) break;

        if (ht.contains(key)) {
            cout << "Key already exists. Please enter a different key." << endl;
            continue;
        }

        cout << "Enter value: ";
        cin >> value;

        ht.insert(key, value);
    }

    cout << "Enter key to search: ";
    cin >> key;
    cout << "Value corresponding to key " << key << ": " << ht.search(key) << endl;

    cout << "Enter key to remove: ";
    cin >> key;
    ht.remove(key);

    cout << "After removing key " << key << ", value corresponding to key " << key << ": " << ht.search(key) << endl;

    return 0;
}
