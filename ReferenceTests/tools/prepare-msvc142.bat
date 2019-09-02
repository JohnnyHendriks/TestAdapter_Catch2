@echo off

echo Start setup of VS2019 solution...
echo.

:: CMake should be called from directory of root "CMakeList.txt"-file
:: Otherwise the toolchain file will make use of the incorrect reference path
:: to generate paths other paths with.
cd ..
cmake -G "Visual Studio 16 2019" -A x64 -T host=x64 -B ./build/msvc142 -S . -C ./src/cmake/config/msvc142.config.cmake
cd tools

echo.
echo Done.

pause
