@echo off

echo Start setup of VS2022 solution...
echo.

cmake --preset=msvc143 -S ..

call :BuildTests Release

echo.
echo Done.

pause

exit

:BuildTests

cmake --build ../build/msvc143 --config %~1
xcopy "..\build\msvc143\bin\%~1\*.exe" "..\..\_reftests\%~1" /I /Q

exit /B 0