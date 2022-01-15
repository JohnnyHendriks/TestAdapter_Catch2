@echo off

echo Start setup of VS2019 solution...
echo.

cmake --preset=msvc142 -S ..

call :BuildTests Release

echo.
echo Done.

pause

exit

:BuildTests

cmake --build ../build/msvc142 --config %~1
xcopy "..\build\msvc142\bin\%~1\*.exe" "..\..\_reftests\%~1" /I /Q

exit /B 0