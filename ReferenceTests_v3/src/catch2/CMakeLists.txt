cmake_minimum_required( VERSION 3.16 )

set( targetname TAFC2_Catch2 )

add_library( ${targetname} INTERFACE )

target_include_directories( ${targetname} INTERFACE "${CMAKE_CURRENT_LIST_DIR}" )

target_compile_definitions(
  ${targetname}
  INTERFACE
    $<$<CONFIG:Debug>:TA_CATCH2_V3_0_0>
    $<$<CONFIG:Release>:TA_CATCH2_V3_0_0>
    $<$<CONFIG:Rel_0_0>:TA_CATCH2_V3_0_0>
)
