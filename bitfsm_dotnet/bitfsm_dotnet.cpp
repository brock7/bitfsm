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

#include "stdafx.h"
#include "bitfsm_dotnet.h"
#include "configfile.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace fsm {

	class Config : public Singleton<Config> {

	public:
		Config() {
		}
		virtual ~Config() {
		}

		ConfigFile &getGlobalCfg(void) {
			return globalCfg;
		}
		ConfigFile &getStatusCfg(void) {
			return statusCfg;
		}
		ConfigFile &getCommandCfg(void) {
			return commandCfg;
		}

	private:
		ConfigFile globalCfg;
		ConfigFile statusCfg;
		ConfigFile commandCfg;

	};

	int ObjToStatus::operator ()(const std::string &_obj) {
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

	int ObjToCommand::operator ()(const std::string &_obj) {
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

	void MyTagStreamer::write(std::fstream &_fs, const std::string &_tag) {
		int _len = _tag.length() + 1;
		_fs.write((char*)&_len, sizeof(_len));
		_fs.write(_tag.c_str(), _len);
	}

	void MyTagStreamer::read(std::fstream &_fs, std::string &_tag) {
		int _len = 0;
		char _buf[1024];
		_fs.read((char*)&_len, sizeof(_len));
		_fs.read(_buf, _len);
		_tag = _buf;
	}

	std::string cliStringToStl(String^ _str) {
		IntPtr _p = Marshal::StringToHGlobalAnsi(_str);
		const char* _c_str = static_cast<char*>(_p.ToPointer());
		std::string _result(_c_str);
		Marshal::FreeHGlobal(_p);

		return _result;
	}

	Bitfsm::Bitfsm() {
		fsm = new Fsm;
		streamer = new MyTagStreamer;
		fsm->setTagStreamer(streamer);
	}

	Bitfsm::~Bitfsm() {
		delete fsm;
		delete streamer;
		fsm = 0;
	}

	Void Bitfsm::config(String^ _globalCfg, String^ _statusCfg, String^ _commandCfg) {
		std::string _glb = cliStringToStl(_globalCfg);
		Config::getSingleton().getGlobalCfg().processFile(_glb);

		std::string _stt = cliStringToStl(_statusCfg);
		Config::getSingleton().getGlobalCfg().processFile(_stt);

		std::string _cmd = cliStringToStl(_commandCfg);
		Config::getSingleton().getGlobalCfg().processFile(_cmd);
	}

};
