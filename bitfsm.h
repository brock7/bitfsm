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

#ifndef __BIT_FSM_H__
#define __BIT_FSM_H__

#include <assert.h>
#include <memory.h>
#include <fstream>
#include <vector>

#ifndef _countof
#	define _countof(a) (sizeof(a) / sizeof((a)[0]))
#endif // _countof

#ifndef _size_to_dword
#	define _size_to_dword(n) (((n) - 1) / 32 + 1)
#endif // _size_to_dword

#ifndef _size_to_byte
#	define _size_to_byte(n) (((n) - 1) / 8 + 1)
#endif // _size_to_byte

namespace fsm {

	template<int N>
	class Bitset {

	private:
		typedef unsigned int _u32;

	public:
		Bitset() {
			clear();
		}

		Bitset(const Bitset<N> &_other) {
			set(_other);
		}

		~Bitset() {
		}

		void* ptr(void) {
			return (void*)raw;
		}

		void clear(void) {
			memset(raw, 0, sizeof(raw));
		}

		void set(int _pos, bool _val = true) {
			int _index = 0; int _offset = 0;
			_getRawArrayPos(_pos, _index, _offset);
			if(_val) {
				raw[_index] |= (1 << _offset);
			} else {
				raw[_index] &= ~(1 << _offset);
			}
		}

		void set(const Bitset<N> &_other) {
			memcpy(raw, _other.raw, sizeof(raw));
		}

		bool get(int _pos) {
			int _index = 0; int _offset = 0;
			_getRawArrayPos(_pos, _index, _offset);

			return (raw[_index] | (1 << _offset)) != 0;
		}

		bool empty(void) const {
			for(int i = 0; i < size; ++i) {
				if(raw[i] != 0) {
					return false;
				}
			}

			return true;
		}

		Bitset<N> &operator =(const Bitset<N> &_other) {
			set(_other);

			return *this;
		}

		bool operator ==(const Bitset<N> &_other) const {
			return memcmp(raw, _other.raw, sizeof(raw)) == 0;
		}

		bool operator !=(const Bitset<N> &_other) const {
			return memcmp(raw, _other.raw, sizeof(raw)) != 0;
		}

		Bitset<N> operator &(const Bitset<N> &_other) {
			Bitset<N> _bs;
			for(int i = 0; i < size; ++i) {
				_bs.raw[i] = raw[i] & _other.raw[i];
			}

			return _bs;
		}

		Bitset<N> operator |(const Bitset<N> &_other) {
			Bitset<N> _bs;
			for(int i = 0; i < size; ++i) {
				_bs.raw[i] = raw[i] | _other.raw[i];
			}

			return _bs;
		}

		Bitset<N> operator ~(void) {
			Bitset<N> _bs;
			for(int i = 0; i < size; ++i) {
				_bs.raw[i] = ~raw[i];
			}

			return _bs;
		}

		bool operator &(const Bitset<N> &_other) const {
			for(int i = 0; i < size; ++i) {
				if((raw[i] & _other.raw[i]) != 0) {
					return true;
				}
			}

			return false;
		}

		bool operator |(const Bitset<N> &_other) const {
			for(int i = 0; i < size; ++i) {
				if((raw[i] | _other.raw[i]) != 0) {
					return true;
				}
			}

			return false;
		}

		Bitset<N> &operator &=(const Bitset<N> &_other) {
			for(int i = 0; i < size; ++i) {
				raw[i] &= _other.raw[i];
			}

			return *this;
		}

		Bitset<N> &operator |=(const Bitset<N> &_other) {
			for(int i = 0; i < size; ++i) {
				raw[i] |= _other.raw[i];
			}

			return *this;
		}

	private:
		inline void _getRawArrayPos(int _pos, int &_index, int &_offset) {
			assert(_pos >= 0 && _pos < N);

			_index = _pos / 32;
			_offset = _pos % 32;
		}

	private:
		static const int size = _size_to_dword(N);
		_u32 raw[size];

	};

	struct _IntToInt {
		int operator ()(const int &_obj) {
			return _obj;
		}
	};

