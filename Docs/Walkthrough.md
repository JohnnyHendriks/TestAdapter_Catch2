# Walkthough: using the Test Adapter for Catch2

## Introduction

It can be a bit tricky to get the **Test Adapter for Catch2** running. So if you are having trouble getting the test adapter to work you are not alone. For this Walkthrough I'm using the latest version of Microsoft Visual Studio Community 2017 (version 15.7.5 at the time of writing). This walkthrough makes use of the v1.1.0 release of the **Test Adapter for Catch2**.

I will use the ReferenceTests created for testing the **Test Adapter for Catch2**. You can find the Visual Studio solution in the [ReferenceTests](../ReferenceTests/Readme.md) folder of this GitHub repository. I assume you know how to open the Test Explorer Window.

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

## Select a runsettings file

Out of the box the test adapter does not work. This is by design. Visual Studio provides a list of all the executables in your solution to the test adapter. As part of the discovery process these executables are called. A worst-case scenario would be that you have a project in your solution for an executable that when executed formats your C-drive.

After you have opened the [ReferenceTests.sln](../ReferenceTests/Readme.md) in Visual Studio and made sure the Test Explorer window is open, the first thing to do is to select a test settings file.

![Select Test Settings File](Images/Walkthrough-02.png)

For this walkthrough we will use the `ReferenceTests.runsettings` that you can find in the [ReferenceTests](../ReferenceTests/Readme.md) folder of this GitHub repository.

![Select Test Settings File](Images/Walkthrough-03.png)

Note you can load multiple _.runsettings_ files and switch between them. The one with a checkmark next to it is the one that is being used. Also, clicking on a _.runsettings_ file in the `Test Settings` sub-menu that has a checkmark next to it, will deselect it.

## Trigger test discovery

