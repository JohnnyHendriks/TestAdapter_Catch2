cmake_minimum_required( VERSION 3.21 )

set_property( GLOBAL PROPERTY USE_FOLDERS ON )
enable_testing()

project( "Catch2Builder"
         VERSION 1.0.0
         LANGUAGES NONE
)

list( APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/cmake" )

include( tafc2-catch2builder-helpers )

#tafc2_config_output_paths()

######################################
# Download Catch2 versions if needed
######################################

tafc2_prepare_catch2(
  "Catch2-3.0.1"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.0.1.zip"
  "809789B8FD1083CEE29BAACD3B6DA32C93D18A069EDF714C704811A4EAD67472"
)

tafc2_prepare_catch2(
  "Catch2-3.1.0"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.1.0.zip"
  "7219C2CA75A6B2A157B1B162E4AD819FB32585995CAC32542A4F72D950DD96F7"
)

tafc2_prepare_catch2(
  "Catch2-3.1.1"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.1.1.zip"
  "EEC6C327CD9187C63BBAAA8486F715E31544000BF8876C0543E1181A2A52A5DE"
)

tafc2_prepare_catch2(
  "Catch2-3.2.0"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.2.0.zip"
  "B9F3887915F32EB732140AF6A153065A11FABCD3F3E9355F3ABFF3D3618FD0FE"
)

tafc2_prepare_catch2(
  "Catch2-3.2.1"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.2.1.zip"
  "BFEE681EAA920C6DDBE05C1EEF1912440D38C5F9A7924F68A6AA219ED1A39C0F"
)

tafc2_prepare_catch2(
  "Catch2-3.3.0"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.3.0.zip"
  "4B75388E5A0DD45AB86AB75EE9BE0208A825B9C1650591BA41AD9D2E0CCE6DAE"
)

tafc2_prepare_catch2(
  "Catch2-3.3.1"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.3.1.zip"
  "5E5283BF93B2693F6877BBA3EAA76D66588955374D0CEC5B40117066C044AD5E"
)

tafc2_prepare_catch2(
  "Catch2-3.3.2"
  "https://github.com/catchorg/Catch2/archive/refs/tags/v3.3.2.zip"
  "8361907F4D9BFF3AE7C1EDB027F813659F793053C99B67837A0C0375F065BAE2"
)