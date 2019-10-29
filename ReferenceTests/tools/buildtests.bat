@echo off

echo Start setup of VS2019 solution...
echo.

:: CMake should be called from directory of root "CMakeList.txt"-file
:: Otherwise the toolchain file will make use of the incorrect reference path
:: to generate paths other paths with.
cd ..
cmake -G "Visual Studio 16 2019" -A x64 -T host=x64 -B ./build/msvc142 -S . -C ./src/cmake/config/msvc142.config.cmake

call :BuildTests Release
call :BuildTests Rel_0_1
call :BuildTests Rel_1_0
call :BuildTests Rel_1_1
call :BuildTests Rel_1_2
call :BuildTests Rel_2_0
call :BuildTests Rel_2_1
call :BuildTests Rel_2_2
call :BuildTests Rel_2_3
call :BuildTests Rel_3_0
call :BuildTests Rel_4_0
call :BuildTests Rel_4_1
call :BuildTests Rel_4_2
call :BuildTests Rel_5_0
call :BuildTests Rel_6_0
call :BuildTests Rel_6_1
call :BuildTests Rel_7_0
call :BuildTests Rel_7_1
call :BuildTests Rel_7_2
call :BuildTests Rel_8_0
call :BuildTests Rel_9_0
call :BuildTests Rel_9_1
call :BuildTests Rel_9_2
call :BuildTests Rel_10_0
call :BuildTests Rel_10_1
call :BuildTests Rel_10_2

cd tools

echo.
echo Done.

pause

:BuildTests

cmake --build ./build/msvc142 --config %~1
xcopy ".\build\msvc142\_exe\%~1\*.exe" "..\_reftests\%~1" /I /Q

exit /B 0