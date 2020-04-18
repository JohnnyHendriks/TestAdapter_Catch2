cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Dummy )

# Define source files
set( src_Files
  UT_Dummy.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Functionality" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
