#pragma once
#include<string>
#include<queue>
#include<set>
#include<vector>

using namespace std;
class BFS {
	const int oo = 1e6;
	bool vis[11][11];
	int cost[11][11];
	pair<int, int> par[11][11];
	queue<pair<int, int> > q;
	int dx[4] = { 0 , 0 , 1 , -1 };
	int dy[4] = { 1 , -1 , 0 , 0 };
public:
	void SetToDefult();
	bool vaild(int x, int y);
	void walk(bool grid[][11], int, int);
	vector<pair<int, pair<int, int> > > getDistToCells(bool grid[][11], set<pair<int, int> >, int, int);
	vector<pair<int, int> > path(bool grid[][11], int sx, int sy, int dx, int dy);
	int getDist(bool grid[][11], int, int, int, int);
};
