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

#ifndef __TONYs_DATA_FILE__
#define __TONYs_DATA_FILE__

#include <map>
#include <string>
#include <locale>

namespace fsm {

	const int SINGLE_LINE_BUFFER_SIZE = 10240;

	struct AutoLocale {

	public:
		AutoLocale() {
			_old = std::locale::global(std::locale(""));
		}
		~AutoLocale() {
			std::locale::global(_old);
		}

	private:
		std::locale _old;

	};
	#define AUTO_LOCALE AutoLocale (__al__)

	class ConfigFile {

	public:
		typedef std::map<std::string, std::string>::iterator iterator;
		std::map<std::string, std::string> mElems;
		std::string mComment;

	public:
		static std::string _trim(std::string &s) {
			if(s.empty()) {
				return s;
			}
			size_t i = 0;
			size_t j = s.length() - 1;
			for( ; i < s.length(); i++) {
				if(s[i] != '\t' &&
					s[i] != ' ' &&
					s[i] != '\r' &&
					s[i] != '\n') {
						break;
				}
			}
			for( ; j > 0; j--) {
				if(s[j] != '\t' &&
					s[j] != ' ' &&
					s[j] != '\r' &&
					s[j] != '\n') {
						break;
				}
			}

			return s.substr(i, j - i + 1);
		}

	private:
		bool _processLine(const std::string &line) {
			int count = 0;
			int index = -1;
			for(size_t i = 0; i < line.length(); i++) {
				if(line[i] == '=') {
					if(count == 0) {
						index = i;
					}
					count++;
				}
			}
			if(count != 1) {
				return false;
			}

			std::string b = line.substr(0, index);
			std::string a = line.substr(index + 1, line.length() - index);
			b = _trim(b);
			a = _trim(a);
			mElems[b] = a;

			return true;
		}

		bool isComment(const std::string &_str, size_t _index) {
			for(size_t i = 0; i < mComment.length(); i++) {
				if(_str[_index + i] != mComment[i]) {
					return false;
				}
			}

			return true;
		}

	public:
		ConfigFile() {
			mComment = "//";
			mElems.clear();
		}

		ConfigFile(const std::string &comment) {
			setCommentFormat(comment);
			mElems.clear();
		}

		virtual ~ConfigFile() {
			mElems.clear();
		}

		void setCommentFormat(const std::string &comment) {
			if(comment.length() > 0) {
				mComment = comment;
			}
		}

		int processFile(const std::string &fileName) {
			AUTO_LOCALE;

			char* emptyLine = (char*)malloc(2); emptyLine[0] = '\r'; emptyLine[1] = '\0';
			mElems.clear();
			std::fstream fs(fileName.c_str(),
				std::ios_base::in | std::ios_base::binary);
			if(fs.fail()) {
				free(emptyLine);

				return -1;
			}
			while(!fs.eof()) {
				char buf[SINGLE_LINE_BUFFER_SIZE] = "";
				fs.getline(buf, SINGLE_LINE_BUFFER_SIZE);
				std::string sBuf(buf);
				if(sBuf.length() >= mComment.length()) {
					if(isComment(sBuf, 0)) {
						continue;
					}
					for(size_t i = 0; i < sBuf.length() - 1; i++) {
						if(isComment(sBuf, i)) {
							sBuf = sBuf.substr(0, i);
							break;
						}
					}
				}
				if(0 == strcmp(sBuf.c_str(), emptyLine)) {
					continue;
				}
				_processLine(sBuf);
			}
			fs.close();
			free(emptyLine);

			return mElems.size();
		}

		bool saveToFile(const std::string &fileName) {
			AUTO_LOCALE;
			std::fstream fs(fileName.c_str(),
				std::ios_base::out | std::ios_base::binary);
			if(fs.fail()) {
				return false;
			}

			for(std::map<std::string, std::string>::iterator it = mElems.begin(); it != mElems.end(); it++) {
				fs.write(it->first.c_str(), it->first.length());
				fs.write("=", 1);
				fs.write(it->second.c_str(), it->second.length());
				fs.write("\r\n", 2);
			}
			fs.close();

			return true;
		}

		size_t getDataCount(void) {
			return mElems.size();
		}

		bool hasData(const std::string &key) {
			return mElems.end() != mElems.find(key);
		}

		int eraseData(const std::string &key) {
			if(!hasData(key)) {
				return 0;
			}
			mElems.erase(key);

			return 1;
		}

		void clearData(void) {
			mElems.clear();
		}

		template<typename T>
		bool getData(const std::string &key, T &val) {
			if(!hasData(key)) {
				return false;
			}
			std::stringstream ss(mElems[key]);
			T t;
			ss >> t;
			val = t;

			return true;
		}

		template<typename Tk, typename Tv>
		bool getData(const std::string &key, Tk &_key, Tv &_val) {
			if(!hasData(key)) {
				return false;
			}
			std::stringstream sk(key);
			Tk tk;
			sk >> tk;
			_key = tk;
			std::stringstream sv(mElems[key]);
			Tv tv;
			sv >> tv;
			_val = tv;

			return true;
		}

		template<typename T>
		void setData(const std::string &key, T val) {
			std::stringstream ss;
			T t = val;
			ss << t;
			std::string sv;
			ss >> sv;
			mElems[key] = sv;
		}

		iterator begin(void) {
			return mElems.begin();
		}

		iterator end(void) {
			return mElems.end();
		}

	};

};

#endif
