#pragma once

#include<string>
#include<sstream>
#include<vector>
#include<set>
#include<map>

using namespace std;

class KnowledgeInference {
	map<string, bool > facts;
	set<pair<string, string> > rules;

	vector<string > split(string q, char tok);
public:
	~KnowledgeInference() {
		facts.clear();
		rules.clear();
/*		cout << facts.size() << endl;
		for (auto f : facts)cout << f.first << ' ' << f.second << endl;		*/
	}
	void TellFact(string q);
	void TellRule(string q);
	void UpdateFacts();

	int Ask(string q);
	int AND(string q);
	int OR(string q);
};



