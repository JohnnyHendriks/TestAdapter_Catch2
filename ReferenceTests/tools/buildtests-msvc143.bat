@echo off

echo Start setup of VS2022 solution...
echo.

cmake --preset=msvc143 -S ..

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
call :BuildTests Rel_11_0
call :BuildTests Rel_11_1
call :BuildTests Rel_11_2
call :BuildTests Rel_11_3
call :BuildTests Rel_12_0
call :BuildTests Rel_12_1
call :BuildTests Rel_12_2
call :BuildTests Rel_12_3
call :BuildTests Rel_12_4
call :BuildTests Rel_13_0
call :BuildTests Rel_13_1
call :BuildTests Rel_13_2
call :BuildTests Rel_13_3
call :BuildTests Rel_13_4
call :BuildTests Rel_13_5
call :BuildTests Rel_13_6
call :BuildTests Rel_13_7
call :BuildTests Rel_13_8
call :BuildTests Rel_13_9
call :BuildTests Rel_13_10

cd tools

echo.
echo Done.

pause

exit

:BuildTests

cmake --build ../build/msvc143 --config %~1
xcopy "..\build\msvc143\bin\%~1\*.exe" "..\..\_reftests\%~1" /I /Q

exit /B 0