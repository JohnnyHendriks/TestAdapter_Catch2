# Helper functions to setup TAFC2 unit tests

include( GNUInstallDirs )

function( tafc2_config_output_paths )

  get_property( isMultiConfig GLOBAL PROPERTY GENERATOR_IS_MULTI_CONFIG )

  if( NOT CMAKE_RUNTIME_OUTPUT_DIRECTORY )
    set( CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR} CACHE PATH "Output path for executables" FORCE )
  endif()

  if( NOT CMAKE_LIBRARY_OUTPUT_DIRECTORY )
    set( CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_LIBDIR} CACHE PATH "Output path for dynamic libraries" FORCE )
  endif()

  if( NOT CMAKE_ARCHIVE_OUTPUT_DIRECTORY )
    set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_LIBDIR} CACHE PATH "Output path for static libraries" FORCE )
  endif()

  mark_as_advanced(
    CMAKE_RUNTIME_OUTPUT_DIRECTORY
    CMAKE_LIBRARY_OUTPUT_DIRECTORY
    CMAKE_ARCHIVE_OUTPUT_DIRECTORY
  )

  if( isMultiConfig )

    foreach( configtype IN LISTS CMAKE_CONFIGURATION_TYPES )

      string( TOUPPER ${configtype} namepostfix )

      if( NOT CMAKE_RUNTIME_OUTPUT_DIRECTORY_${namepostfix} )
        set( CMAKE_RUNTIME_OUTPUT_DIRECTORY_${namepostfix} ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/${configtype} CACHE PATH "Output path for ${configtype} executables" FORCE )
      endif()
      if( NOT CMAKE_LIBRARY_OUTPUT_DIRECTORY_${namepostfix} )
        set( CMAKE_LIBRARY_OUTPUT_DIRECTORY_${namepostfix} ${CMAKE_LIBRARY_OUTPUT_DIRECTORY}/${configtype} CACHE PATH "Output path for ${configtype} dynamic libraries" FORCE )
      endif()
      if( NOT CMAKE_ARCHIVE_OUTPUT_DIRECTORY_${namepostfix} )
        set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY_${namepostfix} ${CMAKE_ARCHIVE_OUTPUT_DIRECTORY}/${configtype} CACHE PATH "Output path for ${configtype} static libraries" FORCE )
      endif()

      mark_as_advanced(
        CMAKE_RUNTIME_OUTPUT_DIRECTORY_${namepostfix}
        CMAKE_LIBRARY_OUTPUT_DIRECTORY_${namepostfix}
        CMAKE_ARCHIVE_OUTPUT_DIRECTORY_${namepostfix}
      )

    endforeach()

  endif()

endfunction()

function( tafc2_config_catch2_packages versions )

  foreach( tafc2_catchversion IN ITEMS ${versions} )

    message( DEBUG "TAFC2: tafc2_catchversion: ${tafc2_catchversion}")
    list( APPEND CMAKE_PREFIX_PATH "${CMAKE_SOURCE_DIR}/src/cmake-package/${tafc2_catchversion}" )
    message( DEBUG "TAFC2: CMAKE_PREFIX_PATH: ${CMAKE_PREFIX_PATH}")

    string( REPLACE "." "_" project_versionname ${tafc2_catchversion} )
    find_package( "Tafc2-Catch2_${project_versionname}" )

  endforeach()

endfunction()

function( tafc2_add_test targetname idefolder )

  # Add test executable
  add_executable( ${targetname} )

  # Add dependencies
  target_link_libraries(
    ${targetname}
    PRIVATE
      TAFC2_Common
      $<$<CONFIG:Debug>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Release>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Rel3_0_1>:Tafc2::Catch2_v3_0_1>
      $<$<CONFIG:Rel3_1_0>:Tafc2::Catch2_v3_1_0>
      $<$<CONFIG:Rel3_1_1>:Tafc2::Catch2_v3_1_1>
      $<$<CONFIG:Rel3_2_0>:Tafc2::Catch2_v3_2_0>
      $<$<CONFIG:Rel3_2_1>:Tafc2::Catch2_v3_2_1>
      $<$<CONFIG:Rel3_3_0>:Tafc2::Catch2_v3_3_0>
      $<$<CONFIG:Rel3_3_1>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Rel3_3_2>:Tafc2::Catch2_v3_3_2>
      $<$<CONFIG:Rel3_4_0>:Tafc2::Catch2_v3_4_0>
  )

  # Group files for IDE environments
  source_group( Files REGULAR_EXPRESSION "(\\.[ch]pp)$" )
  source_group( Files\\Main REGULAR_EXPRESSION "(main\\.cpp)$" )

  set_target_properties( ${targetname} PROPERTIES FOLDER "${idefolder}" )

  # Add test
  add_test(
    NAME ${targetname}
    COMMAND $<TARGET_FILE_NAME:${targetname}>
    WORKING_DIRECTORY $<TARGET_FILE_DIR:${targetname}>
  )

endfunction()

function( tafc2_add_dlltest targetname idefolder )

  # Add test executable
  add_library( ${targetname} SHARED )

  # Add dependencies
  target_link_libraries(
    ${targetname}
    PRIVATE
      TAFC2_Dll_Common
      $<$<CONFIG:Debug>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Release>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Rel3_0_1>:Tafc2::Catch2_v3_0_1>
      $<$<CONFIG:Rel3_1_0>:Tafc2::Catch2_v3_1_0>
      $<$<CONFIG:Rel3_1_1>:Tafc2::Catch2_v3_1_1>
      $<$<CONFIG:Rel3_2_0>:Tafc2::Catch2_v3_2_0>
      $<$<CONFIG:Rel3_2_1>:Tafc2::Catch2_v3_2_1>
      $<$<CONFIG:Rel3_3_0>:Tafc2::Catch2_v3_3_0>
      $<$<CONFIG:Rel3_3_1>:Tafc2::Catch2_v3_3_1>
      $<$<CONFIG:Rel3_3_2>:Tafc2::Catch2_v3_3_2>
      $<$<CONFIG:Rel3_4_0>:Tafc2::Catch2_v3_4_0>
  )

  # Group files for IDE environments
  source_group( Files REGULAR_EXPRESSION "(\\.[ch]pp)$" )
  source_group( Files\\Main REGULAR_EXPRESSION "(main\\.cpp)$" )

  set_target_properties( ${targetname} PROPERTIES FOLDER "${idefolder}" )

  # Add test
#  add_test(
#    NAME ${targetname}
#    COMMAND $<TARGET_FILE_NAME:${targetname}>
#    WORKING_DIRECTORY $<TARGET_FILE_DIR:${targetname}>
#  )

endfunction()

function( tafc2_add_runner targetname idefolder )

  # Add test executable
  add_executable( ${targetname} )

  # Group files for IDE environments
  source_group( Files REGULAR_EXPRESSION "(\\.[ch]pp)$" )

  set_target_properties( ${targetname} PROPERTIES FOLDER "${idefolder}" )

endfunction()