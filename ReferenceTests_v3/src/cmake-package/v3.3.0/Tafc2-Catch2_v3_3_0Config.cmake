####################
# Config Package for Tafc2-Catch2v3_3_0
####################

cmake_minimum_required(VERSION 3.20)

# create absolute path prefix for package
set(TAFC2_CATCH2_V3_3_0_PREFIX_DIR "${CMAKE_CURRENT_LIST_DIR}/../../../build/catch2/Rel3_3_0/")
cmake_path(
  ABSOLUTE_PATH TAFC2_CATCH2_V3_3_0_PREFIX_DIR
  NORMALIZE
  OUTPUT_VARIABLE TAFC2_CATCH2_V3_3_0_PREFIX_DIR
)
message( DEBUG "TAFC2: TAFC2_CATCH2_V3_3_0_PREFIX_DIR was set to: ${TAFC2_CATCH2_V3_3_0_PREFIX_DIR}")

if(NOT TARGET Tafc2::Catch2_v3_3_0)

  include(${CMAKE_CURRENT_LIST_DIR}/Tafc2-Catch2_v3_3_0Targets.cmake)

endif()
