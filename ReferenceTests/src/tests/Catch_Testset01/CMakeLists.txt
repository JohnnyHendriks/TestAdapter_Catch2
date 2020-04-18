cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Testset01 )

# Define source files
set( src_Files
  UT_Tests01.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Playground" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