	template<int _Ns, int _Nc, typename _Tc = int, typename _CVi = _IntToInt, typename _CVc = _IntToInt>
	class FSM {

	public:
		typedef Bitset<_Nc> ConditionType;

		class StepHandler {

		public:
			virtual void handleStep(int _src, int _tgt) {
			}

			virtual void handleStep(const _Tc &_src, const _Tc &_tgt) {
			}

		};

		class TagStreamer {

		public:
			virtual void write(std::fstream &_fs, const _Tc &_tag) = 0;

			virtual void read(std::fstream &_fs, _Tc &_tag) = 0;

		};

		class CommandParams : public std::vector<_Tc> {

		public:
			void add(const _Tc &_data) {
				push_back(_data);
			}

			void reset(void) {
				clear();
			}

		};

	private:
		struct Status {
			Bitset<_Nc> status;
			int index;
			Status() {
				index = -1;
			}
		};

		struct Step {
			Bitset<_Nc> condition;
			int next;
			bool exact;
			Step() {
				next = -1;
				exact = false;
			}
			Step(const Bitset<_Nc> &_cond, int _next, bool _exact) {
				condition = _cond;
				next = _next;
				exact = _exact;
			}
		};

		typedef std::vector<Step> StepColl;

		struct RuleStep {
			int index;
			StepColl steps;
			_Tc tag;
			RuleStep() {
				index = -1;
			}
			bool walk(Status &_curr, const Bitset<_Nc> &_status, bool _exact) {
				if(index == -1) {
					return false;
				}

				Bitset<_Nc> _s = _curr.status | _status;
				for(StepColl::iterator _it = steps.begin(); _it != steps.end(); ++_it) {
					const Step &_ck = *_it;
					if((_ck.exact && _ck.condition == _s) || (!_ck.exact && (_ck.condition & _s))) {
						_curr.index = _ck.next;
						_curr.status = _exact ? _status : _s;

						return true;
					}
				}

				return false;
			}
		};

	public:
		FSM() {
			assert(_Nc > 0 && _Ns > 0);

			terminalIndex = -1;
			handler = 0;
			streamer = 0;
		}

		~FSM() {
			reset();
		}

		void reset(void) {
			current.status.clear();
			current.index = -1;
			terminalIndex = -1;
		}

		void clear(void) {
			reset();
			clearRuleStep();
		}

		bool setCurrentStep(int _index) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}
			current.index = _index;

