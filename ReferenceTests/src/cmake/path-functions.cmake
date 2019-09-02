# Find path

# This function is intended to generate a relative path
# to the location of a provided directory name
# Requires the following named arguments
#   SRC: The path to extract a relative path from
#   DIR: The name of the directory to find the relative path to
#   OUT_RELPATH: The variable to set the constructed relative path in
function( get_relative_path_to )
  # Process arguments
  set(options)
  set(args SRC DIR OUT_RELPATH)
  set(list_args)

  cmake_parse_arguments(
    PARSE_ARGV 0
    local
    "${options}"
    "${args}"
    "${list_args}"
  )

  # Checks
  if( NOT DEFINED local_SRC )
    message( FATAL_ERROR "missing SRC parameter" )
    return()
  endif()

  if( NOT DEFINED local_DIR )
    message( FATAL_ERROR "missing DIR parameter" )
    return()
  endif()

  if( NOT DEFINED local_OUT_RELPATH )
    message( FATAL_ERROR "missing OUT_RELPATH parameter" )
    return()
  endif()

  foreach( arg IN LISTS local_UNPARSED_ARGUMENTS )
    message(AUTHOR_WARNING "unparsed argument: ${arg}" )
  endforeach()

  # process path
  set(relpath)

  # Special case, search directory is current directory
  string( REGEX MATCH "/${local_DIR}$" relpath "${local_SRC}")

  # Check if special case failed. If so use other algorithm
  if( relpath STREQUAL "" )
    set( idx )
    string( FIND "${local_SRC}" "/${local_DIR}/" idx REVERSE )
    if( ${idx} EQUAL -1 )
      set( "${local_OUT_RELPATH}" ":NOTFOUND:" PARENT_SCOPE)
      return()
    endif()
    string( SUBSTRING "${local_SRC}" ${idx} -1 relpath)
  endif()

  # Remove starting slash
  string( SUBSTRING "${relpath}" 1 -1 relpath)

  # Replace Directories with ..
  string( REGEX REPLACE "[^/]+" "\.\." relpath ${relpath})

  # Set return value
  set( "${local_OUT_RELPATH}" "${relpath}" PARENT_SCOPE)
endfunction()

# This function is intended to generate a relative path
# to the location of a provided directory name
# Requires the following named arguments
#   SRC: The path to extract a relative path from
#   OUT_RELPATH: The variable to set the constructed relative path in
function( get_relative_path_to_catch2 )
  # Process arguments
  set(options)
  set(args SRC OUT_RELPATH)
  set(list_args)

  cmake_parse_arguments(
    PARSE_ARGV 0
    local
    "${options}"
    "${args}"
    "${list_args}"
  )

  # Checks
  if( NOT DEFINED local_SRC )
    message( FATAL_ERROR "missing SRC parameter" )
    return()
  endif()

  if( NOT DEFINED local_OUT_RELPATH )
    message( FATAL_ERROR "missing OUT_RELPATH parameter" )
    return()
  endif()

  foreach( arg IN LISTS local_UNPARSED_ARGUMENTS )
    message(AUTHOR_WARNING "unparsed argument: ${arg}" )
  endforeach()

  # First try
  set( local_rel_path_catch2 )
  get_relative_path_to(
    SRC ${local_SRC}
    DIR "tests"
    OUT_RELPATH local_rel_path_catch2
  )

  if( EXISTS "${local_SRC}/${local_rel_path_catch2}/catch2/catch.hpp" )
    # Used in SciDaP-Core
    set( local_rel_path_catch2 "${local_rel_path_catch2}/catch2" )
  else()
    message( FATAL_ERROR "Cannot find path to Catch2-framework." )
  endif()

  # Set return value
  set( "${local_OUT_RELPATH}" "${local_rel_path_catch2}" PARENT_SCOPE)

endfunction()
