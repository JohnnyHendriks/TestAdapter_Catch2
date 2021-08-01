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

# Set flags in CMakeCache
set( CMAKE_CONFIGURATION_TYPES
  Debug
  Release
  Rel_0_1
  Rel_1_0
  Rel_1_1
  Rel_1_2
  Rel_2_0
  Rel_2_1
  Rel_2_2
  Rel_2_3
  Rel_3_0
  Rel_4_0
  Rel_4_1
  Rel_4_2
  Rel_5_0
  Rel_6_0
  Rel_6_1
  Rel_7_0
  Rel_7_1
  Rel_7_2
  Rel_8_0
  Rel_9_0
  Rel_9_1
  Rel_9_2
  Rel_10_0
  Rel_10_1
  Rel_10_2
  Rel_11_0
  Rel_11_1
  Rel_11_2
  Rel_11_3
  Rel_12_0
  Rel_12_1
  Rel_12_2
  Rel_12_3
  Rel_12_4
  Rel_13_0
  Rel_13_1
  Rel_13_2
  Rel_13_3
  Rel_13_4
  Rel_13_5
  Rel_13_6
  CACHE STRING "Configuration types." FORCE)

set( CMAKE_CXX_FLAGS "${TAfC2_CXX_FLAGS}" CACHE STRING "Flags used by the CXX compiler during all build types." FORCE)
set( CMAKE_CXX_FLAGS_DEBUG "${TAfC2_CXX_FLAGS_DEBUG}" CACHE STRING "Flags used by the CXX compiler during DEBUG builds." FORCE)
set( CMAKE_CXX_FLAGS_RELEASE "${TAfC2_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during RELEASE builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_0_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_0_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_1_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_1_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_1_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_1_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_1_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_1_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_2_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_2_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_2_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_2_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_2_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_2_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_2_3 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_2_3 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_3_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_3_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_4_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_4_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_4_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_4_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_4_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_4_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_5_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_5_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_6_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_6_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_6_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_6_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_7_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_7_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_7_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_7_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_7_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_7_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_8_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_8_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_9_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_9_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_9_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_9_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_9_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_9_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_10_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_10_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_10_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_10_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_10_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_10_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_11_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_11_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_11_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_11_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_11_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_11_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_11_3 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_11_3 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_12_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_12_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_12_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_12_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_12_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_12_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_12_3 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_12_3 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_12_4 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_12_4 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_0 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_0 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_1 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_1 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_2 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_2 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_3 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_3 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_4 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_4 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_5 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_5 builds." FORCE)
set( CMAKE_CXX_FLAGS_REL_13_6 "${CMAKE_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during REL_13_6 builds." FORCE)

set( CMAKE_EXE_LINKER_FLAGS_DEBUG "${TAfC2_LINKER_FLAGS_DEBUG}" CACHE STRING "Flags used by the linker during DEBUG builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_RELEASE "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during RELEASE builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_0_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_0_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_1_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_1_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_1_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_2_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_2_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_2_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_2_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_3 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_3_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_3_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_4_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_4_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_4_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_5_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_5_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_6_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_6_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_6_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_6_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_7_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_7_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_7_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_8_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_8_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_9_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_9_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_9_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_10_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_10_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_10_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_11_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_11_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_11_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_11_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_3 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_12_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_12_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_12_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_12_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_3 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_12_4 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_4 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_0 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_1 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_2 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_3 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_4 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_4 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_5 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_5 builds." FORCE)
set( CMAKE_EXE_LINKER_FLAGS_REL_13_6 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_6 builds." FORCE)

set( CMAKE_SHARED_LINKER_FLAGS_DEBUG "${TAfC2_LINKER_FLAGS_DEBUG}" CACHE STRING "Flags used by the linker during DEBUG builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_RELEASE "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during RELEASE builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_0_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_0_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_1_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_1_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_1_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_1_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_2_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_2_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_2_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_2_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_2_3 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_3_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_3_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_4_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_4_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_4_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_4_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_5_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_5_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_6_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_6_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_6_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_6_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_7_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_7_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_7_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_7_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_8_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_8_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_9_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_9_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_9_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_9_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_10_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_10_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_10_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_10_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_11_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_11_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_11_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_11_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_11_3 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_12_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_12_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_12_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_12_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_3 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_12_4 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_12_4 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_0 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_0 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_1 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_1 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_2 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_2 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_3 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_3 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_4 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_4 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_5 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_5 builds." FORCE)
set( CMAKE_SHARED_LINKER_FLAGS_REL_13_6 "${TAfC2_LINKER_FLAGS_RELEASE}" CACHE STRING "Flags used by the linker during REL_13_6 builds." FORCE)
