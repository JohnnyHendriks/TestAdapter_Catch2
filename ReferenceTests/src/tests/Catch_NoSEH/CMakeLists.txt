cmake_minimum_required( VERSION 3.14 )

set( projectname Catch_NoSEH )

project( ${projectname} VERSION 1.0.0 )

# Define source files
set( src_Files
  UT_Tests01.cpp
)

# Add test executable
add_test_ut(
  ${projectname}
  "Functionality"
  SRC ${src_Files}
)

target_compile_definitions( ${projectname} PRIVATE CATCH_CONFIG_NO_WINDOWS_SEH)

# Group source files for IDE environments
source_group( "Files" FILES ${src_Files} )
