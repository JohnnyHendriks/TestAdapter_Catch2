cmake_minimum_required( VERSION 3.16 )

set( targetname TAFC2_Common )

add_library( ${targetname} INTERFACE )

# Add default source files
set( src_Catch2
  "${CMAKE_CURRENT_LIST_DIR}/main.cpp"
)

target_sources( ${targetname} INTERFACE ${src_Catch2} )

target_include_directories( ${targetname} INTERFACE "${CMAKE_CURRENT_LIST_DIR}" )
