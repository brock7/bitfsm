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

#pragma once

#include <string>

#include "../bitfsm.h"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;

namespace fsm {

	struct ObjToStatus {
		int operator ()(const std::string &_obj);
	};

	struct ObjToCommand {
		int operator ()(const std::string &_obj);
	};

	typedef fsm::FSM<256, 256, std::string, ObjToStatus, ObjToCommand> Fsm;

	typedef void(* ManagedStepHandlerPtr)(const std::string &_srcTag, const std::string &_tgtTag);

	class MyStepHandler : public Fsm::StepHandler {

	public:
		MyStepHandler();

		virtual void handleStep(const std::string &_srcTag, const std::string &_tgtTag);

		void setManagedStepHandler(void* _ptr);

	private:
		ManagedStepHandlerPtr managedHandler;

	};

	class MyTagStreamer : public Fsm::TagStreamer {

	public:
		virtual void write(std::fstream &_fs, const std::string &_tag);
		virtual void read(std::fstream &_fs, std::string &_tag);

	};

	typedef Dictionary<String^, Int32> Dict;

	public ref class Bitfsm {

	public:
		delegate Void ManagedStepHandlerDelegate(const std::string &_srcTag, const std::string &_tgtTag);

	public:
		ref struct StepEventArgs {
			String^ sourceTag;
			String^ targetTag;
		};

		delegate Void StepHandler(Object^ sender, StepEventArgs^ e);

	protected:
		StepHandler^ OnStepped;

	public:
		event StepHandler^ Stepped {
			void add(StepHandler^ _d) {
				OnStepped += _d;
			}
			void remove(StepHandler^ _d) {
				OnStepped -= _d;
			}
			void raise(Object^ sender, StepEventArgs^ e) {
				StepHandler^ _tmp = OnStepped;
				if (_tmp) {
					_tmp->Invoke(sender, e);
				}
			}
		}

	public:
		property Dict::KeyCollection^ StatusColl {
			Dict::KeyCollection^ get();
		}
		property Dict::KeyCollection^ CommandColl {
			Dict::KeyCollection^ get();
		}

	public:
		Bitfsm();
		virtual ~Bitfsm();

		Void config(String^ _globalCfg, String^ _statusCfg, String^ _commandCfg);

		Boolean addRuleStep(String^ _index, cli::array<String^>^ _params, String^ _next, Boolean _exact);
		Boolean removeRuleStep(String^ _index, cli::array<String^>^ _params);

		Boolean setCurrentStep(String^ _index);
		Boolean setTerminalStep(String^ _index);

		Boolean writeRuleSteps(String^ _file, Int32 _ns, Int32 _nc);
		Void clear();
		Void reset();
		Boolean readRuleSteps(String^ _file);

		Boolean walk(String^ _cmd, Boolean _exact);
		Boolean terminated();

		Void onStep(const std::string &_srcTag, const std::string &_tgtTag);

	protected:
		Dict^ statusColl;
		Dict^ commandColl;
		Fsm* fsm;
		MyTagStreamer* streamer;
		MyStepHandler* handler;
		ManagedStepHandlerDelegate^ managedStepHandlerDelegate;

	};

}
