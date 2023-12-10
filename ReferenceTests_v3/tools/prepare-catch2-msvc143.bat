@echo off

echo Start setup of VS2022 solution...
echo.

::cmake --preset=msvc143 -S ..
cmake --preset=msvc143 -S ../catch2 --log-level=DEBUG

::Prepare files for version 3.0.1
call :BuildCatch2 Rel3_0_1 Catch2-3.0.1
call :BuildCatch2 Rel3_1_0 Catch2-3.1.0
call :BuildCatch2 Rel3_1_1 Catch2-3.1.1
call :BuildCatch2 Rel3_2_0 Catch2-3.2.0
call :BuildCatch2 Rel3_2_1 Catch2-3.2.1
call :BuildCatch2 Rel3_3_0 Catch2-3.3.0
call :BuildCatch2 Rel3_3_1 Catch2-3.3.1
call :BuildCatch2 Rel3_3_2 Catch2-3.3.2
call :BuildCatch2 Rel3_4_0 Catch2-3.4.0

echo.
echo Done.

pause

exit /b

:BuildCatch2

pushd "..\catch2\build\msvc143\catch2\%2"

cmake --preset=msvc143 -S . --log-level=DEBUG

cmake --build --preset=msvc143-build --config %~1
cmake --build --preset=msvc143-build --config Release
cmake --build --preset=msvc143-build --config Debug

:: Copy include files
pushd src
xcopy "*.hpp" "..\..\..\..\..\..\build\catch2\%1\include" /i /q /s
popd

pushd build
pushd generated-includes
xcopy "*.hpp" "..\..\..\..\..\..\..\build\catch2\%1\include" /i /q /s
popd
popd

: Copy lib and pdb files
xcopy "build\src\%1\*.lib" "..\..\..\..\..\build\catch2\%1\lib\%1" /i /q
xcopy "build\src\%1\*.pdb" "..\..\..\..\..\build\catch2\%1\lib\%1" /i /q

xcopy "build\src\Release\*.lib" "..\..\..\..\..\build\catch2\%1\lib\Release" /i /q
xcopy "build\src\Release\*.pdb" "..\..\..\..\..\build\catch2\%1\lib\Release" /i /q

xcopy "build\src\Debug\*.lib" "..\..\..\..\..\build\catch2\%1\lib\Debug" /i /q
xcopy "build\src\Debug\*.pdb" "..\..\..\..\..\build\catch2\%1\lib\Debug" /i /q

popd

exit /b 0
