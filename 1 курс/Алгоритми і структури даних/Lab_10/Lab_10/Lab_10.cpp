#include <iostream>
#include <queue>
#include <climits>
#include <vector>

using namespace std;

class MyGraph {
    int** matrix;
    int num;

public:

    MyGraph(int num) : num(num) {
        matrix = new int* [num];
        for (int i = 1; i < num; i++) {
            matrix[i] = new int[num];
            for (int j = 1; j < num; j++) {
                matrix[i][j] = 0;
            }
        }
    }

    void add(int i, int j, int k) {
        matrix[i][j] = k;
        matrix[j][i] = k;
    }

    void printPath(vector<int>& parent, int j) {
        if (parent[j] == -1)
            return;

        printPath(parent, parent[j]);
        cout << j << " ";
    }

    void algorithm(int start, int end) {
        bool vis[100];
        int dis[100];
        vector<int> parent(num, -1);

        for (int i = 0; i < num; ++i) {
            vis[i] = false;
            dis[i] = INT_MAX;
        }

        vis[start] = true;
        dis[start] = 0;

        queue<int> que;
        que.push(start);

        while (!que.empty()) {
            int vetrex = que.front(); que.pop();

            for (int i = 0; i < num; ++i) {
                if (!vis[i] && matrix[vetrex][i] && matrix[vetrex][i] + dis[vetrex] < dis[i]) {
                    dis[i] = matrix[vetrex][i] + dis[vetrex];
                    que.push(i);
                    vis[i] = true;
                    parent[i] = vetrex;
                }
            }
        }

        cout << "Shortest distance from vertex " << start << " to vertex " << end << ": " << dis[end] << endl;
        cout << "Shortest path: ";
        printPath(parent, end);
        cout << endl;
    }

    void print() {
        cout << "Graph" << endl;
        for (int i = 1; i < num; i++) {
            cout << i << " : ";
            for (int j = 1; j < num; j++) {
                cout << matrix[i][j] << " ";
            }
            cout << endl;
        }
        cout << endl;
    }
};

int main()
{
    MyGraph g(5);

    g.add(1, 2, 3);
    g.add(1, 3, 3);
    g.add(1, 4, 1);
    g.add(3, 2, 2);
    g.add(4, 3, 1);
    

    g.print();

    int start, end;
    cout << "Enter the start: ";
    cin >> start;
    cout << "Enter the end: ";
    cin >> end;

    g.algorithm(start, end);

    return 0;
}
