#include <stdlib.h>
#include <stdio.h>
#include <string>

#include "../bitfsm.h"

enum Status {
	S_BEGIN,
	S_1,
	S_2,
	S_3,
	S_END,
};

struct ObjToStatus {
	int operator ()(const std::string &_obj) {
		if(_obj == "begin") {
			return 0;
		} else if(_obj == "1") {
			return 1;
		} else if(_obj == "2") {
			return 2;
		} else if(_obj == "3") {
			return 3;
		} else if(_obj == "end") {
			return 4;
		}

		return -1;
	}
};

struct ObjToCommand {
	int operator ()(const std::string &_obj) {
		if(_obj == "_cmd0") {
			return 0;
		} else if(_obj == "_cmd1") {
			return 1;
		} else if(_obj == "_cmd2") {
			return 2;
		} else if(_obj == "_cmd3") {
			return 3;
		} else if(_obj == "_cmd4") {
			return 4;
		} else if(_obj == "_cmd5") {
			return 5;
		}

		return -1;
	}
};

typedef fsm::FSM<5, 6, std::string, ObjToStatus, ObjToCommand> Fsm;

class MyStepHandler : public Fsm::StepHandler {

public:
	virtual void handleStep(int _src, int _tgt) {
		printf("Status changed from %d to %d\n", _src, _tgt);
	}

};

int main(int argc, char* argv[]) {
	bool done = false;

	Fsm::CommandParams params;
	Fsm sbs;

	Fsm::StepHandler* hdl = new MyStepHandler;
	sbs.setStepHandler(hdl);

	{
		sbs.setCurrentStep("begin");
		sbs.setStopStep("end");

		params.reset();
		params.add("_cmd0");
		sbs.addRuleStep("begin", params, "1");

		params.reset();
		params.add("_cmd1");
		sbs.addRuleStep("begin", params, "2");

		params.reset();
		params.add("_cmd3");
		sbs.addRuleStep("1", params, "3");

		params.reset();
		params.add("_cmd4");
		sbs.addRuleStep("2", params, "3");

		params.reset();
		params.add("_cmd5");
		sbs.addRuleStep("3", params, "end");
	}

	{
		sbs.walk("_cmd0");
		done = sbs.isDone();

		sbs.walk("_cmd3");
		done = sbs.isDone();

		sbs.walk("_cmd5");
		done = sbs.isDone();
	}

	delete hdl;

	system("pause");

	return 0;
}
