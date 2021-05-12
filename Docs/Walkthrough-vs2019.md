# Walkthough: using the Test Adapter for Catch2

## Introduction

It can be a bit tricky to get the **Test Adapter for Catch2** running. So, if you are having trouble getting the test adapter to work you are not alone. For this Walkthrough I'm using the latest version of Microsoft Visual Studio Community 2019 (version 16.9 at the time of writing). This walkthrough makes use of the v1.6.0 release of the **Test Adapter for Catch2**. Note, that in Visual Studio 2019 the Test Explorer has undergone a facelift and things work slightly different. There is an alternate [walkthrough for use with Visual Studio 2017](Walkthrough-vs2017.md).

I will use the ReferenceTests created for testing the **Test Adapter for Catch2**. You can find a Visual Studio solution in the [ReferenceTests](../ReferenceTests/walkthrough/) folder of this GitHub repository. I will assume you know how to open the Test Explorer Window.

The following topics are discussed.
- [Make sure the test adapter is installed](#make-sure-the-test-adapter-is-installed)
- [Select a runsettings file](#select-a-runsettings-file)
- [Trigger test discovery](#trigger-test-discovery)
  - [Solving problems with discovery](#solving-problems-with-discovery)
- [Running tests](#running-tests)
- [Jump to TEST_CASE in source](#jump-to-test_case-in-source)
- [Examples of test case detail views](#examples-of-test-case-detail-views)

## Make sure the test adapter is installed

There are several ways to install the **Test Adapter for Catch2**, but the most typical way is using the `Manage Extensions` dialog in Visual Studio. Installing via this route typically schedules the extension for installation after you shutdown Visual Studio. It is possible you missed the VSIX installer dialog that then automatically pops up, effectively resulting in the extension not being installed. I have seen this happen more than once, so don't feel bad if it happens to you.

To make sure the test adapter is installed, open the `Manage Extensions` dialog in Visual Studio and search for it in your installed extensions list.

![Manage Extensions dialog](Images/walkthrough-vs2019/Walkthrough-01.png)

> For information on installing and managing extensions see [Microsoft Docs: Find and use Visual Studio extensions](https://docs.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions?view=vs-2019)

## Select a _.runsettings_ file

Out of the box the test adapter does not work. This is by design. Visual Studio provides a list of all the executables in your solution to the test adapter. As part of the discovery process these executables are called. A worst-case scenario would be that you have a project in your solution for an executable that when executed formats your C-drive. Note, the **Test Adapter for Catch2** does not discover Catch2 tests inside dll-files.

After you have opened the [ReferenceTests_VS2019.sln](../ReferenceTests/walkthrough) in Visual Studio 2019 and made sure the Test Explorer window is open, the first thing to do is to configure the run settings.

![Select Run Settings File](Images/walkthrough-vs2019/Walkthrough-02.png)

For this walkthrough we will use the `Minimal.runsettings`, `ReferenceTests.runsettings` and `ReferenceTestsCombined.runsettings` that you can find in the [ReferenceTests walkthrough](../ReferenceTests/walkthrough) folder of this GitHub repository. Note that use is made of custom discovery in the `ReferenceTests.runsettings` and `ReferenceTestsCombined.runsettings` file.

![Select Run Settings File](Images/walkthrough-vs2019/Walkthrough-03.png)

You can load multiple _.runsettings_ files and switch between them. The one with a checkmark next to it is the one that is being used. Note that clicking on a _.runsettings_ file in the `Configure Run Settings` sub-menu that has a checkmark next to it, will deselect it.

![Select Run Settings File](Images/walkthrough-vs2019/Walkthrough-04.png)

## Trigger test discovery

### First build

When you have selected the `Minimal.runsettings` and then build the solution you will get output similar to the following.

```
---------- Starting test discovery ----------
Started Catch2Adapter test discovery...
Discover log:
  Error Occurred (exit code 255):
error: TEST_CASE( "SameTestNames. Duplicate" ) already defined.
  First seen at D:\GitHub\TestAdapter_Catch2\ReferenceTests\src\tests\Catch_Duplicates\UT_SameTestNames.cpp(28)
  Redefined at D:\GitHub\TestAdapter_Catch2\ReferenceTests\src\tests\Catch_Duplicates\UT_SameTestNames.cpp(42)

Finished Catch2Adapter test discovery.
========== Test discovery finished: 131 Tests found in 1.5 sec ==========
```

Note that the `Catch_Duplicates.exe` contains tests with duplicate names, which is why those tests do not appear in the Test Explorer. The errors resulting from this are also logged. In case there are no errors the normal logging output would have looked something like the following.

```
---------- Starting test discovery ----------
Started Catch2Adapter test discovery...
Finished Catch2Adapter test discovery.
========== Test discovery finished: 131 Tests found in 1.5 sec ==========
```

### Subsequent builds

This is where it can get a bit annoying. Changing or setting a _.runsettings_ file will not trigger test discovery. Due to the way the Test Explorer caches information, even building one of the test projects or reloading a solution/project will not always trigger discovery when test have already been previously discovered. If you added new tests and they don't show up after the build, then there seem to be two workarounds that work. You can first switch the solution configuration (_e.g._, from `Debug` to `Release`) and then trigger a build. Or, you can clean the test executable (its test cases should disappear from the Test Explorer) and then build it again. This is of course a major annoyance. Here having a refresh button in the Test Explorer to trigger discovery would be really helpful.

### Solving problems with discovery

There are many ways discovery can go wrong or provide you with unexpected results. Above you have already seen one way for the test project that contained test cases with duplicate names. But there are more ways, here are a few of the common ones.

- In case you do not have a _.runsettings_ file selected you may get output similar to the following.
```
---------- Starting test discovery ----------
Catch2 Test Adapter Settings not found, Catch2 test discovery is cancelled. Add Catch2Adapter settings to runsettings-file.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Dummy.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Duplicates.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Hidden.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_NoSEH.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset01.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset02.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset03.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1 sec ==========
```

- In case the **Test Adapter for Catch2** is disabled you may get output similar to the following.
```
---------- Starting test discovery ----------
Catch2Adapter is disabled.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Dummy.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Duplicates.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Hidden.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_NoSEH.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset01.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset02.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset03.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1.1 sec ==========
```

- In case you provided an invalid `<DiscoverCommandLine>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#discovercommandline) on how to resolve this problem.
```
---------- Starting test discovery ----------
Catch2 Test Adapter Settings contain an invalid discovery commandline. Catch2 test discovery is cancelled. Add Valid DiscoverCommandLine to Catch2Adapter Settings in runsettings-file.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Dummy.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Duplicates.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Hidden.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_NoSEH.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset01.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset02.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset03.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1.2 sec ==========
```

Alternatively if you made a typo you may get output similar to the following.

```
---------- Starting test discovery ----------
Started Catch2Adapter test discovery...
Discover log:
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe
  Error Occurred (exit code 255):

Error(s) in input:
  Unrecognised token: --disc

Run with -? for usage

  Testcase count: 0

Finished Catch2Adapter test discovery.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1 sec ==========
```


- In case you did not provide the `<FilenameFilter>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#filenamefilter) on how to resolve this problem.
```
---------- Starting test discovery ----------
Catch2 Test Adapter Settings contains an empty filename filter, Catch2 test discovery is cancelled. Add a valid FilenameFilter to Catch2Adapter Settings in runsettings-file.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Dummy.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Duplicates.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Hidden.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_NoSEH.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset01.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset02.exe D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset03.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1.3 sec ==========
```

- In some cases, it is possible that test case discovery fails every now and then causing previously discovered tests to disappear from the Test Explorer. This may occur if you set the `<DiscoverTimeout>` option to a relatively short time. In this case increasing this timeout may solve your problems. See explanation of this [Setting](Settings.md#discovertimeout) for more information. Of course, setting this timeout to a very small value may also result in test case discovery to always fail. To help with this problem output similar to the following is produced when a timeout occurs.
```
---------- Starting test discovery ----------
Started Catch2Adapter test discovery...
Discover log:
  Warning: Discovery timeout for D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe

Finished Catch2Adapter test discovery.
No test is available in D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.
========== Test discovery finished: 0 Tests found in 1.6 sec ==========
```

- In some cases, other test adapters may interfere with Catch2 test discovery. In those cases, look in the `Tests` output for hints. Worst case you may have to disable the test adapter that is causing trouble. For instance, if the output includes `Could not locate debug symbols`, that is probably output from the Boost Test Adapter, which can be disabled from the \[Extensions]->\[Manage Extensions] menu item. By the way, this is the reason a feature was added to the **Test Adapter for Catch2** to disable it via the _.runsettings_ file. So in case the reverse happens and the **Test Adapter for Catch2** interferes with another test adapter, you have an easy way to disable the **Test Adapter for Catch2** via the _.runsettings_ file.

### Logging level

By default Logging level is set to `Normal`. However, if you need more information you can change this setting to get more information. See explanation of this [Setting](Settings.md#logging) to see all available log levels. Below example output for discovery when Logging is set to `Verbose`

```
---------- Starting test discovery ----------
Started Catch2Adapter test discovery...
Discover log:
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Discover.exe
  Testcase count: 26
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Dummy.exe
  Testcase count: 1
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Hidden.exe
  Testcase count: 6
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_NoSEH.exe
  Testcase count: 3
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset01.exe
  Testcase count: 6
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset02.exe
  Testcase count: 45
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\walkthrough\_unittest64\VS2019\Release\Catch_Testset03.exe
  Testcase count: 44

Finished Catch2Adapter test discovery.
========== Test discovery finished: 131 Tests found in 1.7 sec ==========
```

## Running tests

You can of course run all tests using the `Run All Tests In View`-button in the Test Explorer. However, this may trigger a full solution build, which is not always something you may want to occur. My preference is to select the tests I want to run and use the `Run`-button to run the selected tests.

![Run selected tests](Images/walkthrough-vs2019/Walkthrough-05.png) ![Run selected tests](Images/walkthrough-vs2019/Walkthrough-06.png)

## Debugging tests

You have several option to start a debug session for a test. You can just to it the standard way, and set the project for the test executable as the default project. Configure the appropriate commandline and then start a debug session. But you can also start a debug session directly from the Test Explorer. Select the tests you want to debug (though typically you would select only one), and either use the right-click context menu, or the drop-down next to the `Run`-button. When you start a debug session via the Test Explorer you can set your own break points and/or enable the [DebugBreak feature](Settings.md#debugbreak) to help with debugging your code.

## Test case timeout

The [ReferenceTests.sln](../ReferenceTests/) contains a test ('Catch_Dummy') that will run forever. If you run this test using the `Minimal.runsettings` test settings the test will run until you press the Cancel button in the Test Explorer.

![Test case run forever](Images/walkthrough-vs2019/Walkthrough-07.png)

If you run this test using the `ReferenceTests.runsettings` test settings the test will be cancelled automatically after 20 seconds. The length of this timeout is set via the [`<TestCaseTimeout>`](Settings.md#testcasetimeout) setting in the _.runsettings_ file.

![Test case timeout](Images/walkthrough-vs2019/Walkthrough-08.png)

Note, the Test Explorer also has the option to set a timeout, however that one is a timeout for the test session, not for individual tests. For more information on the test session timeout see [Configure unit tests by using a _.runsettings_ file](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file) on [Visual Studio Docs](https://docs.microsoft.com/en-us/visualstudio/).

## Execution Mode

By default each test case is run in a separate instantiation of the test executable, _i.e._ the default execution mode is `Single`. Starting in version 1.6.0 of the **Test Adapter for Catch2** it is also possible to execute multiple test cases in a single instantiation of the test executable, _i.e._, the `Combined` execution mode was added. The advantage of this is that the test results can be obtained faster as the overhead of instantiating the test executable is minimized. The execution mode can be set via the [`<ExecutionMode>`](Settings.md#executionmode) setting in the _.runsettings_ file.

If you run the tests using the `ReferenceTestsCombined.runsettings` test settings you should be able to notice the difference compared to running the tests with the `ReferenceTests.runsettings` test settings.

### Forcing Single Execution mode

The nature of some tests may require them to be preferably executed in the `Single` execution mode. _E.g._, a test may be slow, or potentially run forever. To enable this you can use the [`<ExecutionModeForceSingleTagRgx>`](Settings.md#executionmodeforcesingletagrgx) setting in the _.runsettings_ file to configure Catch2 Tag-names that signal a test should always be run in `Single` execution mode.

### Timeouts

The different execution modes require different timeout logic. _E.g._, you may want tests that are run together in the `Combined`execution mode to timeout after 10s (_i.e._, all tests should be finished within 10s). However, tests that are run in `Single` execution mode may only timeout after 30s, because they are tests that are known to take a long time for example. You can set the different timeout settings using the [`<CombinedTimeout>`](Settings.md#combinedtimeout) and [`<TestCaseTimeout>`](Settings.md#testcasetimeout) setting in the _.runsettings_ file.

## Examples of test case detail views

To finish this walkthrough some screenshots of the detailed view for various test cases. Note the assertion statistics are displayed as part of the message only when the test was run in the `Single` execution mode. This is done for both Failed and Successful tests.

### Link to source

![Test case source link](Images/walkthrough-vs2019/Walkthrough-09.png)

The details view will typically provide a link to the source of the test case in the detailed view of the test case. Clicking the link will bring focus to the source file and place the cursor on the line of the TEST_CASE. This link is available regardless of whether the test was run or not, as such it can be used to quickly navigate to a specific test case.

### Successful test

![Testset03.Tests01. 01p Basic](Images/walkthrough-vs2019/Walkthrough-10.png)

### Failed test

Note the 2 StackTrace items at the bottom. They provide information about the failure, and also act as links to the source of the failure. Note that only the assertion statistics are displayed as part of the message. Additional information can be reached via the `Open additional output for this result` link below the StackTrace section. If you prefer to have the additional information be part of the message adjust the [StackTraceFormat option](Settings.md#stacktraceformat) in the _.runsettings_ file.

![Testset03.Tests01. 01f Basic; detailed view](Images/walkthrough-vs2019/Walkthrough-11.png)

![Testset03.Tests01. 01f Basic; output](Images/walkthrough-vs2019/Walkthrough-12.png)

### Failed test with a failure inside a nested SECTION

Note, only the additional information provides information about sections and info messages that may help diagnose the reason for the failure. In this example the root section is named "`CHECK REQUIRE`" and the nested section with the failed test assertion is named "`CHECK_FALSE REQUIRE_FALSE`".

![Testset03.Tests01. 03f-2 Nested Sections; detailed view](Images/walkthrough-vs2019/Walkthrough-13.png)

![Testset03.Tests01. 03f-2 Nested Sections; output](Images/walkthrough-vs2019/Walkthrough-14.png)

### Successful test with std::cout and std::cerr output

Note, in case test failures occur any additional information will be prepended to the Standard Output section.

![Testset03.Tests07. cerr & cout passing tests; detailed view](Images/walkthrough-vs2019/Walkthrough-15.png)

![Testset03.Tests07. cerr & cout passing tests; output](Images/walkthrough-vs2019/Walkthrough-16.png)

### Successful test with warnings

Note, some additional information, with regard to the number of warnings generated, is added to the message. See the additional information for the actual warnings.

![Testset03.Tests02. 04p Nested Sections WARN; detailed view](Images/walkthrough-vs2019/Walkthrough-17.png)

![Testset03.Tests07. 04p Nested Sections WARN; output](Images/walkthrough-vs2019/Walkthrough-18.png)
