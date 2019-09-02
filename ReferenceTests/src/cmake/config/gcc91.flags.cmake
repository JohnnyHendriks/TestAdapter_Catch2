# Configuration to use with GCC 9.1

# CXX flags shared by all
string(APPEND SciDaP_CXX_FLAGS " -std=c++17") # Language opions
string(APPEND SciDaP_CXX_FLAGS " -Wall")      # Enable most warnings
string(APPEND SciDaP_CXX_FLAGS " -Wextra")    # Enable extra warnings not enabled by 'Wall'

string(STRIP ${SciDaP_CXX_FLAGS} SciDaP_CXX_FLAGS)

# CXX flags Debug
string(APPEND SciDaP_CXX_FLAGS_DEBUG " -ggdb") # Produce debugging information for use by GDB

string(STRIP ${SciDaP_CXX_FLAGS_DEBUG} SciDaP_CXX_FLAGS_DEBUG)

# CXX flags Release
string(APPEND SciDaP_CXX_FLAGS_RELEASE " -O3")  # Use optimization level 3

string(APPEND SciDaP_CXX_FLAGS_RELEASE " -D NDEBUG") # Define NDEBUG (disables assert)

string(STRIP ${SciDaP_CXX_FLAGS_RELEASE} SciDaP_CXX_FLAGS_RELEASE)

# CXX flags RelWithDebInfo
string(APPEND SciDaP_CXX_FLAGS_RELWITHDEBINFO ${SciDaP_CXX_FLAGS_RELEASE})  # Use same settings as Release

string(APPEND SciDaP_CXX_FLAGS_RELWITHDEBINFO " -ggdb") # Produce debugging information for use by GDB

# Set flags in CMakeCache
set( CMAKE_CONFIGURATION_TYPES Debug Release RelWithDebInfo CACHE STRING "Configuration types." FORCE)
set( CMAKE_CXX_FLAGS "${SciDaP_CXX_FLAGS}" CACHE STRING "Flags used by the CXX compiler during all build types." FORCE )
set( CMAKE_CXX_FLAGS_DEBUG "${SciDaP_CXX_FLAGS_DEBUG}" CACHE STRING "Flags used by the CXX compiler during DEBUG builds." FORCE )
set( CMAKE_CXX_FLAGS_RELEASE "${SciDaP_CXX_FLAGS_RELEASE}" CACHE STRING "Flags used by the CXX compiler during RELEASE builds." FORCE )
set( CMAKE_CXX_FLAGS_RELWITHDEBINFO "${SciDaP_CXX_FLAGS_RELWITHDEBINFO}" CACHE STRING "Flags used by the CXX compiler during RELWITHDEBINFO builds." FORCE )
