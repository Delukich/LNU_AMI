#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

class Point {
public:
    int x, y;
    Point(int x, int y) : x(x), y(y) {}
};

class Rectangle {
public:
    Point* topLeft, * bottomRight;
    Rectangle(Point* tl, Point* br) : topLeft(tl), bottomRight(br) {}
    int area() const {
        return (bottomRight->x - topLeft->x) * (bottomRight->y - topLeft->y);
    }
};

bool compareRectangles(const Rectangle* r1, const Rectangle* r2) {
    return r1->area() < r2->area();
}

int main() {
    try {
        int n;
        cout << "Enter the number of rectangles: ";
        cin >> n;

        vector<Rectangle*> rectangles;
        cout << "Enter the coordinates:" << endl;
        for (int i = 0; i < n; ++i) {
            int x1, y1, x2, y2, x3, y3, x4, y4;
            cin >> x1 >> y1 >> x2 >> y2 >> x3 >> y3 >> x4 >> y4;

            Point* topLeft = new Point(min({ x1, x2, x3, x4 }), min({ y1, y2, y3, y4 }));
            Point* bottomRight = new Point(max({ x1, x2, x3, x4 }), max({ y1, y2, y3, y4 }));

            rectangles.push_back(new Rectangle(topLeft, bottomRight));
        }

        sort(rectangles.begin(), rectangles.end(), compareRectangles);

        cout << "Sorted list:" << endl;
        ofstream outputFile("C:/Навчальна практика/Task_11/sqranes.txt");
        for (const auto& rect : rectangles) {
            cout << "Rectangle: (" << rect->topLeft->x << "," << rect->topLeft->y << "), ("
                << rect->bottomRight->x << "," << rect->bottomRight->y << "), Area: " << rect->area() << endl;
            outputFile << "Rectangle: (" << rect->topLeft->x << "," << rect->topLeft->y << "), ("
                << rect->bottomRight->x << "," << rect->bottomRight->y << "), Area: " << rect->area() << endl;
            delete rect; 
        }
        outputFile.close();

    }
    catch (const exception& e) {
        cout << "Exception: " << e.what() << endl;
    }

    return 0;
}
