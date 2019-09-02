cmake_minimum_required(VERSION 3.14)

set( CXX_EXTENSIONS OFF )

project(ut-testadapter VERSION 1.0.0)

include("ut-macros")

add_subdirectory(Catch_Discover)
add_subdirectory(Catch_Dummy)
add_subdirectory(Catch_Duplicates)
add_subdirectory(Catch_Hidden)
add_subdirectory(Catch_NoSEH)

add_subdirectory(Catch_Testset01)
add_subdirectory(Catch_Testset02)
add_subdirectory(Catch_Testset03)
