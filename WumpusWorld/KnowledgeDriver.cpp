#include<iostream>

#include"KnowledgeInference.h"

vector<string > KnowledgeInference::split(string q, char tok) {
	vector<string > res;
	int lst = -1;
	for (int i = 0; i < q.size(); i++) {
		if (q[i] == tok) {
			if (i - lst - 2 > 0)res.push_back(q.substr(lst + 1, i - lst - 2));
			lst = i + 1;
		}
	}
	if (lst < (int)q.size()) {
		res.push_back(q.substr(lst + 1, q.size()));
	}
	return res;
}
void KnowledgeInference::TellFact(string q) {
	if (q.back() == '\'') {
		facts[q] = true;
		q.pop_back();
		facts[q] = false;
	}
	else {
		facts[q] = true;
		q += '\'';
		facts[q] = false;
	}
}


void KnowledgeInference::TellRule(string q) {
	int imp = q.find('=');
	string antecedent = q.substr(0, q.find('=') - 1);
	string consequent = q.substr(imp + 3, q.size());
	rules.insert({ antecedent,consequent });
}

int KnowledgeInference::Ask(string q) {
	return (facts.find(q) == facts.end() ? -1 : facts[q]);
}

int KnowledgeInference::OR(string q) {
	stringstream stream(q);
	bool od = 0;
	string literal;
	int res = -1, undef = 0;
	vector<string > allLiterals = split(q, '|');
	for (int i = 0; i < allLiterals.size(); i++) {
		if (allLiterals[i][0] == '(') allLiterals[i] = split(allLiterals[i], '(')[0];
		if (allLiterals[i].back() == ')')allLiterals[i] = split(allLiterals[i], ')')[0];
		if (facts.find(allLiterals[i]) == facts.end()) {
			undef++;
		}
		else {
			if (res == -1)res = facts[allLiterals[i]];
			else res |= facts[allLiterals[i]];
			if (res == 1)return 1;
		}
	}
	if (res || res == 0 && undef == 0)return res;
	return -1;
}

int KnowledgeInference::AND(string q) {
	string in, between;
	int val = -1, undef = 0;
	vector<string  > allExp = split(q, '&');
	for (auto in : allExp) {
		int res = OR(in);
		if (res != -1) {
			if (val == -1)val = res;
			else val &= res;
			if (val == 0)return 0;
		}
		else undef++;
	}

	if (val == 0 || val == 1 && undef == 0)return val;
	return -1;
}

void KnowledgeInference::UpdateFacts() {
	vector<pair<string, string > > del;
	for (auto rule:rules) {
		if (facts.find(rule.second) != facts.end()) {
			del.push_back(rule);
			continue;
		}
		int result = AND(rule.first);
		if (result == 1) {
			del.push_back(rule);
			TellFact(rule.second);
		}
	}
	for (auto e : del)rules.erase(e);
}



