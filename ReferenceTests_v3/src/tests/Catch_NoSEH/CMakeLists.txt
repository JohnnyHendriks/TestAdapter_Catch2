cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_NoSEH )

# Define source files
set( src_Files
  UT_Tests01.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Functionality" )

target_compile_definitions( ${targetname} PRIVATE CATCH_CONFIG_NO_WINDOWS_SEH)

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
