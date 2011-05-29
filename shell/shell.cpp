/*
** This source file is part of BITFSM
**
** For the latest info, see http://code.google.com/p/bitfsm/
**
** Copyright (c) 2011 Tony & Tony's Toy Game Development Team
**
** Permission is hereby granted, free of charge, to any person obtaining a copy of
** this software and associated documentation files (the "Software"), to deal in
** the Software without restriction, including without limitation the rights to
** use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
** the Software, and to permit persons to whom the Software is furnished to do so,
** subject to the following conditions:
**
** The above copyright notice and this permission notice shall be included in all
** copies or substantial portions of the Software.
**
** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
** FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
** COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
** IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
** CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#include <stdlib.h>
#include <stdio.h>
#include <string>

#include "../bitfsm.h"

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
	virtual void handleStep(const std::string &_srcTag, const std::string &_tgtTag) {
		printf("Status changed from %s to %s\n", _srcTag.c_str(), _tgtTag.c_str());
	}

};

class MyTagStreamer : public Fsm::TagStreamer {

public:
	virtual void write(std::fstream &_fs, const std::string &_tag) {
		int _len = _tag.length() + 1;
		_fs.write((char*)&_len, sizeof(_len));
		_fs.write(_tag.c_str(), _len);
	}

	virtual void read(std::fstream &_fs, std::string &_tag) {
		int _len = 0;
		char _buf[1024];
		_fs.read((char*)&_len, sizeof(_len));
		_fs.read(_buf, _len);
		_tag = _buf;
	}

};

int main(int argc, char* argv[]) {
	bool done = false;

	Fsm::CommandParams params;
	Fsm sbs;

	Fsm::StepHandler* hdl = new MyStepHandler;
	Fsm::TagStreamer* str = new MyTagStreamer;
	sbs.setStepHandler(hdl);
	sbs.setTagStreamer(str);

	{
		sbs.registerRuleStepTag("begin");
		sbs.registerRuleStepTag("1");
		sbs.registerRuleStepTag("2");
		sbs.registerRuleStepTag("3");
		sbs.registerRuleStepTag("end");
	}

	{
		sbs.setCurrentStep("begin");
		sbs.setTerminalStep("end");

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
		sbs.writeRuleSteps("backup.fsm");
		sbs.reset();
		sbs.readRuleSteps("backup.fsm");
	}

	{
		sbs.setCurrentStep("begin");

		sbs.walk("_cmd0");
		done = sbs.terminated();

		sbs.walk("_cmd3");
		done = sbs.terminated();

		sbs.walk("_cmd5");
		done = sbs.terminated();
	}

	delete hdl;
	delete str;

	system("pause");

	return 0;
}
