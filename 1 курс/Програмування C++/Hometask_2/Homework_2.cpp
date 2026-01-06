//Завдання з минулої пари
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

class Book {
public:
private:
    string name;
    string author;
    int year;
public:
    Book(const string& name, const string& author, int year)
        : name(name), author(author), year(year) {}

    virtual ~Book() {}

    const string& getName() const {
        return name;
    }

    const string& getAuthor() const {
        return author;
    }

    int getYear() const {
        return year;
    }
};

void print(const Book& book) {
    cout << "Name: " << book.getName() << ", Author: " << book.getAuthor() << ", Year: " << book.getYear() << endl;
}

class Library {
private:
    vector<Book> books;


public:
    void addBook(const Book& book) {
        books.push_back(book);
        cout << "Book added: " << book.getName() << endl;
    }

    void removeBook(const string& name) {
        auto it = remove_if(books.begin(), books.end(),[name](const Book& book) { return book.getName() == name; });

        if (it != books.end()) {
            books.erase(it, books.end());
            cout << "Book removed: " << name << endl;
        }
        else {
            cout << "Book not found: " << name << endl;
        }
    }

    void searchByName(const string& name) {
        cout << "Books with name '" << name << "':" << endl;
        for (auto book : books) {
            if (book.getName() == name) {
                print(book);
            }
        }
    }

    void searchByAuthor(const string& author) {
        cout << "Books by author '" << author << "':" << endl;
        for (auto book : books) {
            if (book.getAuthor() == author) {
                print(book);
            }
        }
    }

    void printAllBooks() {
        cout << "All books in the library:" << endl;
        for (auto book : books) {
            print(book);
        }
    }
};

int main() {
    Library library;

    library.addBook(Book("First Book", "First Author", 1998));
    library.addBook(Book("Second Book", "Second Author", 1960));
    library.addBook(Book("Third Book", "Third Author", 2017));

    library.searchByName("Second Book");
    library.searchByAuthor("First Author");

    library.removeBook("Third Book");

    library.printAllBooks();

    return 0;
}
