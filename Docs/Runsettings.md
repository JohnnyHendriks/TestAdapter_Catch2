# Configure runsettings

Visual Studio's Test Explorer configuration works through runsettings files. What follows is a collection of tips & tricks for configuring runsettings-files for various use cases.

## Visual Studio specific

It is possible to configure Visual Strudio to automatically discover a runsettings files. To that end a file named `.runsettings` should be located next to the solution file of the Visual Studio solution. Of course this requires the `Auto detect runsettings files`-feature to be turned on. If you are making use of CMake (or equivalent tool), consider to auto generate an appropriate `.runsettings`-file and place it next to the generated solution-file. Of course it is still possible to select specific runsettings files as well.

## Test Adapter for Catch2 specific

Depending on your specific use case there are several options to consider. Starting with version 2.0.0 of the **Test Adapter for Catch2** you have the option to link Catch2 test executanbles and/or test dlls to the Test Explorer. Before it was only possible to link test executables. Here we go a little deeper into specific scenarios.

- [Catch2 tests are located in an executable](#catch2-tests-are-located-in-an-executable)
- [Catch2 tests are located in a dll](#catch2-tests-are-located-in-a-dll)
- [Catch2 tests are located both in an executable and in a dll](#catch2-tests-are-located-both-in-an-executable-and-in-a-dll)
- [Run specific tests in a single process](#run-specific-tests-in-a-single-process)
- [Use different timeout settings for specific test sources](#use-different-timeout-settings-for-specific-test-sources)

---

### Catch2 tests are located in an executable

In this scenario it is required to configure the [`<FilenameFilter>`](./Settings.md#filenamefilter)-setting.

#### Example of a minimal runsettings file

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
  </Catch2Adapter>

</RunSettings>
```

---

### Catch2 tests are located in a dll

Before version 2.0.0 of the **Test Adapter for Catch2** you would configure the [`<FilenameFilter>`](./Settings.md#filenamefilter)-setting to point at the specific runner executable for the dll. Though this worked it did have an annoying drawback. As the runner executable typically does not change, the Test Explorer is under the impression no changes to the tests have been made, and then refuses to re-discover tests. The only workaround to my knowledge is to switch configurations (_e.g._, from `Debug` to `Release`) and and trigger a build.

Starting version 2.0.0 of the **Test Adapter for Catch2** it is possible to configure the [`<DllFilenameFilter>`](./Settings.md#dllfilenamefilter)-setting instead. In this case the dll is linked to the Test Explorer, and as a result any changes to the dll will result the Test Explorer to re-discover tests. The **Test Adapter for Catch2** does however need to be informed on which runner to use to run the tests, as such you also need to configure the [`<DllRunner>`](Settings.md#dllrunner)-setting.

The following runner scenarios are covered

- A single runner for all test dll-files (requires configuration of the [`<DllRunnerCommandline>`](Settings.md#dllrunnercommandline)-setting)
- A specific runner for each dll-file
- Use of debug-postfix for debug builds (requires configuration of the [`<DllPostfix>`](Settings.md#dllpostfix)-setting)

#### Example of a minimal runsettings file - specific runner for each dll-file scenario

This example assumes the name of the runner is based on the name of the dll contraining the tests, by appending `Runner` to the dll-name.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <DllFilenameFilter>.*</DllFilenameFilter><!-- Regex filter -->
    <DllRunner>${dllpath}\${dllname}Runner.exe</DllRunner>
  </Catch2Adapter>

</RunSettings>
```

#### Example of a minimal runsettings file - single runner for all test dll-files scenario

This example is for a test runner that takes the path to the dll as last parameter. All other parameters are the normal Catch2 commanline parameters. Location of the runner is the same path as the test dll in this example.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <DllFilenameFilter>.*</DllFilenameFilter><!-- Regex filter -->
    <DllRunner>${dllpath}\Catch2TestRunner.exe</DllRunner>
    <DllRunnerCommandline>${catch2} ${dll}</DllRunnerCommandline>
  </Catch2Adapter>

</RunSettings>
```

#### Example of a minimal runsettings file - debug-postfix for debug builds scenario

This example builds on top of the single runner for all test dll-files scenario.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <DllFilenameFilter>.*</DllFilenameFilter><!-- Regex filter -->
    <DllPostfix>_D</DllPostfix>
    <DllRunner>${dllpath}\${dllname}Runner${postfix}.exe</DllRunner>
  </Catch2Adapter>

</RunSettings>
```

---

### Catch2 tests are located both in an executable and in a dll

It is possible to link both dll and exe sources to the Test explorer. Just combine the required fields for both scenarios.

#### Example of a minimal runsettings file

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <DllFilenameFilter>.*</DllFilenameFilter><!-- Regex filter dll source -->
    <DllRunner>${dllpath}\${dllname}Runner.exe</DllRunner>
    <FilenameFilter>.*</FilenameFilter><!-- Regex filter exe source-->
  </Catch2Adapter>

</RunSettings>
```

---

### Run specific tests in a single process

Starting version 2.0.0 of the **Test Adapter for Catch2** tests from a specific test source are run in a single process by default. Before each tests was run in its own process by default. This behavior is configured via the [`<ExecutionMode>`](Settings.md#executionmode)-setting. The advantage of running tests from a specific test source in a single process is that the test execution is typically significantly faster. However, there are some tests that would benefit from being run in a separate process. The way to do that is to add a tag of your choosing to the test. If you then configure the [`<ExecutionModeForceSingleTagRgx>`](Settings.md#executionmodeforcesingletagrgx)-setting to force test cases with specific tags to be run in a single process.

#### Example of a runsettings file for this scenario

This example is for tests located in a test executable. Tests tagged with the `[Slow]`-tag should be run isolated in a single separate process. For bonus points, the combined execution of all tests not tagged with the `[Slow]`-tag should finish within 10 seconds. Each test tagged with the `[Slow]`-tag is allowed to take 60 seconds.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    <ExecutionMode>Combine</ExecutionMode>
    <ExecutionModeForceSingleTagRgx>Slow</ExecutionModeForceSingleTagRgx>
    <CombinedTimeout>10000</CombinedTimeout><!-- Milliseconds -->
    <TestCaseTimeout>60000</TestCaseTimeout><!-- Milliseconds -->
  </Catch2Adapter>

</RunSettings>
```

---

### Use different timeout settings for specific test sources

Starting version 2.0.0 of the **Test Adapter for Catch2** it is possible override settings for specific test sources. This is configured via the [`<Overrides>`](Settings.md#overrides) section.

#### Example of a runsettings file for this scenario

This example builds on the example for the [Run specific tests in a single process](#run-specific-tests-in-a-single-process) scenario. Here we want the `<TestCaseTimeout>` timeout for test sources with names ending in `_debug` to be set to 5 minutes and `<CombinedTimeout>` to 2 minutes. Test sources with names ending in `_fast` should not run any test in a separate process.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

  <!-- Adapter Specific sections -->
  <Catch2Adapter>
    <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    <ExecutionMode>Combine</ExecutionMode>
    <ExecutionModeForceSingleTagRgx>Slow</ExecutionModeForceSingleTagRgx>
    <CombinedTimeout>10000</CombinedTimeout><!-- Milliseconds -->
    <TestCaseTimeout>60000</TestCaseTimeout><!-- Milliseconds -->
    <Overrides><!-- Introduced in v2.0.0 -->
      <Source filter="_debug$">
        <CombinedTimeout>120000</CombinedTimeout><!-- 2 mintes in Milliseconds -->
        <TestCaseTimeout>300000</TestCaseTimeout><!-- 5 minutes in Milliseconds -->
      </Source>
      <Source filter="_fast$">
        <ExecutionModeForceSingleTagRgx>UnusedTag</ExecutionModeForceSingleTagRgx>
      </Source>
    </Overrides>
  </Catch2Adapter>

</RunSettings>
```

Note, `<ExecutionModeForceSingleTagRgx>` is set to a random string not used as any tag. Leaving it empy (_e.g._, using `<ExecutionModeForceSingleTagRgx/>`) will not reset it to the default value, but will keep the current setting, and as such not override it. As such you need to set a value that you know is not used in tags for your tests.
