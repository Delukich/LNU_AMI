#include <iostream>
#include <fstream>
#include <string>
#include <map>

using namespace std;

struct Book {
    string title;
    string author;
    int year;
    string publisher;
};

void printBook(const Book& book) {
    cout << "Title: " << book.title << endl;
    cout << "Author: " << book.author << endl;
    cout << "Year: " << book.year << endl;
    cout << "Publisher: " << book.publisher << endl;
    cout << "-------------------------" << endl;
}

int main() {
    multimap<string, Book> booksDatabase;

    ifstream inputFile("C:/Навчальна практика/Task_10/books.txt");
    if (inputFile.is_open()) {
        string title, author, publisher;
        int year;
        cout << "Books from the file:" << endl;
        while (inputFile >> title >> author >> year >> publisher) {
            Book book;
            book.title = title;
            book.author = author;
            book.year = year;
            book.publisher = publisher;
            booksDatabase.insert(make_pair(author, book));

            printBook(book);
        }
        inputFile.close();
    }
    else {
        cout << "Unable to open file!" << endl;
    }

    Book newBook;
    cout << "Enter title: ";
    cin >> newBook.title;
    cout << "Enter author: ";
    cin >> newBook.author;
    cout << "Enter year: ";
    cin >> newBook.year;
    cout << "Enter publisher: ";
    cin >> newBook.publisher;
    booksDatabase.insert(make_pair(newBook.author, newBook));

    cout << "Books after adding a new book:" << endl;
    for (const auto& entry : booksDatabase) {
        printBook(entry.second);
    }

    string searchAuthor;
    cout << "Enter author's name to search books: ";
    cin >> searchAuthor;
    auto range = booksDatabase.equal_range(searchAuthor);
    cout << "Books by " << searchAuthor << ":" << endl;
    for (auto it = range.first; it != range.second; ++it) {
        printBook(it->second);
    }

    string deleteAuthor;
    cout << "Enter author's name to delete books: ";
    cin >> deleteAuthor;
    booksDatabase.erase(deleteAuthor);

    cout << "Books after deleting books by " << deleteAuthor << ":" << endl;
    for (const auto& entry : booksDatabase) {
        printBook(entry.second);
    }

    string searchTitle;
    cout << "Enter title of the book to search author: ";
    cin >> searchTitle;
    for (const auto& entry : booksDatabase) {
        if (entry.second.title == searchTitle) {
            cout << "Author of " << searchTitle << " is " << entry.second.author << endl;
            break;
        }
    }

    string searchPublisher;
    cout << "Enter publisher's name to count books and save them to file: ";
    cin >> searchPublisher;
    int bookCount = 0;
    cout << "Books published by " << searchPublisher << ":" << endl;
    for (const auto& entry : booksDatabase) {
        if (entry.second.publisher == searchPublisher) {
            printBook(entry.second);
            bookCount++;
        }
    }
    cout << "Total books for " << searchPublisher << ": " << bookCount << endl;

    return 0;
}
