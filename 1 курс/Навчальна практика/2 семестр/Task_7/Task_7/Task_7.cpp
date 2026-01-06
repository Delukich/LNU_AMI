#include <iostream>
#include <fstream>
#include <string>

using namespace std;

template<typename T>
class FileHandler {
private:
    string filename;
    fstream file;

public:
    FileHandler() {}

    void setFileName(const string& name) {
        filename = name;
    }

    void renameFile(const string& newName) {
        if (rename(filename.c_str(), newName.c_str()) != 0) {
            cout << "Error renaming file" << endl;
        }
        else {
            filename = newName;
        }
    }

    bool openFile(ios_base::openmode mode) {
        file.open(filename, mode);
        return file.is_open();
    }

    void writeToFile(const T& item) {
        if (file.is_open()) {
            file << item << endl;
        }
        else {
            cout << "File is not open" << endl;
        }
    }

    bool readFromFile(T& item) {
        if (file >> item) {
            return true;
        }
        else {
            return false;
        }
    }

    void closeFile() {
        if (file.is_open()) {
            file.close();
        }
    }
};

int main() {
   
    FileHandler<int> intFile;
    intFile.setFileName("C:/Навчальна практика/Task_7/intFile.txt");
    intFile.renameFile("C:/Навчальна практика/Task_7/newIntFile.txt");
    intFile.openFile(ios::out);
    intFile.writeToFile(13);
    intFile.closeFile();

    int readValue;
    intFile.openFile(ios::in);
    if (intFile.readFromFile(readValue)) {
        cout << "Read value from integer file: " << readValue << endl;
    }
    else {
        cout << "Failed to read from integer file" << endl;
    }
    intFile.closeFile();

    FileHandler<string> charFile;
    charFile.setFileName("C:/Навчальна практика/Task_7/stringFile.txt");
    charFile.renameFile("C:/Навчальна практика/Task_7/newStringFile.txt");
    charFile.openFile(ios::out);
    charFile.writeToFile("Hello");
    charFile.closeFile();

    string readChar;
    charFile.openFile(ios::in);
    if (charFile.readFromFile(readChar)) {
        cout << "Read character from char file: " << readChar << endl;
    }
    else {
        cout << "Failed to read from char file" << endl;
    }
    charFile.closeFile();

    return 0;
}






