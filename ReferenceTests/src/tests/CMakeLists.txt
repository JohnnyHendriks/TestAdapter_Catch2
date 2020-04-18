cmake_minimum_required(VERSION 3.16)

set( CXX_EXTENSIONS OFF )

add_subdirectory(Catch_Common)

add_subdirectory(Catch_Discover)
add_subdirectory(Catch_Dummy)
add_subdirectory(Catch_Duplicates)
add_subdirectory(Catch_Execution)
add_subdirectory(Catch_Hidden)
add_subdirectory(Catch_NoSEH)

add_subdirectory(Catch_Testset01)
add_subdirectory(Catch_Testset02)
add_subdirectory(Catch_Testset03)
