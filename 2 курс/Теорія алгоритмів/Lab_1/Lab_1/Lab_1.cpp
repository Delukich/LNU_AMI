#include <iostream>      
#include <vector>                
#include <unordered_map> 
#include <cmath> 
#include <unordered_set> 

using namespace std;


// 1. алгоритм для віднімання послідовності чисел B від числа A
void subtraction(int& A, const vector<int>& B) {
    // Для кожного елементу в масиві B віднімаємо його від числа A
    for (int i = 0; i < B.size(); i++) {
        A -= B[i]; // Віднімаємо B[i] від A
    }
    // Складність цього алгоритму: O(n), де n — кількість елементів у масиві B
}


// 2. алгоритм для перевірки, чи число є простим
bool prime(int A) {
    // якщо число дорівнює 1 або менше то воно не є простим
    if (A <= 1) return false;

    // перевіряємо чи ділиться число на будь-яке число від 2 до кореня з A
    for (int i = 2; i <= sqrt(A); i++) {
        if (A % i == 0) return false; // якщо є дільник то число не просте
    }
    return true; // якщо дільників не знайдено то число просте
    // складність цього алгоритму: O(√A) де A — перевіряєме число. Потрібно перевірити числа до кореня з A
}


// 3. алгоритм для знаходження максимального числа однакових елементів у масиві
int max(const vector<int>& arr) {
    unordered_map<int, int> freq; // контейнер для підрахунку частоти елементів

    // проходимо по масиву і рахуємо кількість кожного елементу
    for (int num : arr) {
        freq[num]++; // збільшуємо лічильник для кожного елемента
    }

    int maxFreq = 0; // ініціалізуємо змінну для збереження максимальної кількості повторів

    // перевірка на максимальну частоту
    for (const auto& pair : freq) {
        if (pair.second > maxFreq) {
            maxFreq = pair.second; // оновлюємо максимальну кількість
        }
    }
    return maxFreq; // повертаємо максимальну кількість однакових елементів
    // складність цього алгоритму: O(n), де n — кількість елементів у масиві
    // для кожного елементу ми оновлюємо картку частот, і потім робимо один прохід по картці
}

// 4. алгоритм для знаходження кількості різних елементів у масиві
int count(const vector<int>& arr) {
    unordered_set<int> uniqueElements; // множина для збереження унікальних елементів

    // додаємо кожен елемент до множини 
    for (int num : arr) {
        uniqueElements.insert(num);
    }

    return uniqueElements.size(); // повертаємо кількість різних елементів
    // складність цього алгоритму: O(n), де n — кількість елементів у масиві
    // Оскільки кожен елемент додається до множини, що має середню складність O(1) для кожної операції вставки
}

int main() {
    // завдання №1
    int A = 15; 
    vector<int> B = { 1, 4, 3, 8, 11 }; 
    subtraction(A, B); 
    cout << "1. Subtraction result: " << A << endl; 

    // завдання №2
    int number = 27; 
    if (prime(number)) { 
        cout << "2. " << number << " is a prime number " << endl;
    }
    else {
        cout << "2. " << number << " is not a prime number " << endl;
    }

    // завдання №3
    vector<int> arr1 = { 1, 2, 2, 3, 3, 3, 4, 5 }; 
    cout << "3. Maximum frequency of identical elements: " << max(arr1) << endl;


    // завдання №4
    vector<int> arr2 = { 1, 2, 2, 3, 3, 4, 5 }; 
    cout << "4. Number of unique elements: " << count(arr2) << endl;

    return 0; 
}
