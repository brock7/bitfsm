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

namespace fsm {

	struct ObjToStatus {
		int operator ()(const std::string &_obj);
	};

	struct ObjToCommand {
		int operator ()(const std::string &_obj);
	};

	typedef fsm::FSM<256, 256, std::string, ObjToStatus, ObjToCommand> Fsm;

	class MyTagStreamer : public Fsm::TagStreamer {

	public:
		virtual void write(std::fstream &_fs, const std::string &_tag);
		virtual void read(std::fstream &_fs, std::string &_tag);

	};

	public ref class Bitfsm {

	public:
		Bitfsm();
		virtual ~Bitfsm();

		Void config(String^ _globalCfg, String^ _statusCfg, String^ _commandCfg);

	protected:
		Fsm* fsm;
		MyTagStreamer* streamer;

	};

}
