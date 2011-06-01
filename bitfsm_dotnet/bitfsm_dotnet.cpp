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
		int _val = -1;
		if(Config::getSingleton().getStatusCfg().getData(_obj, _val)) {
			return _val;
		}

		return -1;
	}

	int ObjToCommand::operator ()(const std::string &_obj) {
		int _val = -1;
		if(Config::getSingleton().getCommandCfg().getData(_obj, _val)) {
			return _val;
		}

		return -1;
	}

	MyStepHandler::MyStepHandler() {
		managedHandler = 0;
	}

	void MyStepHandler::handleStep(const std::string &_srcTag, const std::string &_tgtTag) {
		if(managedHandler) {
			managedHandler(_srcTag, _tgtTag);
		}
	}

	void MyStepHandler::setManagedStepHandler(void* _ptr) {
		managedHandler = (ManagedStepHandlerPtr)_ptr;
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

	Dict::KeyCollection^ Bitfsm::StatusColl::get() {
		return statusColl->Keys;
	}

	Dict::KeyCollection^ Bitfsm::CommandColl::get() {
		return commandColl->Keys;
	}

	Bitfsm::Bitfsm() {
		fsm = new Fsm;
		handler = new MyStepHandler;
		managedStepHandlerDelegate = gcnew ManagedStepHandlerDelegate(this, &Bitfsm::onStep);
		IntPtr _hp = Marshal::GetFunctionPointerForDelegate(managedStepHandlerDelegate);
		void* _shp = _hp.ToPointer();
		handler->setManagedStepHandler(_shp);
		OnStepped = nullptr;
		streamer = new MyTagStreamer;
		fsm->setStepHandler(handler);
		fsm->setTagStreamer(streamer);

		statusColl = gcnew Dictionary<String^, Int32>();
		commandColl = gcnew Dictionary<String^, Int32>();
	}

	Bitfsm::~Bitfsm() {
		statusColl->Clear();
		commandColl->Clear();
		statusColl = nullptr;
		commandColl = nullptr;

		delete fsm; fsm = 0;
		delete streamer; streamer = 0;
		delete handler; handler = 0;
	}

	Void Bitfsm::config(String^ _globalCfg, String^ _statusCfg, String^ _commandCfg) {
		std::string _glb = cliStringToStl(_globalCfg);
		Config::getSingleton().getGlobalCfg().processFile(_glb);

		std::string _stt = cliStringToStl(_statusCfg);
		Config::getSingleton().getStatusCfg().processFile(_stt);

		std::string _cmd = cliStringToStl(_commandCfg);
		Config::getSingleton().getCommandCfg().processFile(_cmd);

		ConfigFile::iterator _sit = Config::getSingleton().getStatusCfg().begin();
		while(_sit != Config::getSingleton().getStatusCfg().end()) {
			const std::string &_key = _sit->first;
			int _val = -1;
			Config::getSingleton().getStatusCfg().getData(_key, _val);

			fsm->registerRuleStepTag(_val, _key);

			statusColl->Add(gcnew String(_key.c_str()), _val);

			++_sit;
		}

		ConfigFile::iterator _cit = Config::getSingleton().getCommandCfg().begin();
		while(_cit != Config::getSingleton().getCommandCfg().end()) {
			const std::string &_key = _cit->first;
			int _val = -1;
			Config::getSingleton().getCommandCfg().getData(_key, _val);

			commandColl->Add(gcnew String(_key.c_str()), _val);

			++_cit;
		}

		ConfigFile::iterator _git = Config::getSingleton().getGlobalCfg().begin();
		while(_git != Config::getSingleton().getGlobalCfg().end()) {
			const std::string &_key = _git->first;
			const std::string &_val = _git->second;

			if(_key == "__BEGIN_STEP__") {
				fsm->setCurrentStep(_val);
			} else if(_key == "__END_STEP__") {
				fsm->setTerminalStep(_val);
			}

			++_git;
		}
	}

	Boolean Bitfsm::addRuleStep(String^ _index, cli::array<String^>^ _params, String^ _next, Boolean _exact) {
		std::string _idx = cliStringToStl(_index);
		std::string _nxt = cliStringToStl(_next);
		Fsm::CommandParams _par;
		_par.reset();
		for each(String^ _param in _params) {
			std::string _p = cliStringToStl(_param);
			_par.add(_p);
		}
		Boolean _result = fsm->addRuleStep(_idx, _par, _nxt, _exact);

		return _result;
	}

	Boolean Bitfsm::removeRuleStep(String^ _index, cli::array<String^>^ _params) {
		std::string _idx = cliStringToStl(_index);
		Fsm::CommandParams _par;
		_par.reset();
		for each(String^ _param in _params) {
			std::string _p = cliStringToStl(_param);
			_par.add(_p);
		}
		Boolean _result = fsm->removeRuleStep(_idx, _par);

		return _result;
	}

	Boolean Bitfsm::setCurrentStep(String^ _index) {
		std::string _s = cliStringToStl(_index);

		return fsm->setCurrentStep(_s);
	}

	Boolean Bitfsm::setTerminalStep(String^ _index) {
		std::string _s = cliStringToStl(_index);

		return fsm->setTerminalStep(_s);
	}

	Boolean Bitfsm::writeRuleSteps(String^ _file, Int32 _ns, Int32 _nc) {
		std::string _f = cliStringToStl(_file);

		return fsm->writeRuleSteps(_f.c_str(), _ns, _nc);
	}

	Void Bitfsm::clear() {
		fsm->clear();
	}

	Void Bitfsm::reset() {
		fsm->reset();
	}

	Boolean Bitfsm::readRuleSteps(String^ _file) {
		std::string _f = cliStringToStl(_file);

		return fsm->readRuleSteps(_f.c_str());
	}

	Boolean Bitfsm::walk(String^ _cmd, Boolean _exact) {
		std::string _c = cliStringToStl(_cmd);

		return fsm->walk(_c, _exact);
	}

	Boolean Bitfsm::terminated() {
		return fsm->terminated();
	}

	Void Bitfsm::onStep(const std::string &_srcTag, const std::string &_tgtTag) {
		if(OnStepped != nullptr) {
			StepEventArgs^ _e = gcnew StepEventArgs;
			_e->sourceTag = gcnew String(_srcTag.c_str());
			_e->targetTag = gcnew String(_tgtTag.c_str());
			OnStepped(this, _e);
		}
	}

};
