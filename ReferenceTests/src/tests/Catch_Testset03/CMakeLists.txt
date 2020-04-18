cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Testset03 )

# Define source files
set( src_Files
  UT_Tests01.cpp
  UT_Tests02.cpp
  UT_Tests03.cpp
  UT_Tests04.cpp
  UT_Tests05.cpp
  UT_Tests06.cpp
  UT_Tests07.cpp
  UT_Tests08.cpp
  UT_Tests09.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Playground" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
