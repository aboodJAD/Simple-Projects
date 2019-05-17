#include<iostream>
#include<map>
#include<algorithm>
#include<stdlib.h>
#include<time.h>
#include<iomanip>

#include"KnowledgeInference.h"
#include"BFS.h"
#include"GridGenerator.h"
using namespace std;

int const N = 10, M = 10, MAX_STEPS = 150,
GOLD = 100, SHOOT_ARROW = -100, DIE = -10000;

/*store the KB and infer new facts;*/
KnowledgeInference* inferencer;

/*to navigate through the grid with the shortest distance*/
BFS* bfs;

//generator
G_MAP* Grid;
/*Grid*/
string table[11][11];
bool vis[11][11];

int arrows, score, steps;
/*current postion*/
int r, c;
int dirX[4] = { 1,-1,0,0 };
int dirY[4] = { 0,0,1,-1 };

/*indicate the wumpus is killed*/
bool WumpusKilled = 0;

bool killedByWumpus = 0, killedByPit = 0;
/*
at every stage of the game this set store the cells player can visit
these cells either the player is sure that they have no pits or wumpus
or he has no knowledge about them
*/
set<pair<int, int > > can_visit;


bool isValidLoc(int r, int c) {
	return r > 0 && r <= N&&c > 0 && c <= M;
}

void output() {
	puts("---------------------------------");
	puts("Wumpus World output");
	puts("---------------------------------");
	for (int i = N; i > 0; i--) {
		printf("%5d", i - 1);
		for (int j = 1; j <= M; j++) {
			char pt;
			if (vis[i][j]) {
				if (i == r&&j == c) pt='A';
				else pt = 'X';
			}
			else pt='0';
			printf("%5c", pt);
		}
		for (int j = 1; j <= M; j++) {
			if (j == 1)printf("%10s", table[i][j].c_str());
			else printf("%5s", table[i][j].c_str());
		}
		puts("");
	}
	puts("");
	printf("Pos: %d %d\nScore: %d\nSteps: %d/%d\n\n", r, c, score, steps, MAX_STEPS);
}

void Die() {
	return;
	printf("DIE\nPos: %d %d\nScore: %d\nSteps: %d/%d\n\n",r,c,score,steps,MAX_STEPS);
/*		cout << "DIE" << endl
		<< "Pos: " << r << ' ' << c << endl
		<< "Score: " << score << endl
		<< "Steps: " << steps << "/" << MAX_STEPS << endl << endl;*/
}

string getSymbol(char a, int r, int c, bool neg) {
	string res;
	res += a;
	res += to_string(r);
	res += to_string(c);
	if (neg)res += "'";
	return res;
}

pair<int, int > getNxtPos(int r, int c) {
	/*
	if the number of steps is larger than 120 then return back to home
	as the probabiliy of dieing increased
	
	if (steps > 120)return{ 1,1 };
	*/
	vector<pair<int, pair<int, int> > > shortestToCells;
	for (auto y : can_visit) {
		vis[y.first][y.second] = 1;
		shortestToCells.push_back({ bfs->getDist(vis,r,c,y.first,y.second) ,y });
		vis[y.first][y.second] = 0;
	}
	sort(shortestToCells.begin(), shortestToCells.end());
	pair<int, int > nxt = { -1,-1 };

	vector<pair<int, int > > nigh;
	for (int k = 0; k < 4; k++) {
		if (isValidLoc(r + dirY[k], c + dirX[k]) && !vis[r + dirY[k]][c + dirX[k]]) {
			nigh.push_back({ r + dirY[k], c + dirX[k] });
		}
	}
	/*
	i don't think this is the right way to kill the wumpus
	but at least it's killed
	*/
	if (table[r][c].find('S') != string::npos && !WumpusKilled) {
		score += SHOOT_ARROW*nigh.size();
		arrows -= nigh.size();
		WumpusKilled = true;
	}
	/*
	find the nearest available cell that we are sure it has no pit or wumups in it
	*/
	for (auto cell : shortestToCells) {
		int d = cell.first;
		int tarR = cell.second.first, tarC = cell.second.second;
		if (inferencer->Ask(getSymbol('P', tarR, tarC, 0)) == 0 &&
			(inferencer->Ask(getSymbol('W', tarR, tarC, 0)) == 0 || WumpusKilled)) {
			nxt = { tarR,tarC };
			break;
		}
	}
	/*
	if all the cells in can_visit set are dangerous then make a random walk

	*/
	if (nxt.first == -1) {
		//if the score is positive then don't make the random move and return home
		if (can_visit.empty() || score > 0)return{ 1,1 };
		int idx = rand() % (int)can_visit.size();
		auto it = can_visit.begin();
		while (idx--) it++;
		nxt = *it;
	}
	/*
	if the distance to nxt cell is larger than 150 the return home
	*/
	vis[nxt.first][nxt.second] = 1;
	if (steps + bfs->getDist(vis, r, c, nxt.first, nxt.second) > MAX_STEPS) return{ 1,1 };
	vis[nxt.first][nxt.second] = 0;
	return nxt;
}

