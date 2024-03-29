# Version file for Catch2 3.0.1 library
set(PACKAGE_VERSION "3.0.1")

if(PACKAGE_FIND_VERSION STREQUAL PACKAGE_VERSION)
  set(PACKAGE_VERSION_EXACT TRUE)
endif()

if(PACKAGE_FIND_VERSION MATCHES "^(3\\.0\\.1)")
  set(PACKAGE_VERSION_COMPATIBLE TRUE)
else()
  set(PACKAGE_VERSION_COMPATIBLE FALSE)
endif()
