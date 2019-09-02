cmake_minimum_required( VERSION 3.14 )

set( projectname Catch_Hidden )

project( ${projectname} VERSION 1.0.0 )

# Define source files
set( src_Files
  UT_Hidden.cpp
  UT_NotHidden.cpp
)

# Add test executable
add_test_ut(
  ${projectname}
  "Functionality"
  SRC ${src_Files}
)

# Group source files for IDE environments
source_group( "Files" FILES ${src_Files} )
