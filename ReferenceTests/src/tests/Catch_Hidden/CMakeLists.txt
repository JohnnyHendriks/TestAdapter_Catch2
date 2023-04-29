cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Hidden )
set( targetnamedll CatchDll_Hidden )

# Define source files
set( src_Files
  UT_Hidden.cpp
  UT_NotHidden.cpp
)

# Add test executable
tafc2_add_test( ${targetname} "Functionality" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)

# Add test dll
tafc2_add_dlltest( ${targetnamedll} "Dll" )

# Add sources
target_sources(
  ${targetnamedll}
  PRIVATE ${src_Files}
)
