get_property( isMultiConfig GLOBAL PROPERTY GENERATOR_IS_MULTI_CONFIG )

####################
# Static Libraries #
####################

add_library( Tafc2::Catch2_v3_3_1 STATIC IMPORTED )
add_library( Tafc2::Catch2_v3_3_1_Main STATIC IMPORTED )

target_include_directories( Tafc2::Catch2_v3_3_1
  INTERFACE "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}include"
)

target_link_libraries( Tafc2::Catch2_v3_3_1 INTERFACE Tafc2::Catch2_v3_3_1_Main )

# Configuration
if( isMultiConfig )

  foreach( configtype IN LISTS CMAKE_CONFIGURATION_TYPES)

    string( TOUPPER ${configtype} namepostfix )

    if( namepostfix STREQUAL "DEBUG")

      set_property( TARGET Tafc2::Catch2_v3_3_1 APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_1
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/Debug/Catch2d.lib"
      )
      set_property( TARGET Tafc2::Catch2_v3_3_1_Main APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_1_Main
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/Debug/Catch2Maind.lib"
      )

    else()

      set_property( TARGET Tafc2::Catch2_v3_3_1 APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_1
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/Release/Catch2.lib"
      )
      set_property( TARGET Tafc2::Catch2_v3_3_1_Main APPEND PROPERTY IMPORTED_CONFIGURATIONS ${configtype} )
      set_target_properties( Tafc2::Catch2_v3_3_1_Main
        PROPERTIES
          IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
          IMPORTED_LOCATION_${namepostfix} "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/Release/Catch2Main.lib"
      )

    endif()

  endforeach()

else()

  set_target_properties( Tafc2::Catch2_v3_3_1
    PROPERTIES
      IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
      IMPORTED_LOCATION "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/${CMAKE_BUILD_TYPE}/Catch2.lib"
  )

  set_target_properties( Tafc2::Catch2_v3_3_1_Main
    PROPERTIES
      IMPORTED_LINK_INTERFACE_LANGUAGES_${namepostfix} "CXX"
      IMPORTED_LOCATION "${TAFC2_CATCH2_V3_3_1_PREFIX_DIR}lib/${CMAKE_BUILD_TYPE}/Catch2Main.lib"
  )

endif()
