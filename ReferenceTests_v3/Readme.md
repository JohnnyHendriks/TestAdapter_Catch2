# Reference tests for the Test Adapter for Catch2

For the development of the **Test Adapter for Catch2** some Catch2 tests were created. Some of these tests are actually used in the unit tests for the different components of the **Test Adapter for Catch2**. Others are just used the manually check how the test adapter handles them. So expect many of the tests to be failing tests.

**Warning: the written tests do not necessarily follow best practices.**

## Build tests

These tests were adapted from the previous set to make use of Catch2 v3. A script is provided that downloads the appropriate catch2 v3 versions and builds them. A separate script is used that generates and builds the tests, and copies the generated executables to the appropriate place for use by the unittests for the different components of the **Test Adapter for Catch2**.
