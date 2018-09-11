# Unit Tests

## Organization

Similar to [Zen Coding: Structuring Unit Tests](http://zendeveloper.blogspot.co.at/2012/01/structuring-unit-tests.html),
but instead of nested classes new namespaces and top-level classes are used, so it gets clearer.

In terms of organization this means that sub-folders get introduced. E.g.
```
Tests.csproj
+ Services
  + ProcessorTests
    | MethodA.cs
    | MethodB.cs
    | ...
  + RepositoryTests
    | Base.cs (common SetUp-Code)
    | MethodA.cs
    | MethodB.cs
    | ...
    + MethodC
      | StateA.cs
      | StateB.cs
      | ...
```

## Naming

The scheme from [Naming standards for unit tests - Osherove](http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html) 
is quite good, and is used with a modification to match beforementioned organization.

`State_under_Test___expected_Behavior` 

The method-name is already included in the class-name of the test, according to the above organizational-rules.

This naming-scheme is give by the following regex-pattern: `^([^_]+_)*[^_]+_{3}([^_]+_)*[^_]+$`

Note: if states are divied in folders (see above), so the naming can be shortened to `expected_Bahavior`, because the state is already given.
