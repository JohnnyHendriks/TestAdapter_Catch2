# Helper functions to setup catch2 v3 for use with TAFC2 unit tests

function( tafc2_dowload_catch2 zipfilename url hashvalue )

  if( NOT EXISTS "${CMAKE_BINARY_DIR}/../repos/${zipfilename}" )

    message(STATUS "TAFC2: Downloading ${zipfilename} from ${url}")
    file(
      DOWNLOAD
        ${url}
        "${CMAKE_BINARY_DIR}/../repos/${zipfilename}"
      EXPECTED_HASH SHA256=${hashvalue}
    )

  endif()

endfunction()

function( tafc2_prepare_catch2 zipfilename url hashvalue )

  tafc2_dowload_catch2("${zipfilename}.zip" ${url} ${hashvalue})

  # Extract archive
  message(STATUS "TAFC2: Extracting ${zipfilename} from ${CMAKE_BINARY_DIR}/../repos/")
  file( ARCHIVE_EXTRACT INPUT "${CMAKE_BINARY_DIR}/../repos/${zipfilename}.zip")

  # Copy CMakePresets.json
  file( COPY_FILE "${CMAKE_SOURCE_DIR}/cmake/CMakePresets.json" "${CMAKE_BINARY_DIR}/${zipfilename}/CMakePresets.json" )

endfunction()
