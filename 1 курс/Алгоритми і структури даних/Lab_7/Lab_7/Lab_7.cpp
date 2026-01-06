#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

class Queue {
private:
	vector<pair<int, int>> elements;

public:
	Queue() {}

	void enqueue(int val, int priority) {
		elements.push_back(make_pair(val, priority));
		sort(elements.begin(), elements.end(), [](const pair<int, int>& a, const pair<int, int>& b) {
			return a.second < b.second;
			});
	}

	void dequeue() {
		if (!elements.empty()) {
			elements.erase(elements.begin());
		}
		else {
			cout << "Error: queue is empty." << endl;
		}
	}

	int front() {
		if (!isEmpty()) {
			return elements.front().first;
		}
		else {
			cout << "Error: queue is empty." << endl;
			return -1;
		}
	}

	bool isEmpty() {
		return elements.empty();
	}

	void print() {
		cout << "Queue elements: ";
		for (const auto& a : elements) {
			cout << "(" << a.first << ", " << a.second << ") ";
		}
		cout << endl;
	}

	int size() {
		return elements.size();
	}
};

int main() {

	Queue priority;

	priority.enqueue(3, 3);
	priority.enqueue(5, 1);
	priority.enqueue(7, 2);
	priority.enqueue(1, 1);
	priority.enqueue(2, 4);
	

	int iteration = -1;
	while (!priority.isEmpty()) {
		cout << endl << "Iteration: " << ++iteration << endl;
		priority.print();
		cout << "Front element: " << priority.front() << endl;
		priority.dequeue();
	}

	cout << "Queue is empty" << endl;

	return 0;
}

