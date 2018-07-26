# Known issues for Test Adapter for Catch2

All software has issues, the **Test Adapter for Catch2** is no exception. The following is a list of issues that are currently known (at least to me).

- Test case names that differ only in case (_e.g._, "NAME" and "name") and are part of the same Catch2 executable, are not handled well. See [Notes on test case names](Capabilities.md#notes-on-test-case-names) for details. The workaround for this issue is obvious, don't use test case names that differ only in case.