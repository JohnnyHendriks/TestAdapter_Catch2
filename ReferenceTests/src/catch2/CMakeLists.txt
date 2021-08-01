cmake_minimum_required( VERSION 3.16 )

set( targetname TAFC2_Catch2 )

add_library( ${targetname} INTERFACE )

target_include_directories( ${targetname} INTERFACE "${CMAKE_CURRENT_LIST_DIR}" )

target_compile_definitions(
  ${targetname}
  INTERFACE
    $<$<CONFIG:Debug>:TA_CATCH2_V2_11_3>
    $<$<CONFIG:Release>:TA_CATCH2_V2_11_3>
    $<$<CONFIG:Rel_0_1>:TA_CATCH2_V2_0_1>
    $<$<CONFIG:Rel_1_0>:TA_CATCH2_V2_1_0>
    $<$<CONFIG:Rel_1_1>:TA_CATCH2_V2_1_1>
    $<$<CONFIG:Rel_1_2>:TA_CATCH2_V2_1_2>
    $<$<CONFIG:Rel_2_0>:TA_CATCH2_V2_2_0>
    $<$<CONFIG:Rel_2_1>:TA_CATCH2_V2_2_1>
    $<$<CONFIG:Rel_2_2>:TA_CATCH2_V2_2_2>
    $<$<CONFIG:Rel_2_3>:TA_CATCH2_V2_2_3>
    $<$<CONFIG:Rel_3_0>:TA_CATCH2_V2_3_0>
    $<$<CONFIG:Rel_4_0>:TA_CATCH2_V2_4_0>
    $<$<CONFIG:Rel_4_1>:TA_CATCH2_V2_4_1>
    $<$<CONFIG:Rel_4_2>:TA_CATCH2_V2_4_2>
    $<$<CONFIG:Rel_5_0>:TA_CATCH2_V2_5_0>
    $<$<CONFIG:Rel_6_0>:TA_CATCH2_V2_6_0>
    $<$<CONFIG:Rel_6_1>:TA_CATCH2_V2_6_1>
    $<$<CONFIG:Rel_7_0>:TA_CATCH2_V2_7_0>
    $<$<CONFIG:Rel_7_1>:TA_CATCH2_V2_7_1>
    $<$<CONFIG:Rel_7_2>:TA_CATCH2_V2_7_2>
    $<$<CONFIG:Rel_8_0>:TA_CATCH2_V2_8_0>
    $<$<CONFIG:Rel_9_0>:TA_CATCH2_V2_9_0>
    $<$<CONFIG:Rel_9_1>:TA_CATCH2_V2_9_1>
    $<$<CONFIG:Rel_9_2>:TA_CATCH2_V2_9_2>
    $<$<CONFIG:Rel_10_0>:TA_CATCH2_V2_10_0>
    $<$<CONFIG:Rel_10_1>:TA_CATCH2_V2_10_1>
    $<$<CONFIG:Rel_10_2>:TA_CATCH2_V2_10_2>
    $<$<CONFIG:Rel_11_0>:TA_CATCH2_V2_11_0>
    $<$<CONFIG:Rel_11_1>:TA_CATCH2_V2_11_1>
    $<$<CONFIG:Rel_11_2>:TA_CATCH2_V2_11_2>
    $<$<CONFIG:Rel_11_3>:TA_CATCH2_V2_11_3>
    $<$<CONFIG:Rel_12_0>:TA_CATCH2_V2_12_0>
    $<$<CONFIG:Rel_12_1>:TA_CATCH2_V2_12_1>
    $<$<CONFIG:Rel_12_2>:TA_CATCH2_V2_12_2>
    $<$<CONFIG:Rel_12_3>:TA_CATCH2_V2_12_3>
    $<$<CONFIG:Rel_12_4>:TA_CATCH2_V2_12_4>
    $<$<CONFIG:Rel_13_0>:TA_CATCH2_V2_13_0>
    $<$<CONFIG:Rel_13_1>:TA_CATCH2_V2_13_1>
    $<$<CONFIG:Rel_13_2>:TA_CATCH2_V2_13_2>
    $<$<CONFIG:Rel_13_3>:TA_CATCH2_V2_13_3>
    $<$<CONFIG:Rel_13_4>:TA_CATCH2_V2_13_4>
    $<$<CONFIG:Rel_13_5>:TA_CATCH2_V2_13_5>
    $<$<CONFIG:Rel_13_6>:TA_CATCH2_V2_13_6>
  )
