@echo off

echo Start setup of VS2019 solution...
echo.

cmake --preset=msvc142 -S ..
::cmake --preset=msvc142 -S .. --log-level=DEBUG

echo.
echo Done.

pause
