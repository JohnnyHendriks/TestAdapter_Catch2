cmake_minimum_required( VERSION 3.14 )

set( projectname Catch_Dummy )

project( ${projectname} VERSION 1.0.0 )

# Define source files
set( src_Files
  UT_Dummy.cpp
)

set( src_Catch2
  main.cpp
  ../catch_discover.hpp
)

# Add test executable
add_test_ut(
  ${projectname}
  "Functionality"
  SRC ${src_Files}
  MAIN ${src_Catch2}
)

# Group source files for IDE environments
source_group( "Files" FILES ${src_Files} )
source_group( Files\\Catch2 FILES ${src_Catch2} )
