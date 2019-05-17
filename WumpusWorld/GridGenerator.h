#include<iostream>
#include<time.h>
#include<vector>
#include<algorithm>
#include<string>

using namespace std;

class G_MAP {
	int dx[4] = { 0, 0, 1, -1 };
	int dy[4] = { 1, -1, 0, 0 };
public:
	bool valid(int x, int y) {
		return (x >= 1 && x<11 && y >= 1 && y<11);
	}
	bool can(int x, int y) {
		if (x == 10 && y == 1)
			return false;
		return true;
	}

	void NEW_MAP(string grid[][11]) {
		srand(time(NULL));
		int x = rand() % 100;
		vector <pair<int, int> > p_pos, g_pos;
		int cnt = rand() % 20 + 5;
		while (p_pos.size() < cnt) {
			int x = rand() % 10 + 1;
			int y = rand() % 10 + 1;
			if (can(x, y))
				p_pos.push_back(make_pair(x, y));
		}
		int gcnt = rand() % 11 + 10;
		while (g_pos.size() < gcnt) {
			int x = rand() % 10 + 1;
			int y = rand() % 10 + 1;
			if (can(x, y))
				g_pos.push_back(make_pair(x, y));
		}
		pair <int, int> wombus;
		while (1) {
			int x = rand() % 10 + 1;
			int y = rand() % 10 + 1;
			wombus = make_pair(x, y);
			if (x != 10 || y != 1)
				break;
		}
		sort(p_pos.begin(), p_pos.end());
		p_pos.erase(unique(p_pos.begin(), p_pos.end()), p_pos.end());
		sort(g_pos.begin(), g_pos.end());
		g_pos.erase(unique(g_pos.begin(), g_pos.end()), g_pos.end());
		for (int i = 0; i < p_pos.size(); i++) {
			grid[p_pos[i].first][p_pos[i].second] += 'P';
		}
		for (int i = 0; i < p_pos.size(); i++) {
			for (int j = 0; j < 4; j++)
				if (valid(p_pos[i].first + dx[j], p_pos[i].second + dy[j]))
					if (grid[p_pos[i].first + dx[j]][p_pos[i].second + dy[j]].size() == 0) {
						grid[p_pos[i].first + dx[j]][p_pos[i].second + dy[j]] += 'B';
					}
					else if (grid[p_pos[i].first + dx[j]][p_pos[i].second + dy[j]][grid[p_pos[i].first + dx[j]][p_pos[i].second + dy[j]].size() - 1] != 'B')
						grid[p_pos[i].first + dx[j]][p_pos[i].second + dy[j]] += 'B';
		}
		for (int i = 0; i < g_pos.size(); i++) {
			grid[g_pos[i].first][g_pos[i].second] += 'G';
		}
		grid[wombus.first][wombus.second] += 'W';
		for (int i = 0; i < 4; i++) {
			if (valid(wombus.first + dx[i], wombus.second + dy[i]))
				grid[wombus.first + dx[i]][wombus.second + dy[i]] += 'S';
		}
		for (int i = 1; i <= 10; i++) {
			for (int j = 1; j <= 10; j++) {
				if (grid[i][j].size() == 0)
					grid[i][j] = 'Z';
			}
		}
	}
};

