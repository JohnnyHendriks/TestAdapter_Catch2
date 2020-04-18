@echo off

echo Start setup of VS2019 solution...
echo.

cmake -G "Visual Studio 16 2019" -A x64 -T host=x64 -B ../build/msvc142 -S .. -C ../src/cmake/config/msvc142.config.cmake

call :BuildTests Release

echo.
echo Done.

pause

exit

:BuildTests

cmake --build ../build/msvc142 --config %~1
xcopy "..\build\msvc142\bin\%~1\*.exe" "..\..\_reftests\%~1" /I /Q

exit /B 0