void Simulate() {
	while (steps < 150) {

		///tell new facts aquired from the new position
		if (table[r][c].find('B') != string::npos) {
			inferencer->TellFact(getSymbol('B', r, c, 0));
		}
		else {
			inferencer->TellFact(getSymbol('B', r, c, 1));
		}

		if (table[r][c].find('S') != string::npos) {
			inferencer->TellFact(getSymbol('S', r, c, 0));
		}
		else {
			inferencer->TellFact(getSymbol('S', r, c, 1));
		}
		/*
		if player enters a cell with wumpus or pit then he will die
		this can happen only when there is a rondom move
		*/
		if ((table[r][c].find('W') != string::npos && !WumpusKilled)) {
			//puts("DIE");
			output();
			score += DIE;
			killedByWumpus = 1;
			return;
		}
		if (table[r][c].find('P') != string::npos) {
			//puts("DIE");
			output();
			score += DIE;
			killedByPit = 1;
			return;
		}
		///add Gold ^^
		if (table[r][c].find('G') != string::npos) score += GOLD;

		inferencer->TellFact(getSymbol('P', r, c, 1));
		inferencer->TellFact(getSymbol('W', r, c, 1));

		//update the KB after the facts aquired from the new position	
		inferencer->UpdateFacts();

		//erase this cell from the set of cells we can_visit;
		can_visit.erase({ r,c });
		/*
		add nighbouring cells to can_visit set if the cell has no Wumpus and pit
		and is not visted
		*/
		for (int k = 0; k < 4; k++) {
			int new_r = r + dirY[k], new_c = c + dirX[k];
			if (isValidLoc(new_r, new_c) && !vis[new_r][new_c] &&
				inferencer->Ask(getSymbol('P', new_r, new_c, 0)) != 1 &&
				(inferencer->Ask(getSymbol('W', new_r, new_c, 0)) != 1 || WumpusKilled)) {
				can_visit.insert({ new_r,new_c });
			}
		}
		/*
		if there is a cell surly has a Wumpus or a Pit then erase it from can visit cell
		*/
		vector<pair<int, int > > er;
		for (auto del : can_visit) {
			if (inferencer->Ask(getSymbol('P', del.first, del.second, 0)) == 1 ||
				(inferencer->Ask(getSymbol('W', del.first, del.second, 0)) == 1 && !WumpusKilled)) {
				er.push_back(del);
			}
		}
		for (auto toDel : er) can_visit.erase(toDel);
		output();

		/*
		call for geNxtPos function
		*/
		pair<int, int > nxt_pos = getNxtPos(r, c);
		/*
		if the returned cell is (1,1) then this means the player should climp out the cave
		*/
		if (nxt_pos.first == 1 && nxt_pos.second == 1) {
			steps += bfs->getDist(vis, r, c, 1, 1);
			r = 1, c = 1;
			output();
			return;
		}
		vis[nxt_pos.first][nxt_pos.second] = true;
		/*
		add the distance to next cell to count of steps
		*/
		steps += bfs->getDist(vis, r, c, nxt_pos.first, nxt_pos.second);
		r = nxt_pos.first;
		c = nxt_pos.second;
	}
}


