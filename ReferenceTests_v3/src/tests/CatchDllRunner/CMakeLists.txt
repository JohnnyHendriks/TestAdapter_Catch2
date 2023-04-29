cmake_minimum_required( VERSION 3.16 )

set( targetname CatchDllRunner )

# Define source files
set( src_Files
  main.cpp
)

# Add test executable
tafc2_add_runner( ${targetname} "Dll" )

# Add sources
target_sources(
  ${targetname}
  PRIVATE ${src_Files}
)
