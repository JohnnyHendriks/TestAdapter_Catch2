# Configuration to use with MSVC142
include( "${CMAKE_CURRENT_LIST_DIR}/msvc142.flags.cmake" )

# Directory settings to use
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/msvc/_lib" CACHE PATH "Output path for static libraries" FORCE )
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_DEBUG "${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug static libraries" FORCE )
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_RELEASE "${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release static libraries" FORCE )

set( CMAKE_LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/msvc/_dll" CACHE PATH "Output path for dynamic libraries" FORCE )
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG "${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug dynamic libraries" FORCE )
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE "${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release dynamic libraries" FORCE )

set( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/build/ninja/msvc/_exe" CACHE PATH "Output path for executables" FORCE )
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Debug" CACHE PATH "Output path for debug executables" FORCE )
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/Release" CACHE PATH "Output path for release executables" FORCE )
