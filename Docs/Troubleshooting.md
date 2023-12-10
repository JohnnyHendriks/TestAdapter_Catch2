# Troubleshooting guide for Test Adapter for Catch2

All is well when everything is working as advertised. The fun stops when things don't work as expected. Here you can find a collection of hints and tips to help you diagnose and potentially fix the troubles you may encounter when using the **Test Adapter for Catch2**. This guide assumes you want to use the **Test Adapter for Catch2** to discover your Catch2 tests for use with Visual Studio's Test Explorer.

- [My Catch2 tests are not showing up in the Test Explorer window, what can I do to resolve the problem?](#my-catch2-tests-are-not-showing-up-in-the-test-explorer-window-what-can-i-do-to-resolve-the-problem)
- [I added new test cases, but they don't show up in the Test Explorer, is there anything that can be done?](#i-added-new-test_cases-but-they-dont-show-up-in-the-test-explorer-is-there-anything-that-can-be-done)
- [Some of my Catch2 tests are not showing up in the Test Explorer window, what can I do to resolve the problem?](#some-of-my-catch2-tests-are-not-showing-up-in-the-test-explorer-window-what-can-i-do-to-resolve-the-problem)

---

## Diagnosing issues with test discovery

### My Catch2 tests are not showing up in the Test Explorer window, what can I do to resolve the problem?

#### Sanity check-list

- [ ] Is the **Test Adapter for Catch2** installed and enabled?
- [ ] Is a `.runsettings`-file configured?
- [ ] Does the active `.runsettings`-file have the appropriate `<Catch2Adapter>`-configuration?
- [ ] Check Test Explorer log and see if the **Test Adapter for Catch2** is actually called and/or if it encountered a problem.

See [Walkthrough]() if you need help with any items of this sanity check-list.

#### Increase logging

Increasing the logging level for the **Test Adapter for Catch2** to `Verbose` may give you some more insight into what is going wrong. If that fails you can also set the logging to `Debug`, though be warned this potentially generates a lot of logging noise.

### I added new test cases, but they don't show up in the Test Explorer, is there anything that can be done?

This problem typically occurs when your tests are located in a DLL and you access them via a runner executable. Starting with version 2.0.0 of the **Test Adapter for Catch2** it has become possible to perform test discovery directly on the DLL (_.i.e._, from the point of view of the Test Explorer). The alternative workaround is to switch configurations and start a build. Another option is to first clean your test-executable projects and then build them (choosing re-build will typically not work, as that won't remove the tests from the Test Explorer).

### Some of my Catch2 tests are not showing up in the Test Explorer window, what can I do to resolve the problem?

This will typically be caused by the used test case name. _E.g._, if the test case name contains matching opening and closing brackets ("()" or "{}") the Visual Studio Test Explorer may refuse to show that test case.

To test this theory change the name to something very simple, if the test case then shows up the test case name was the problem and you should think about adjusting the test case name.
