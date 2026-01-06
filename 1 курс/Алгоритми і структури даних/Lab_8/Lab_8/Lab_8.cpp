#include <iostream>
#include <vector>
#include <utility>
#include <string>

using namespace std;

class MarkovAlgorithm {
private:
    vector<pair<string, string>> rules;
public:
    void addRule(const string& left, const string& right) {
        rules.emplace_back(left, right);
    }

    string run(const string& input) {
        string output = input;
        bool replaced;

        do {
            replaced = false;
            for (const auto& rule : rules) {
                size_t pos = output.find(rule.first);
                if (pos != string::npos) {
                    output.replace(pos, rule.first.length(), rule.second);
                    replaced = true;
                    break;
                }
            }
        } while (replaced);

        return output;
    }
};

int main() {
    MarkovAlgorithm ma;
    ma.addRule("ab", "ba");
    
    string input = "abba";
    string output = ma.run(input);

    cout << "Input: " << input << endl;
    cout << "Output: " << output << endl;

    return 0;
}
