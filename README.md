# TestAA
[![Build status](https://ci.appveyor.com/api/projects/status/8a7wlfjt9oedlmy5/branch/master?svg=true)](https://ci.appveyor.com/project/inasync/testaa/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Inasync.TestAA.svg)](https://www.nuget.org/packages/Inasync.TestAA/)

***TestAA*** は Arrange-Act-Assert（AAA）パターンによるテストの記述をサポートする為のシンプルなライブラリです。


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.0+
- .NET Framework 4.5+


## Description
***TestAA*** は AAA パターンのうち Act-Assert の記述を直接的に補助します。

基本的な使い方は下記になります：
```cs
TestAA
    .Act(テスト対象コード)
    .Assert(予期するテスト対象コードの戻り値 or 例外の型);
```

Arrange に相当する処理は `TestAA.Act()` の呼び出しより前に記述します。
```cs
// Arrange
// ...

// Act
TestAA.Act(...)
```

`Act()` の引数には、テスト対象となるメソッドまたはコードのラムダ式やデリゲートを渡して下さい。テストの対象ではないメソッドやコードを含めますと、そこから発生した例外がテスト対象コードから生じたものとして扱われてしまい、正しい検証が行えなくなります。
```cs
TestAA.Act(() => { /* ここでテスト対象のメソッドを呼ぶ */ })
```

`Assert()` で `Act()` の結果を検証します。テスト対象コードが例外を生じる事を予期する場合は、`Assert()` の引数または型引数にその例外の型を渡します。
```cs
TestAA.Act(() => int.Parse("abc")).Assert(typeof(FormatException));
TestAA.Act(() => int.Parse("abc")).Assert<FormatException>();
```

例外が起こらず戻り値が返される事を予期する場合は、`Assert()` の引数に予期する戻り値を渡します。
```cs
TestAA.Act(() => int.Parse("123")).Assert(123);
```

もしテスト結果が予期した値と異なる場合は、既定では `TestAssertFailedException` のスローによってテストの失敗が通知されます。
```cs
TestAA.Act(() => int.Parse("abc")).Assert(123);  // TestAssertFailedException
```

`Assert()` の引数には予期する戻り値や例外の型以外に、より詳細な検証を行う為のラムダ式を渡すことも可能です。
```cs
    .Act(() => int.Parse("123"))
    .Assert(
        @return: ret => { /* 戻り値の検証コード。Act で例外が生じた場合は戻り値が無いので呼ばれない */ },
        exception: ex => { /* 例外の検証コード */ }
    );
```


## Examples
### Basic
```cs
TestAA.Act(() => int.Parse("123")).Assert(123);
```
### Throws Exception
```cs
TestAA.Act(() => int.Parse("abc")).Assert<FormatException>();
```

### Out Parameter
```cs
int result = default;
TestAA.Act(() => int.TryParse("123", out result)).Assert(true);

// Additional Assert
Assert.AreEqual(123, result);
```

### Lambda Assert
```cs
TestAA.Act(() => int.Parse("123")).Assert(
      @return: ret => Assert.AreEqual(123, ret)
    , exception: ex => Assert.IsNull(ex)
);
```

### Task Synchronously
```cs
// Task
TestAA.Act(() => Task.FromResult(123)).Assert(123);

// ValueTask
TestAA.Act(() => new ValueTask<int>(123)).Assert(123);
```

### Task Throws Exception
```cs
// Task
TestAA.Act(() => Task.FromException(new ApplicationException())).Assert<ApplicationException>();

// ValueTask
TestAA.Act(() => new ValueTask(Task.FromException(new ApplicationException()))).Assert<ApplicationException>();
```

### Raw Task
```cs
// Task
var task = Task.FromResult(123);
TestAA.Act<Task<int>>(() => task).Assert(task);

// ValueTask
TestAA.Act<ValueTask<int>>(() => new ValueTask<int>(123)).Assert(new ValueTask<int>(123));
```

### Immediate Enumerable Evaluation
```cs
TestAA.Act(() => CreateEnumerable()).Assert<ApplicationException>();

IEnumerable<int> CreateEnumerable() {
    yield return 123;
    throw new ApplicationException();
}
```

### Replace Default Assert
```cs
class MSTestAssert : TestAssert {
    public override void Is<T>(T actual, T expected, string message) {
        Assert.AreEqual(expected, actual, message);
    }
}
...
TestAA.TestAssert = new MSTestAssert();

TestAA.Act(() => int.Parse("123")).Assert(123);  // Assert.AreEqual()
```

### Test Cases
```cs
Action TestCase(int testNumber, string input, int expected = default, Type expectedException = null) => () => {
    var msg = "No." + testNumber;

    TestAA.Act(() => int.Parse(input)).Assert(expected, expectedException, msg);
};

foreach (var action in new[] {
    TestCase( 0, null , expectedException: typeof(ArgumentNullException)),
    TestCase( 1, "abc", expectedException: typeof(FormatException)),
    TestCase( 2, "123", expected: 123),
}) { action(); }
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
