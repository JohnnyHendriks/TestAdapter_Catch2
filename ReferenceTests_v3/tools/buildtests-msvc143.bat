@echo off

echo Start setup of VS2022 solution...
echo.

cmake --preset=msvc143 -S .. --log-level=DEBUG --fresh

::call :BuildTests Release
call :BuildTests Rel3_0_1
call :BuildTests Rel3_1_0
call :BuildTests Rel3_1_1
call :BuildTests Rel3_2_0
call :BuildTests Rel3_2_1
call :BuildTests Rel3_3_0
call :BuildTests Rel3_3_1
call :BuildTests Rel3_3_2
call :BuildTests Rel3_4_0

echo.
echo Done.

pause

exit /b

:BuildTests

pushd ..

cmake --build --preset=msvc143-normal-build --config %~1
xcopy "build\msvc143\referencetestsv3\bin\%~1\*.exe" "..\_reftests\%~1" /I /Q
xcopy "build\msvc143\referencetestsv3\bin\%~1\*.dll" "..\_reftests\%~1" /I /Q

popd

exit /b 0