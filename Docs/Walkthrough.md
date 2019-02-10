# Walkthough: using the Test Adapter for Catch2

## Introduction

It can be a bit tricky to get the **Test Adapter for Catch2** running. So, if you are having trouble getting the test adapter to work you are not alone. For this Walkthrough I'm using the latest version of Microsoft Visual Studio Community 2017 (version 15.9.6 at the time of writing). This walkthrough makes use of the v1.5.0 release of the **Test Adapter for Catch2**.

I will use the ReferenceTests created for testing the **Test Adapter for Catch2**. You can find the Visual Studio solution in the [ReferenceTests](../ReferenceTests/) folder of this GitHub repository. I assume you know how to open the Test Explorer Window.

The following topics are discussed.
- [Make sure the test adapter is installed](#make-sure-the-test-adapter-is-installed)
- [Select a runsettings file](#select-a-runsettings-file)
- [Trigger test discovery](#trigger-test-discovery)
  - [Solving problems with discovery](#solving-problems-with-discovery)
- [Running tests](#running-tests)
- [Jump to TEST_CASE in source](#jump-to-test_case-in-source)
- [Examples of test case detail views](#examples-of-test-case-detail-views)

## Make sure the test adapter is installed

There are several ways to install the **Test Adapter for Catch2**, but the most typical way is using the `Extensions and Updates` dialog in Visual Studio. Installing via this route typically schedules the extension for installation after you shutdown Visual Studio. It is possible you missed the VSIX installer dialog that then automatically pops up, effectively resulting in the extension not being installed. I have seen this happen more than once, so don't feel bad if it happens to you.

To make sure the test adapter is installed, open the `Extensions and Updates` dialog in Visual Studio and search for it in your installed extensions list.

![Extensions and Updates dialog](Images/Walkthrough-01.png)

> For information on installing and managing extensions see [Microsoft Docs: Find and use Visual Studio extensions](https://docs.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions)

## Select a _.runsettings_ file

Out of the box the test adapter does not work. This is by design. Visual Studio provides a list of all the executables in your solution to the test adapter. As part of the discovery process the executables are called. A worst-case scenario would be that you have a project in your solution for an executable that when executed formats your C-drive. Note, the **Test Adapter for Catch2** does not discover Catch2 tests inside dll-files. 

After you have opened the [ReferenceTests.sln](../ReferenceTests/) in Visual Studio and made sure the Test Explorer window is open, the first thing to do is to select a test settings file.

![Select Test Settings File](Images/Walkthrough-02.png)

For this walkthrough we will use the `Minimal.runsettings` and `ReferenceTests.runsettings` that you can find in the [ReferenceTests](../ReferenceTests/) folder of this GitHub repository. Note that use is made of custom discovery in the `ReferenceTests.runsettings` file.

![Select Test Settings File](Images/Walkthrough-03.png)

You can load multiple _.runsettings_ files and switch between them. The one with a checkmark next to it is the one that is being used. Also, clicking on a _.runsettings_ file in the `Test Settings` sub-menu that has a checkmark next to it, will deselect it.

![Select Test Settings File](Images/Walkthrough-17.png)

## Trigger test discovery

This is where it can get a bit annoying. Changing or setting a _.runsettings_ file will not trigger test discovery. Typically building one of the test projects will trigger discovery. You can of course also build the entire solution. Naturally, if all the projects were already build test discovery is not triggered. In that case you either need to reload the solution or rebuild one of the test projects. This is of course a major annoyance. Here having a refresh button in the Test Explorer to trigger discovery would be really helpful. If you agree there is a [UserVoice: Easy means of refreshing tests in Test explorer](https://visualstudio.uservoice.com/forums/121579-visual-studio-ide/suggestions/17035426-easy-means-of-refreshing-tests-in-test-explorer) feature request that can use your vote.

If all went well the tests will appear in the test explorer. In the output window you should see output similar to the following. Make sure you selected `Show output from: Tests`.
```
[10/02/2019 21:11:43 Informational] ------ Discover test started ------
[10/02/2019 21:11:44 Informational] Started Catch2Adapter test discovery...
[10/02/2019 21:11:44 Informational] Discover log:
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Discover.exe
  Testcase count: 26
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Dummy.exe
  Testcase count: 1
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Duplicates.exe
  Error Occurred (exit code 255):
error: TEST_CASE( "SameTestNames. Duplicate" ) already defined.
	First seen at d:\github\testadapter_catch2\referencetests\src\catch2\catch_duplicates\ut_sametestnames.cpp(28)
	Redefined at d:\github\testadapter_catch2\referencetests\src\catch2\catch_duplicates\ut_sametestnames.cpp(42)
  Testcase count: 0
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Hidden.exe
  Testcase count: 6
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_NoSEH.exe
  Testcase count: 3
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset01.exe
  Testcase count: 6
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset02.exe
  Testcase count: 45
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset03.exe
  Testcase count: 44

[10/02/2019 21:11:44 Informational] Finished Catch2Adapter test discovery.
[10/02/2019 21:11:44 Informational] ========== Discover test finished: 131 found (0:00:01,7058933) ==========
```

This is the output using the verbose logging setting. Note that the `Catch_Duplicates.exe` contains tests with duplicate names, which is why those tests do not appear in the Test Explorer. With logging set to normal the ouput would look something like the following.
```
[10/02/2019 21:15:50 Informational] ------ Discover test started ------
[10/02/2019 21:15:51 Informational] Started Catch2Adapter test discovery...
[10/02/2019 21:15:52 Informational] Discover log:
  Error Occurred (exit code 255):
error: TEST_CASE( "SameTestNames. Duplicate" ) already defined.
	First seen at d:\github\testadapter_catch2\referencetests\src\catch2\catch_duplicates\ut_sametestnames.cpp(28)
	Redefined at d:\github\testadapter_catch2\referencetests\src\catch2\catch_duplicates\ut_sametestnames.cpp(42)

[10/02/2019 21:15:52 Informational] Finished Catch2Adapter test discovery.
[10/02/2019 21:15:52 Informational] ========== Discover test finished: 131 found (0:00:01,7030783) ==========
```

In case there are no errors the normal logging output would have looked something like the following.
```
[10/02/2019 21:17:16 Informational] ------ Discover test started ------
[10/02/2019 21:17:17 Informational] Started Catch2Adapter test discovery...
[10/02/2019 21:17:17 Informational] Finished Catch2Adapter test discovery.
[10/02/2019 21:17:17 Informational] ========== Discover test finished: 131 found (0:00:01,5681499) ==========
```

### Solving problems with discovery

There are many ways discovery can go wrong or provide you with unexpected results. Above you have already seen one way for the test project that contained test cases with duplicate names. But there are more ways, here are a few of the common ones.

- In case you do not have a _.runsettings_ file selected you may get output similar to the following.
```
[10/02/2019 21:18:43 Informational] ------ Discover test started ------
[10/02/2019 21:18:44 Error] Catch2 Test Adapter Settings not found, Catch2 test discovery is cancelled. Add Catch2Adapter settings to runsettings-file.
[10/02/2019 21:18:44 Informational] ========== Discover test finished: 0 found (0:00:01,1664477) ==========
```

- In case the **Test Adapter for Catch2** is disabled you may get output similar to the following.
```
[10/02/2019 21:20:53 Informational] ------ Discover test started ------
[10/02/2019 21:20:54 Informational] Catch2Adapter is disabled.
[10/02/2019 21:20:54 Informational] ========== Discover test finished: 0 found (0:00:01,1415663) ==========
```

- In case you provided an invalid `<DiscoverCommandLine>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#discovercommandline) on how to resolve this problem.
```
[10/02/2019 21:26:36 Informational] ------ Discover test started ------
[10/02/2019 21:26:37 Error] Catch2 Test Adapter Settings contain an invalid discovery commandline. Catch2 test discovery is cancelled. Add Valid DiscoverCommandLine to Catch2Adapter Settings in runsettings-file.
[10/02/2019 21:26:37 Informational] ========== Discover test finished: 0 found (0:00:01,1516882) ==========
```

- In case you did not provide the `<FilenameFilter>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#filenamefilter) on how to resolve this problem.
```
[10/02/2019 21:28:20 Informational] ------ Discover test started ------
[10/02/2019 21:28:21 Error] Catch2 Test Adapter Settings contains an empty filename filter, Catch2 test discovery is cancelled. Add a valid FilenameFilter to Catch2Adapter Settings in runsettings-file.
[10/02/2019 21:28:21 Informational] ========== Discover test finished: 0 found (0:00:01,1263644) ==========
```

- In some cases, it is possible that test case discovery fails every now and then causing previously discovered tests to disappear from the Test Explorer. This may occur if you set the `<DiscoverTimeout>` option to a relatively short time. In this case increasing this timeout may solve your problems. See explanation of this [Setting](Settings.md#discovertimeout) for more information. Of course, setting this timeout to a very small value may also result in test case discovery to always fail. To help with this problem output similar to the following is produced when a timeout occurs.
```
[10/02/2019 21:31:06 Informational] ------ Discover test started ------
[10/02/2019 21:31:07 Informational] Started Catch2Adapter test discovery...
[10/02/2019 21:31:07 Informational] Discover log:
  Warning: Discovery timeout for D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Dummy.exe

[10/02/2019 21:31:07 Informational] Finished Catch2Adapter test discovery.
[10/02/2019 21:31:07 Informational] ========== Discover test finished: 0 found (0:00:01,5329554) ==========
```

- In some cases, other test adapters may interfere with Catch2 test discovery. In those cases, look in the `Tests` output for hints. Worst case you may have to disable the test adapter that is causing trouble. For instance, if the output includes `Could not locate debug symbols`, that is probably output from the Boost Test Adapter, which can be disabled from the Tools...Extensions and Updates... menu item. By the way, this is the reason a feature was added to the **Test Adapter for Catch2** to disable it via the _.runsettings_ file. So in case the reverse happens and the **Test Adapter for Catch2** interferes with another test adapter, you have an easy way to disable the **Test Adapter for Catch2** via the _.runsettings_ file.

## Running tests

You can of course run all tests using the `Run All` button in the Test Explorer. However, this may trigger a full solution build, which is not always something you may want to occur. My preference is to select the tests I want to run and use the context menu to run the selected tests.

![Run selected tests](Images/Walkthrough-04.png) ![Run selected tests](Images/Walkthrough-05.png)

Similarly, you can debug selected tests, though there you would typically only select a single test case to debug. You can set your own break points and/or enable the [DebugBreak feature](Settings.md#debugbreak) to help with debugging your code.

## Test case timeout

The [ReferenceTests.sln](../ReferenceTests/) contains a test ('Catch_Dummy') that will run forever. If you run this test using the `Minimal.runsettings` test settings the test will run until you press the Cancel button in the Test Explorer.

![Test case run forever](Images/Walkthrough-18.png)

If you run this test using the `ReferenceTests.runsettings` test settings the test will be cancelled automatically after 20 seconds. The length of this timeout is set via the [`<TestCaseTimeout>`](Settings.md#testcasetimeout) setting in the _.runsettings_ file.

![Test case timeout](Images/Walkthrough-19.png)

Note, the Test Explorer also has the option to set a timeout, however that one is a timeout for the test session, not for individual tests. For more information on the test session timeout see [Configure unit tests by using a _.runsettings_ file](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file) on [Visual Studio Docs](https://docs.microsoft.com/en-us/visualstudio/).

## Examples of test case detail views

To finish this walkthrough some screenshots of the detailed view for various test cases. Note the assertion statistics are displayed as part of the message. This is done for both Failed and Successful tests.

### Link to source

![Test case source link](Images/Walkthrough-06.png)

The details view will typically provide a link to the source of the test case in the detailed view of the test case. Clicking the link will bring focus to the source file and the place the cursor on the line of the TEST_CASE. This link is available regardless of whether the test was run or not, as such it can be used to quickly navigate to a specific test case.

### Successful test

![Testset03.Tests01. 01p Basic](Images/Walkthrough-08.png)

### Failed test

Note the 2 StackTrace items at the bottom. They provide information about the failure, and also act as links to the source of the failure. Note that only the assertion statistics are displayed as part of the message. Additional information can be reached via the Output link just above the StackTrace section. If you prefer to have the additional information be part of the message adjust the [StackTraceFormat option](Settings.md#stacktraceformat) in the _.runsettings_ file.

![Testset03.Tests01. 01f Basic; detailed view](Images/Walkthrough-09.png)

![Testset03.Tests01. 01f Basic; output](Images/Walkthrough-10.png)

### Failed test with a failure inside a nested SECTION

Note, only the additional information provides information about sections and info messages that may help diagnose the reason for the failure.

![Testset03.Tests01. 03f-2 Nested Sections; detailed view](Images/Walkthrough-11.png)

![Testset03.Tests01. 03f-2 Nested Sections; output](Images/Walkthrough-12.png)

### Successful test with std::cout and std::cerr output

Note, in case test failures occur any additional information will be prepended to the Standard Output section.

![Testset03.Tests07. cerr & cout passing tests; detailed view](Images/Walkthrough-13.png)

![Testset03.Tests07. cerr & cout passing tests; output](Images/Walkthrough-14.png)

### Successful test with warnings

Note, some additional information, with regard to the number of warnings generated, is added to the message. See the additional information for the actual warnings.

![Testset03.Tests02. 04p Nested Sections WARN; detailed view](Images/Walkthrough-15.png)

![Testset03.Tests07. 04p Nested Sections WARN; output](Images/Walkthrough-16.png)
