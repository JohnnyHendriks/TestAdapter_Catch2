cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Execution )

# Define source files
set( src_Files
  UT_Fail.cpp
  UT_Mixed.cpp
  UT_Names.cpp
  UT_Pass.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Functionality" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
