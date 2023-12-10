# Test Adapter for Catch2

Within Visual Studio, the Test Explorer is a convenient way to run and debug unit tests. This test adapter adds support for the [Catch2 C++ test framework](https://github.com/catchorg/Catch2) (version 2.x and 3.x). This adapter is for use in combination with Visual Studio 2017 and later.

## How to get it

The **Test Adapter for Catch2** is available via the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=JohnnyHendriks.ext01). It is available under the name "Catch Adapter for Catch2". The easiest way to get it is via "Extensions and Updates..." in the "Tools" menu. Of course you can also [build it yourself](Docs/Build.md).

## A note on usage

 Out of the box the extension does not discover tests. You need to add settings for the **Test Adapter for Catch2** to a _.runsettings_ file and use that as your test settings. This prevents the discovery mechanism from running non-Catch2 executables in your solution upon first use. See the [Walkthrough](Docs/Walkthrough-vs2022.md) for a detailed description on how to use the **Test Adapter for Catch2**.

## Documentation

For documentation on the **Test Adapter for Catch2** see the following links.

- [Overview (_all documentation_)](./Docs/Readme.md)
- [Walkthrough: using the Test Adapter for Catch2](./Docs/Walkthrough-vs2022.md)
- [Configure runsettings (_use cases_)](./Docs/Runsettings.md)
- [Troubleshooting guide](./Docs/Troubleshooting.md)
- [Change log](./Docs/CHANGELOG.md)
