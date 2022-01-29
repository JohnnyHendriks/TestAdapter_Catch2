@echo off

echo Start setup of VS2022 solution...
echo.

cmake --preset=msvc143 -S ..

::call :BuildTests Release
call :BuildTests Rel3_0_0

cd tools

echo.
echo Done.

pause

exit /b

:BuildTests

pushd ..

cmake --build --preset=msvc143-normal-build --config %~1
xcopy "build\msvc143\referencetestsv3\bin\%~1\*.exe" "..\_reftests\%~1" /I /Q

popd

exit /b 0