@echo off

echo Start setup of VS2022 solution...
echo.

cmake --preset=msvc143 -S ..
::cmake --preset=msvc143 -S .. --log-level=DEBUG

echo.
echo Done.

pause
