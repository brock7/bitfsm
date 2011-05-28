#ifndef __BIT_FSM_H__
#define __BIT_FSM_H__

#include <assert.h>
#include <memory.h>
#include <vector>

#ifndef _countof
#	define _countof(a) (sizeof(a) / sizeof((a)[0]))
#endif // _countof

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
					return true;
				}
			}

			return false;
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
				if((raw[i] & _other.raw[i]) == 0) {
					return false;
				}
			}

			return true;;
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
		static const int size = N / (32 + 1) + 1;
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
		class StepHandler {

		public:
			virtual void handleStep(int _src, int _tgt) = 0;

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
			stopIndex = -1;
			handler = 0;
		}

		~FSM() {
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

		bool setStopStep(int _index) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
			}
			stopIndex = _index;

			return true;
		}

		bool setStopStep(const _Tc &_obj) {
			int _index = objToIndex(_obj);

			return setStopStep(_index);
		}

		void setStepHandler(StepHandler* _hdl) {
			handler = _hdl;
		}

		bool addRuleStep(int _index, const Bitset<_Nc> &_cond, int _next, bool _exact = false) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(!(_index >= 0 && _index < _countof(ruleSteps))) {
				return false;
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

		void clearRuleStep(int _index) {
			assert(_index >= 0 && _index < _countof(ruleSteps));
			if(_index >= 0 && _index < _countof(ruleSteps)) {
				ruleSteps[_index].index = -1;
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
			Bitset<_Nc> _bs; _bs.set(_status);

			int _src = current.index;
			bool _result = ruleSteps[current.index].walk(current, _bs, _exact);
			int _tgt = current.index;

			if(_result && handler) {
				handler->handleStep(_src, _tgt);
			}

			return _result;
		}

		bool walk(const _Tc &_obj, bool _exact = false) {
			int _status = objToCommand(_obj);

			return walk(_status, _exact);
		}

		bool isDone(void) const {
			return current.index == stopIndex;
		}

	private:
		Status current;
		RuleStep ruleSteps[_Ns];
		int stopIndex;
		_CVi objToIndex;
		_CVc objToCommand;
		StepHandler* handler;

	};

};

#endif // __BIT_FSM_H__
