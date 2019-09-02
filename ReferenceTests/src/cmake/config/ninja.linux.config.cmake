# Select compiler
set( CMAKE_C_COMPILER "/usr/local/gcc-9.1/bin/gcc-9.1" )
set( CMAKE_CXX_COMPILER "/usr/local/gcc-9.1/bin/g++-9.1" )
set( CMAKE_CXX_COMPILER_AR "/usr/local/gcc-9.1/bin/gcc-ar-9.1" )
set( CMAKE_CXX_COMPILER_RANLIB "/usr/local/gcc-9.1/bin/gcc-ranlib-9.1" )

# Configuration to use with GCC
include( "${CMAKE_CURRENT_LIST_DIR}/gcc91.flags.cmake" )

# Directory settings to use
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/linux/_lib" CACHE PATH "Output path for static libraries" FORCE )
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug static libraries" FORCE )
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release static libraries" FORCE )
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELWITHDEBINFO "${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/RelWithDebInfo" CACHE PATH "Output path for release static libraries with debug info" FORCE )

set( CMAKE_LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/linux/_dll" CACHE PATH "Output path for dynamic libraries" FORCE )
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG "${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug dynamic libraries" FORCE )
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE "${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release dynamic libraries" FORCE )
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELWITHDEBINFO "${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/RelWithDebInfo" CACHE PATH "Output path for release dynamic libraries with debug info" FORCE )

set( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/linux/_exe" CACHE PATH "Output path for executables" FORCE )
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug executables" FORCE )
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release executables" FORCE )
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELWITHDEBINFO "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/RelWithDebInfo" CACHE PATH "Output path for release executables with debug info" FORCE )