			return true;
		}

		bool setCurrentStep(const _Tc &_obj) {
			int _index = objToIndex(_obj);

			return setCurrentStep(_index);
		}

		bool setTerminalStep(int _index) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}
			terminalIndex = _index;

			return true;
		}

		bool setTerminalStep(const _Tc &_obj) {
			int _index = objToIndex(_obj);

			return setTerminalStep(_index);
		}

		void setStepHandler(StepHandler* _hdl) {
			handler = _hdl;
		}

		void setTagStreamer(TagStreamer* _str) {
			streamer = _str;
		}

		bool registerRuleStepTag(int _index, const _Tc &_tag) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}
			ruleSteps[_index].tag = _tag;

			return true;
		}

		bool registerRuleStepTag(const _Tc &_tag) {
			int _index = objToIndex(_tag);

			return registerRuleStepTag(_index, _tag);
		}

		bool addRuleStep(int _index, const Bitset<_Nc> &_cond, int _next, bool _exact = false) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}

			StepColl &_steps = ruleSteps[_index].steps;
			for(StepColl::iterator _it = _steps.begin(); _it != _steps.end(); ++_it) {
				Step &_step = *_it;
				if(_step.condition == _cond) {
					return false;
				}
			}

			ruleSteps[_index].index = _index;
			ruleSteps[_index].steps.push_back(Step(_cond, _next, _exact));

			return true;
		}

		bool addRuleStep(const _Tc &_indexObj, const Bitset<_Nc> &_cond, const _Tc &_nextObj, bool _exact = false) {
			int _index = objToIndex(_indexObj);
			int _next = objToIndex(_nextObj);

			return addRuleStep(_index, _cond, _next, _exact);
		}

		bool addRuleStep(const _Tc &_indexObj, const CommandParams &_cond, const _Tc &_nextObj, bool _exact = false) {
			int _index = objToIndex(_indexObj);
			int _next = objToIndex(_nextObj);
			Bitset<_Nc> _condBits;
			for(CommandParams::const_iterator _cit = _cond.begin(); _cit != _cond.end(); ++_cit) {
				int _i = objToCommand(*_cit);
				_condBits.set(_i);
			}

			return addRuleStep(_index, _condBits, _next, _exact);
		}

		bool removeRuleStep(int _index, const Bitset<_Nc> &_cond) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}
			StepColl &_steps = ruleSteps[_index].steps;
			for(StepColl::iterator _it = _steps.begin(); _it != _steps.end(); ++_it) {
				Step &_step = *_it;
				if(_step.condition == _cond) {
					_steps.erase(_it);

					return true;
				}
			}

			return false;
		}

		bool removeRuleStep(const _Tc &_indexObj, const Bitset<_Nc> &_cond) {
			int _index = objToIndex(_indexObj);

			return removeRuleStep(_index, _cond);
		}

		bool removeRuleStep(const _Tc &_indexObj, const CommandParams &_cond) {
			int _index = objToIndex(_indexObj);
			Bitset<_Nc> _condBits;
			for(CommandParams::const_iterator _cit = _cond.begin(); _cit != _cond.end(); ++_cit) {
				int _i = objToCommand(*_cit);
				_condBits.set(_i);
			}

			return removeRuleStep(_index, _condBits);
		}

		void clearRuleStep(int _index) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(_index >= 0 && _index < _countof(ruleSteps)) {
				ruleSteps[_index].index = -1;
				ruleSteps[_index].tag = _Tc();
				std::swap(ruleSteps[_index].steps, StepColl());
			}
		}

		void clearRuleStep(void) {
			for(int i = 0; i < _Ns; ++i) {
				clearRuleStep(i);
			}
		}

		int getCurrentStep(void) const {
			return current.index;
		}

		const Bitset<_Nc> &getCurrentStatus(void) const {
			return current.status;
		}

		bool walk(int _status, bool _exact = false) {
			assert(_status >= 0 && _status < _Nc);
			if(!(_status >= 0 && _status < _Nc)) {
				return false;
			}
			if(current.index < 0 || current.index >= _Ns) {
				return false;
			}

			Bitset<_Nc> _bs; _bs.set(_status);

			int _srcIdx = current.index;
			const _Tc &_srcTag = ruleSteps[current.index].tag;
			bool _result = ruleSteps[current.index].walk(current, _bs, _exact);
			int _tgtIdx = current.index;
			const _Tc &_tgtTag = ruleSteps[current.index].tag;

			if(_result && handler) {
				handler->handleStep(_srcIdx, _tgtIdx);
				handler->handleStep(_srcTag, _tgtTag);
			}

			return _result;
		}

		bool walk(const _Tc &_obj, bool _exact = false) {
			int _status = objToCommand(_obj);

			return walk(_status, _exact);
		}

		bool terminated(void) const {
			return current.index == terminalIndex;
		}

		bool writeRuleSteps(const char* _file, int _ns = _Ns, int _nc = _Nc) {
			assert(_ns > 0 && _ns <= _Ns && _nc > 0 && _nc <= _Nc);

			bool _result = true;

			std::fstream _fs(_file, std::ios_base::out | std::ios_base::binary);
			if(_fs.fail()) {
				_result = false;
			} else {
				_fs.write((char*)&_ns, sizeof(_ns));
				_fs.write((char*)&_nc, sizeof(_nc));

				_fs.write((char*)&terminalIndex, sizeof(terminalIndex));

				for(int _i = 0; _i < _ns; ++_i) {
					RuleStep &_rule = ruleSteps[_i];

					_fs.write((char*)&_rule.index, sizeof(_rule.index));

					int _rsl = (int)_rule.steps.size();
					_fs.write((char*)&_rsl, sizeof(_rsl));
					for(int _j = 0; _j < _rsl; ++_j) {
						Step &_step = _rule.steps[_j];
						int _rs = _size_to_byte(_nc);
						_fs.write((char*)_step.condition.ptr(), _rs);
						_fs.write((char*)&_step.next, sizeof(_step.next));
						_fs.write((char*)&_step.exact, sizeof(_step.exact));
					}

					if(streamer) {
						streamer->write(_fs, _rule.tag);
					}
				}
				_fs.close();
			}

			return _result;
		}

		bool readRuleSteps(const char* _file) {
			bool _result = true;

			std::fstream _fs(_file, std::ios_base::in | std::ios_base::binary);
			if(_fs.fail()) {
				_result = false;
			} else {
				int _ns = 0; int _nc = 0;
				_fs.read((char*)&_ns, sizeof(_ns));
				_fs.read((char*)&_nc, sizeof(_nc));
				assert(_ns > 0 && _ns <= _Ns && _nc > 0 && _nc <= _Nc);

				_fs.read((char*)&terminalIndex, sizeof(terminalIndex));

				for(int _i = 0; _i < _ns; ++_i) {
					RuleStep &_rule = ruleSteps[_i];

					_fs.read((char*)&_rule.index, sizeof(_rule.index));

					int _rsl = 0;
					_fs.read((char*)&_rsl, sizeof(_rsl));
					for(int _j = 0; _j < _rsl; ++_j) {
						Step _step;
						int _rs = _size_to_byte(_nc);
						_fs.read((char*)_step.condition.ptr(), _rs);
						_fs.read((char*)&_step.next, sizeof(_step.next));
						_fs.read((char*)&_step.exact, sizeof(_step.exact));
						_rule.steps.push_back(_step);
					}

					if(streamer) {
						streamer->read(_fs, _rule.tag);
					}
				}

				_fs.close();
			}

			return _result;
		}

		int getStatusCount(void) const {
			return _Ns;
		}

		const _Tc &getStatusTag(int _index) const {
			assert(_index >= 0 && _index < _Nc);
			if(!(_index >= 0 && _index < _Nc)) {
				throw std::exception("Invalid index");
			}

			return ruleSteps[_index].tag;
		}

		int getCommandCount(int _index) const {
			assert(_index >= 0 && _index < _Nc);
			if(!(_index >= 0 && _index < _Nc)) {
				throw std::exception("Invalid index");
			}

			return ruleSteps[_index].steps.size();
		}

		const Bitset<_Nc> &getStepCommandCondition(int _index, int _step) const {
			if(!_ensureIndexAndStepIndexValid(_index, _step)) {
				throw std::exception("Invalid index");
			}

			return ruleSteps[_index].steps[_step].condition;
		}

		int getStepCommandNext(int _index, int _step) const {
			if(!_ensureIndexAndStepIndexValid(_index, _step)) {
				throw std::exception("Invalid index");
			}

			return ruleSteps[_index].steps[_step].next;
		}

		bool getStepCommandExact(int _index, int _step) const {
			if(!_ensureIndexAndStepIndexValid(_index, _step)) {
				throw std::exception("Invalid index");
			}

			return ruleSteps[_index].steps[_step].exact;
		}

	private:
		bool _ensureIndexAndStepIndexValid(int _index, int _step) const {
			assert(_index >= 0 && _index < _Nc);
			if(!(_index >= 0 && _index < _Nc)) {
				return false;
			}
			assert(_step >= 0 && _step < (int)ruleSteps[_index].steps.size());
			if(!(_step >= 0 && _step < (int)ruleSteps[_index].steps.size())) {
				return false;
			}

			return true;
		}

	private:
		Status current;
		RuleStep ruleSteps[_Ns];
		int terminalIndex;
		_CVi objToIndex;
		_CVc objToCommand;
		StepHandler* handler;
		TagStreamer* streamer;

	};

};

#endif // __BIT_FSM_H__
