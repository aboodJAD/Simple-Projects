#include "BFS.h"
#include<iostream>
void BFS::SetToDefult() {
	for (int i = 0; i<11; ++i)
		for (int j = 0; j<11; ++j)
			cost[i][j] = oo, vis[i][j] = false, par[i][j].first = -1, par[i][j].second = -1;
	while (!q.empty())q.pop();
}
bool BFS::vaild(int x, int y) {
	return (x >= 1 && x<11 && y >= 1 && y<11);
}
void BFS::walk(bool grid[][11], int x, int y) {
	cost[x][y] = 0;
	vis[x][y] = true;
	par[x][y].first = par[x][y].second = -1;
	q.push(make_pair(x, y));
	while (!q.empty()) {
		pair<int, int> u = q.front();
		q.pop();
		for (int i = 0; i<4; ++i) {
			int nx = u.first + dx[i];
			int ny = u.second + dy[i];
			if (vaild(nx, ny) == true && grid[nx][ny] == 1 && !vis[nx][ny]) {
				cost[nx][ny] = cost[u.first][u.second] + 1;
				par[nx][ny] = u;
				vis[nx][ny] = true;
				q.push(make_pair(nx, ny));
			}
		}
	}
}
vector<pair<int, pair<int, int> > > BFS::getDistToCells(bool grid[][11], set<pair<int, int> > se, int x, int y) {
	vector<pair<int, pair<int, int> > > vi;
	vi.clear();
	SetToDefult();
	walk(grid, x, y);
	set<pair<int, int> >::iterator it;
	for (it = se.begin(); it != se.end(); ++it) {
		pair<int, int> uu = *it;
		int xx = uu.first;
		int yy = uu.second;
		vi.push_back(make_pair(cost[xx][yy], make_pair(xx, yy)));
	}
	return vi;
}

vector<pair<int, int> > BFS::path(bool grid[][11], int sx, int sy, int dx, int dy) {
	SetToDefult();
	walk(grid, sx, sy);
	vector<pair<int, int> > vi;
	while (dx != -1) {
		vi.push_back(make_pair(dx, dy));
		int rr = dx, ww = dy;
		dx = par[rr][ww].first;
		dy = par[rr][ww].second;
	}
	reverse(vi.begin(), vi.end());
	return vi;
}


int BFS::getDist(bool grid[][11], int sx, int sy, int dx, int dy) {
	SetToDefult();
	walk(grid, sx, sy);
	return cost[dx][dy];
}