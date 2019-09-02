cmake_minimum_required( VERSION 3.14 )

set_property( GLOBAL PROPERTY USE_FOLDERS ON )
enable_testing()

project( "ReferenceTests" VERSION 1.0.0 )

list( APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/src/cmake" )

add_subdirectory( src/tests )