void BuildRules() {
	/*
	these rules will infer if there is a pit or wumpus in x,y
	Bx,y ^ ~Px+1,y ^ ~Px-1,y ^ ~Px,y+1 => Px,y-1
	Sx,y ^ ~Wx+1,y ^ ~Wx-1,y ^ ~Wx,y+1 => Wx,y-1
	.
	.
	*/
	for (int i = 1; i <= N; i++) {
		for (int j = 1; j <= M; j++) {
			for (int k = 0; k < 4; k++) {
				int new_i = i + dirY[k], new_j = j + dirX[k];
				string expB = getSymbol('B', new_i, new_j, 0);
				string expS = getSymbol('S', new_i, new_j, 0);
				if (!isValidLoc(new_i, new_j))continue;
				for (int d = 0; d < 4; d++) {
					int ad_i = new_i + dirY[d], ad_j = new_j + dirX[d];
					if (!isValidLoc(ad_i, ad_j) || ad_i == i&&ad_j == j)continue;
					string Pit = " & ";
					Pit += getSymbol('P', ad_i, ad_j, 1);
					expB += Pit;
					string Wum = " & ";
					Wum += getSymbol('W', ad_i, ad_j, 1);
					expS += Wum;
				}
				expB += " => ";
				expB += getSymbol('P', i, j, 0);
				inferencer->TellRule(expB);

				expS += " => ";
				expS += getSymbol('W', i, j, 0);
				inferencer->TellRule(expS);
			}
		}
	}
	/*
	if a cell is not breezy(~Bx,y) then there are no pits(~Px',y') in all nighbouring cells;
	if a cell is not Stench(~Sx,y) then there is no Wumpus(~Wx',y') in all nighbouring cells;
	~Bx,y => ~Px+1,y ^ ~Px,y+1 ^ ~Px-1,y ^ ~Px,y-1
	*/
	for (int i = 1; i <= N; i++) {
		for (int j = 1; j <= N; j++) {
			string expB = getSymbol('B', i, j, 1),
				expS = getSymbol('S', i, j, 1);
			expB += " => ";
			expS += " => ";
			for (int k = 0; k < 4; k++) {
				int new_i = i + dirX[k], new_j = j + dirY[k];
				if (!isValidLoc(new_i, new_j))continue;
				string P = getSymbol('P', new_i, new_j, 1);
				string W = getSymbol('W', new_i, new_j, 1);

				string ad = expB;
				expB += P;
				inferencer->TellRule(expB);
				expB = ad;

				ad = expS;
				expS += W;
				inferencer->TellRule(expS);
				expS = ad;
			}
		}
	}
}
void start() {
	memset(vis, 0, sizeof vis);
	arrows = 10, score = 0, steps = 0;
	r = 1, c = 1;
	killedByPit = 0;
	killedByWumpus = 0;
	WumpusKilled = 0;
	can_visit.clear();
	delete inferencer;
	delete bfs;
	vis[1][1] = 1;
	inferencer = new KnowledgeInference();
	bfs = new BFS();//
	BuildRules();
	Simulate();
}

int main() {
	freopen("output.txt", "w", stdout);
	freopen("in.txt", "r", stdin);
	srand(time(0));
	Grid = new G_MAP();
	/*Statistics*/
	int KilledByWumpusCnt = 0, KilledByPitsCnt = 0,
		NegativeScoreCnt = 0, PositiveScoreCnt = 0, MaxPositiveScore = 0, RUNS = 1;
	double MaxRunTime = 0;
	for (int test = 0; test < RUNS; test++) {
		for (int i = 1; i <= N; i++)for (int j = 1; j <= N; j++)table[i][j] = "";
		double start_timer = clock();


/*		Grid->NEW_MAP(table);

		for (int i = 1, k = N; i < k; i++, k--) {
			for (int j = 1; j <= M; j++) {
				swap(table[i][j], table[k][j]);
			}
		}*/
		for (int i = N; i > 0; i--) {
			for (int j = 1; j <= M; j++) {
				cin >> table[i][j];
			}
		}
		start();
		KilledByPitsCnt += killedByPit;
		KilledByWumpusCnt += killedByWumpus;
		NegativeScoreCnt += score < 0;
		PositiveScoreCnt += score >= 0;
		MaxPositiveScore = max(MaxPositiveScore, score);
		MaxRunTime = max(MaxRunTime, (1.0*clock()-start_timer) / CLOCKS_PER_SEC);
	}
	printf("Number of kills caused by Pits : %d\n", KilledByPitsCnt);
	printf("Number of kills caused by Wumpus : %d\n", KilledByWumpusCnt);
	printf("Number of time get Negative Score : %d\n", NegativeScoreCnt);
	printf("Number of time get Postive Score : %d\n", PositiveScoreCnt);
	printf("Max Postive Score : %d\n", MaxPositiveScore);
	printf("Max Run Time : %.6f", MaxRunTime);

	delete inferencer;
	delete Grid;
	delete bfs;
	return 0;
}


