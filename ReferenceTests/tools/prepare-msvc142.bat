@echo off

echo Start setup of VS2019 solution...
echo.

cmake -G "Visual Studio 16 2019" -A x64 -T host=x64 -B ../build/msvc142 -S .. -C ../src/cmake/config/msvc142.config.cmake

echo.
echo Done.

pause
