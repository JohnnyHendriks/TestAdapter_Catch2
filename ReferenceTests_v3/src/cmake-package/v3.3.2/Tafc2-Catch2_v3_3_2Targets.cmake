get_property( isMultiConfig GLOBAL PROPERTY GENERATOR_IS_MULTI_CONFIG )

# create absolute path prefix for package
set(TAFC2_CATCH2_V3_3_2_PREFIX_DIR "${CMAKE_CURRENT_LIST_DIR}/../../../build/catch2/Rel3_3_2/")
cmake_path(
  ABSOLUTE_PATH TAFC2_CATCH2_V3_3_2_PREFIX_DIR
  NORMALIZE
  OUTPUT_VARIABLE TAFC2_CATCH2_V3_3_2_PREFIX_DIR
)
message( DEBUG "TAFC2targets: TAFC2_CATCH2_V3_3_2_PREFIX_DIR was set to: ${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}")

####################
# Static Libraries #
####################

add_library( Tafc2::Catch2_v3_3_2 STATIC IMPORTED )
add_library( Tafc2::Catch2_v3_3_2_Main STATIC IMPORTED )

target_include_directories( Tafc2::Catch2_v3_3_2
  INTERFACE "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}include"
)

target_link_libraries( Tafc2::Catch2_v3_3_2 INTERFACE Tafc2::Catch2_v3_3_2_Main )

# Configuration
if( isMultiConfig )

  foreach( configtype IN LISTS CMAKE_CONFIGURATION_TYPES)

    string( TOUPPER ${configtype} namepostfix )

    if( namepostfix STREQUAL "DEBUG")

      set_property( TARGET Tafc2::Catch2_v3_3_2 APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_2
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/Debug/Catch2d.lib"
      )
      set_property( TARGET Tafc2::Catch2_v3_3_2_Main APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_2_Main
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/Debug/Catch2Maind.lib"
      )

    else()

      set_property( TARGET Tafc2::Catch2_v3_3_2 APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_2
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/Release/Catch2.lib"
      )
      set_property( TARGET Tafc2::Catch2_v3_3_2_Main APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_2_Main
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/Release/Catch2Main.lib"
      )

    endif()

  endforeach()

else()

  set_target_properties( Tafc2::Catch2_v3_3_2
    PROPERTIES
      IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
      IMPORTED_LOCATION "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/${CMAKE_BUILD_TYPE}/Catch2.lib"
  )

  set_target_properties( Tafc2::Catch2_v3_3_2_Main
    PROPERTIES
      IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
      IMPORTED_LOCATION "${TAFC2_CATCH2_V3_3_2_PREFIX_DIR}lib/${CMAKE_BUILD_TYPE}/Catch2Main.lib"
  )

endif()
