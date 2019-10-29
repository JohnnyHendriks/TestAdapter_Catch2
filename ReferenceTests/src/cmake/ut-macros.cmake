# Static lib unit tests setup

include( "path-functions" )

macro( add_test_ut projectname idefolder )

  add_generic_ut( ${projectname} "tests" ${idefolder} ${ARGN} )

endmacro()

macro( add_generic_ut projectname rootname idefolder )

  # Process arguments
  set( options )
  set( args )
  set( list_args SRC MAIN )

  cmake_parse_arguments(
    add_generic_ut
    "${options}"
    "${args}"
    "${list_args}"
    ${ARGN}
  )

  # Checks
  if( NOT DEFINED add_generic_ut_SRC )
    message( FATAL_ERROR "missing SRC parameter" )
    return()
  endif()

  foreach( arg IN LISTS add_generic_ut_UNPARSED_ARGUMENTS )
    message(AUTHOR_WARNING "unparsed argument: ${arg}" )
  endforeach()

  # Relative directories
  set( rel_include_path )
  get_relative_path_to(
    SRC ${PROJECT_SOURCE_DIR}
    DIR ${rootname}
    OUT_RELPATH rel_include_path
  )

  set( rel_include_path_catch2 )
  get_relative_path_to_catch2(
    SRC ${PROJECT_SOURCE_DIR}
    OUT_RELPATH rel_include_path_catch2
  )

  if( NOT DEFINED add_generic_ut_MAIN )
    set( src_Catch2
      "${rel_include_path_catch2}/catch.hpp"
      "${rel_include_path}/${rootname}/catch_discover.hpp"
      "${rel_include_path}/${rootname}/main.cpp"
    )
  else()
    set( src_Catch2
      "${rel_include_path_catch2}/catch.hpp"
      ${add_generic_ut_MAIN}
    )
  endif()
  
  # Add test executable
  add_executable( ${projectname} ${add_generic_ut_SRC} ${src_Catch2} )

  # Add include directories
  target_include_directories( ${projectname} PRIVATE ${rel_include_path} ${rel_include_path_catch2} )

  # Select Catch2 version
  target_compile_definitions(
    ${projectname}
    PRIVATE
    $<$<CONFIG:Debug>:TA_CATCH2_V2_10_2>
    $<$<CONFIG:Release>:TA_CATCH2_V2_10_2>
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
  )

  # Group source files for IDE environments
  source_group( Files\\Catch2 FILES ${src_Catch2} )

  set_target_properties( ${projectname} PROPERTIES FOLDER "${idefolder}" )
  
  # Add test
  add_test(
    NAME ${projectname}
    COMMAND ${projectname}
    WORKING_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/../_unittests/staticlibs/$<$<CONFIG:Debug>:Debug>$<$<CONFIG:Release>:Release>"
  )

endmacro()