This is where it can get a bit annoying. Changing or setting a _.runsettings_ file will not trigger test discovery. Typically building one of the test projects will trigger discovery. You can of course also build the entire solution. Of course, if all the project were already build test discovery is not triggered. In that case you either need to reload the solution or rebuild one of the test projects. This is of course a major annoyance. Here having a refresh button in the Test Explorer to trigger discovery would be really helpful. If you agree there is a [UserVoice: Easy means of refreshing tests in Test explorer](https://visualstudio.uservoice.com/forums/121579-visual-studio-ide/suggestions/17035426-easy-means-of-refreshing-tests-in-test-explorer) feature request that can use your vote.

If all went well the tests will appear in the test explorer. In the output window you should see output similar to the following. Make sure you selected `Show output from: Tests`.
```
[14/07/2018 08:53:54 Informational] ------ Discover test started ------
[14/07/2018 08:53:56 Informational] Started Catch2Adapter test discovery...
[14/07/2018 08:53:56 Informational] Discover log:
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Dummy.exe
  Testcase count: 1
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_NoSEH.exe
  Testcase count: 3
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset01.exe
  Testcase count: 2
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset02.exe
  Testcase count: 22
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset03.exe
  Testcase count: 27
Source: D:\GitHub\TestAdapter_Catch2\ReferenceTests\_unittest64\Release\Catch_Testset04.exe
  Error Occurred (exit code 255):
error: TEST_CASE( "Testset04.Tests01. Duplicate" ) already defined.
	First seen at d:\github\testadapter_catch2\referencetests\src\catch2\catch_testset04\ut_tests01.cpp(28)
	Redefined at d:\github\testadapter_catch2\referencetests\src\catch2\catch_testset04\ut_tests01.cpp(42)
  Testcase count: 0

[14/07/2018 08:53:56 Informational] Finished Catch2Adapter test discovery.
[14/07/2018 08:53:56 Informational] ========== Discover test finished: 55 found (0:00:02,8514214) ==========
```

This is the output using the verbose logging setting. Note that the `Catch_Testset04.exe` contains tests with duplicate names, which is why those tests do not appear in the Test Explorer. With logging set to normal the ouput would look something like the following.
```
[14/07/2018 09:10:56 Informational] ------ Discover test started ------
[14/07/2018 09:10:57 Informational] Started Catch2Adapter test discovery...
[14/07/2018 09:10:57 Informational] Discover log:
  Error Occurred (exit code 255):
error: TEST_CASE( "Testset04.Tests01. Duplicate" ) already defined.
	First seen at d:\github\testadapter_catch2\referencetests\src\catch2\catch_testset04\ut_tests01.cpp(28)
	Redefined at d:\github\testadapter_catch2\referencetests\src\catch2\catch_testset04\ut_tests01.cpp(42)

[14/07/2018 09:10:57 Informational] Finished Catch2Adapter test discovery.
[14/07/2018 09:10:57 Informational] ========== Discover test finished: 55 found (0:00:01,5883654) ==========
```

In case there are no errors the normal logging output would have looked something like the following.
```
[14/07/2018 09:13:56 Informational] ------ Discover test started ------
[14/07/2018 09:13:57 Informational] Started Catch2Adapter test discovery...
[14/07/2018 09:13:57 Informational] Finished Catch2Adapter test discovery.
[14/07/2018 09:13:57 Informational] ========== Discover test finished: 55 found (0:00:01,4620216) ==========
```

### Solving problems with discovery

There are many ways discovery can go wrong or provide you with unexpected results. Above you have already seen one way for the test project that contained test cases with duplicate names. But there are more ways, here are a few of the common ones.

In case you do not have a _.runsettings_ file selected you may get output similar to the following.
```
[14/07/2018 09:44:09 Informational] ------ Discover test started ------
[14/07/2018 09:44:09 Warning] Test run will use DLL(s) built for framework Framework45 and platform X86. Following DLL(s) will not be part of run: 
Catch_Dummy.exe, Catch_NoSEH.exe, Catch_Testset01.exe, Catch_Testset02.exe, Catch_Testset03.exe are built for Framework None and Platform X64.
 Go to http://go.microsoft.com/fwlink/?LinkID=236877&clcid=0x409 for more details on managing these settings.
[14/07/2018 09:44:10 Error] Catch2 Test Adapter Settings not found, Catch2 test discovery is cancelled. Add Catch2Adapter settings to runsettings-file.
[14/07/2018 09:44:10 Informational] ========== Discover test finished: 0 found (0:00:01,2495178) ==========
```

In case the **Test Adapter for Catch2** is disabled you may get output similar to the following.
```
[14/07/2018 09:53:41 Informational] ------ Discover test started ------
[14/07/2018 09:53:42 Informational] Catch2Adapter is disabled.
[14/07/2018 09:53:42 Informational] ========== Discover test finished: 0 found (0:00:01,2415869) ==========
```

In case you did not provide the `<DiscoverCommandLine>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#discovercommandline) on how to resolve this problem.
```
[14/07/2018 10:23:31 Informational] ------ Discover test started ------
[14/07/2018 10:23:33 Error] Catch2 Test Adapter Settings contain an invalid discovery commandline. Catch2 test discovery is cancelled. Add Valid DiscoverCommandLine to Catch2Adapter Settings in runsettings-file.
[14/07/2018 10:23:33 Informational] ========== Discover test finished: 0 found (0:00:01,2390112) ==========
```

In case you did not provide the `<FilenameFilter>` option in the _.runsettings_ file you may get output similar to the following. See explanation of this [Setting](Settings.md#filenamefilter) on how to resolve this problem.
```
[14/07/2018 10:27:07 Informational] ------ Discover test started ------
[14/07/2018 10:27:08 Error] Catch2 Test Adapter Settings contains an empty filename filter, Catch2 test discovery is cancelled. Add a valid FilenameFilter to Catch2Adapter Settings in runsettings-file.
[14/07/2018 10:27:08 Informational] ========== Discover test finished: 0 found (0:00:01,3262645) ==========
```

In some cases, other test adapters may interfere with Catch2 test discovery. In those cases, look in the `Tests` output for hints. Worst case you may have to disable the test adapter that is causing trouble. By the way, this is the reason a feature was added to the **Test Adapter for Catch2** to disable it via the _.runsettings_ file. So in case the reverse happens and the **Test Adapter for Catch2** interferes with another test adapter, you have an easy way to disable the **Test Adapter for Catch2** via the _.runsettings_ file.

In some cases, it is possible that test case discovery fails every now and then causing previously discovered tests to disappear from the Test Explorer. This may occur if you set the `<DiscoverTimeout>` option to a relatively short time. In this case increasing this timeout may solve your problems. See explanation of this [Setting](Settings.md#discovertimeout) for more information. Of course, setting this timeout to a very small value may also result in test case discovery to always fail.

## Running tests

You can of course run all tests using the `Run All` button in the Test Explorer. However, this may trigger a full solution build, which is not always something you may want to occur. My preference is to select the tests I want to run and use the context menu to run the selected tests.

![Run selected tests](Images/Walkthrough-04.png) ![Run selected tests](Images/Walkthrough-05.png)

Similarly, you can debug selected tests, though there you would typically only select a single test case to debug. You can set your own break points and/or enable the [DebugBreak feature](Settings.md#debugbreak) to help with debugging your code.

## Jump to TEST_CASE in source

In case you enabled [custom test discovery](Settings.md#discovercommandline), you will be provided with a link to the source of the test case in the detailed view of the test case (see left image). Otherwise the source link will not be available (see right image)

![Test case source link](Images/Walkthrough-06.png) ![Test case source link](Images/Walkthrough-07.png)

Clicking the link will bring focus to the source file and the place the cursor on the line of the TEST_CASE.

## Examples of test case detail views

To finish this walkthrough some screenshots of the detailed view for various test cases. Note the assertion statistics are displayed as part of the message. This is done for both Failed and Successful tests.

- A successful test.

![Testset03.Tests01. 01p Basic](Images/Walkthrough-08.png)

- A failed test. Note the 2 StackTrace items at the bottom. They provide information about on which line had a failure. This information is also part of the Message. Currently the StackTrace is not turned into a link by the Test Explorer. See description of the [StackTraceFormat option](Settings.md#stacktraceformat) for more information.

![Testset03.Tests01. 01p Basic](Images/Walkthrough-09.png)

- A failed test with a failure inside a nested SECTION.

![Testset03.Tests01. 01p Basic](Images/Walkthrough-10.png)

- A successful test with warnings.

![Testset03.Tests01. 01p Basic](Images/Walkthrough-11.png)
