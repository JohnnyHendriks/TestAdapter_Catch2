# Configuration to use with MSVC 14.2

# CXX flags shared by all
string(APPEND TAfC2_CXX_FLAGS " /permissive- /std:c++17") # Language opions
string(APPEND TAfC2_CXX_FLAGS " /W4")                     # Use Warning Level 4
string(APPEND TAfC2_CXX_FLAGS " /EHsc")                   # Exception handling mode. sc= catch C++ exceptions only and extern "C" functions never throw
string(APPEND TAfC2_CXX_FLAGS " /GR")                     # Enables run-time type information

string(STRIP ${TAfC2_CXX_FLAGS} TAfC2_CXX_FLAGS)

# CXX flags Debug
string(APPEND TAfC2_CXX_FLAGS_DEBUG " /MDd")  # Use the debug multithread-specific and DLL-specific version of the run-time library
string(APPEND TAfC2_CXX_FLAGS_DEBUG " /Zi")   # Produce a separate PDB file that contains all the symbolic debugging information for use with the debugger
string(APPEND TAfC2_CXX_FLAGS_DEBUG " /Od")   # Disable optimization
string(APPEND TAfC2_CXX_FLAGS_DEBUG " /RTC1") # Enable runtime error checks

string(STRIP ${TAfC2_CXX_FLAGS_DEBUG} TAfC2_CXX_FLAGS_DEBUG)

# CXX flags Release
string(APPEND TAfC2_CXX_FLAGS_RELEASE " /MD")  # Use the multithread-specific and DLL-specific version of the run-time library
string(APPEND TAfC2_CXX_FLAGS_RELEASE " /Zi")  # Produce a separate PDB file that contains all the symbolic debugging information for use with the debugger
string(APPEND TAfC2_CXX_FLAGS_RELEASE " /O2")  # Optimize for speed

string(APPEND TAfC2_CXX_FLAGS_RELEASE " /DNDEBUG") # Define NDEBUG (disables assert)

string(STRIP ${TAfC2_CXX_FLAGS_RELEASE} TAfC2_CXX_FLAGS_RELEASE)

# Linker flags Debug
string(APPEND TAfC2_LINKER_FLAGS_DEBUG " /DEBUG:FASTLINK")  # Create debugging information (can be used on local machine only)

string(STRIP ${TAfC2_LINKER_FLAGS_DEBUG} TAfC2_LINKER_FLAGS_DEBUG)

# Linker flags Release
string(APPEND TAfC2_LINKER_FLAGS_RELEASE " /DEBUG:FULL")     # Create debugging information (can be used with deployed executable)
string(APPEND TAfC2_LINKER_FLAGS_RELEASE " /INCREMENTAL:NO") # Turn of incremental linking

string(STRIP ${TAfC2_LINKER_FLAGS_RELEASE} TAfC2_LINKER_FLAGS_RELEASE)

set( tafc2_CATCH2_VERSION_CONFIGURATION_TYPES
  Rel3_0_1
  Rel3_1_0
  Rel3_1_1
  Rel3_2_0
  Rel3_2_1
  Rel3_3_0
  Rel3_3_1
  Rel3_3_2
  Rel3_4_0
  Rel3_5_0
)

# Set flags in CMakeCache
set( CMAKE_CONFIGURATION_TYPES
  Debug
  Release
  ${tafc2_CATCH2_VERSION_CONFIGURATION_TYPES}
  CACHE STRING "Configuration types." FORCE)

set( CMAKE_CXX_FLAGS "${TAfC2_CXX_FLAGS}" CACHE STRING "Flags used by the CXX compiler during all build types." FORCE)
set( CMAKE_CXX_FLAGS_DEBUG "${TAfC2_CXX_FLAGS_DEBUG}" CACHE STRING "Flags used by the CXX compiler during DEBUG builds." FORCE)
set( CMAKE_CXX_FLAGS_RELEASE "${TAfC2_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during RELEASE builds." FORCE)

foreach( configtype IN LISTS tafc2_CATCH2_VERSION_CONFIGURATION_TYPES )

  string( TOUPPER ${configtype} namepostfix )

  set( CMAKE_CXX_FLAGS_${namepostfix} "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during ${configtype} builds." FORCE)
  set( CMAKE_EXE_LINKER_FLAGS_${namepostfix} "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during ${configtype} builds." FORCE)
  set( CMAKE_SHARED_LINKER_FLAGS_${namepostfix} "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during ${configtype} builds." FORCE)

endforeach()
