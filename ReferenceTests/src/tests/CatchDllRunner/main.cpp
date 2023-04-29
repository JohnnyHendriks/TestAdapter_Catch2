/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

// Windows
#include <windows.h>
#include <string>

using catch2_main = int (*)(int argc, char** argv);

int main(int argc, char* argv[])
{
    std::string dllname(argv[argc - 1]);
    HMODULE hTestDll = LoadLibraryA(argv[argc-1]);

    if( hTestDll == nullptr )
    {
        return GetLastError();
    }

    auto testMain = (catch2_main)GetProcAddress(hTestDll, "catch2_main");
    if ( testMain == nullptr )
    {
      auto lasterror = GetLastError();
      return -1;
    }
    int result = testMain(argc-1, argv);

    FreeLibrary(hTestDll);

    return result;
}
