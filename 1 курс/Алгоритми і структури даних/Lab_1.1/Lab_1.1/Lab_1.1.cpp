#include <iostream>
#include <vector>

using namespace std;

void displayVector(const vector<int>& vec) {
    for (const auto& num : vec) {
        cout << num << " ";
    }
    cout << endl;
}

vector<int> strandSort(vector<int> input) {
    vector<int> sorted;  
    while (!input.empty()) {
        vector<int> strand;
        strand.push_back(input[0]);
        input.erase(input.begin());

        for (size_t i = 0; i < input.size(); ++i) {
            if (input[i] > strand.back()) {
                strand.push_back(input[i]);
                input.erase(input.begin() + i);
                --i;  
            }
        }

        if (sorted.empty()) {
            sorted = strand;
        }
        else {
            vector<int> merged;
            auto itSorted = sorted.begin();
            auto itStrand = strand.begin();

            while (itSorted != sorted.end() && itStrand != strand.end()) {
                if (*itSorted < *itStrand) {
                    merged.push_back(*itSorted);
                    ++itSorted;
                }
                else {
                    merged.push_back(*itStrand);
                    ++itStrand;
                }
            }

            merged.insert(merged.end(), itSorted, sorted.end());
            merged.insert(merged.end(), itStrand, strand.end());
            sorted = merged;
        }

        cout << "Current State: ";
        displayVector(sorted);
        cout << endl;
    }

    return sorted;
}

int main() {
    vector<int> input = { 8, 7, 3, 0, 5, 12, 3, 2, 1 };

    cout << "Initial Array: ";
    displayVector(input);
    cout << endl;

    vector<int> result = strandSort(input);

    cout << "Sorted Array: ";
    displayVector(result);

    return 0;
}
