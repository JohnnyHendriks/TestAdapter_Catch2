cmake_minimum_required( VERSION 3.16 )

set( targetname Catch_Discover )
set( targetnamedll CatchDll_Discover )

# Define source files
set( src_Files
  UT_LongTestCaseNames.cpp
  UT_NotDefaultDiscoverable.cpp
  UT_Tags.cpp
  UT_TestCases.cpp
)

#########################
# Default Console Width #
#########################

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

######################
# Console Width = 60 #
######################

# Add test executable
tafc2_add_test( ${targetname}_60 "Functionality" )

target_compile_definitions( ${targetname}_60 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=60 )

# Add sources
target_sources(
  ${targetname}_60
  PRIVATE ${src_Files}
)

# Add test dll
tafc2_add_dlltest( ${targetnamedll}_60 "Dll" )

target_compile_definitions( ${targetnamedll}_60 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=60 )

# Add sources
target_sources(
  ${targetnamedll}_60
  PRIVATE ${src_Files}
)

#######################
# Console Width = 120 #
#######################

# Add test executable
tafc2_add_test( ${targetname}_120 "Functionality" )

target_compile_definitions( ${targetname}_120 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=120 )

# Add sources
target_sources(
  ${targetname}_120
  PRIVATE ${src_Files}
)

# Add test dll
tafc2_add_dlltest( ${targetnamedll}_120 "Dll" )

target_compile_definitions( ${targetnamedll}_120 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=120 )

# Add sources
target_sources(
  ${targetnamedll}_120
  PRIVATE ${src_Files}
)

#######################
# Console Width = 255 #
#######################

# Add test executable
tafc2_add_test( ${targetname}_255 "Functionality" )

target_compile_definitions( ${targetname}_255 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=255 )

# Add sources
target_sources(
  ${targetname}_255
  PRIVATE ${src_Files}
)

# Add test dll
tafc2_add_dlltest( ${targetnamedll}_255 "Dll" )

target_compile_definitions( ${targetnamedll}_255 PRIVATE CATCH_CONFIG_CONSOLE_WIDTH=255 )

# Add sources
target_sources(
  ${targetnamedll}_255
  PRIVATE ${src_Files}
)
