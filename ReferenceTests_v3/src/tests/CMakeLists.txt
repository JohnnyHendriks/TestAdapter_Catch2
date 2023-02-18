cmake_minimum_required(VERSION 3.16)

set( CXX_EXTENSIONS OFF )

foreach( tafc2_catchversion IN ITEMS v3.0.1 v3.1.0 v3.1.1 v3.2.0 v3.2.1 v3.3.0 v3.3.1 )

  message( DEBUG "TAFC2: tafc2_catchversion: ${tafc2_catchversion}")
  list( APPEND CMAKE_PREFIX_PATH "${CMAKE_SOURCE_DIR}/cmake-package/${tafc2_catchversion}" )

endforeach()

foreach( tafc2_catchversion IN ITEMS v3_0_1 v3_1_0 v3_1_1 v3_2_0 v3_2_1 v3_3_0 v3_3_1 )

  message( DEBUG "TAFC2: tafc2_catchversion: ${tafc2_catchversion}")
  find_package( "Tafc2-Catch2_${tafc2_catchversion}" )

endforeach()

add_subdirectory(Catch_Common)

add_subdirectory(Catch_Discover)
add_subdirectory(Catch_Dummy)
add_subdirectory(Catch_Duplicates)
add_subdirectory(Catch_Environment)
add_subdirectory(Catch_Execution)
add_subdirectory(Catch_Hidden)
add_subdirectory(Catch_NoSEH)

add_subdirectory(Catch_Testset01)
add_subdirectory(Catch_Testset02)
add_subdirectory(Catch_Testset03)
