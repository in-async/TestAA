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

基本的な使い方は下記の通りです：
```cs
TestAA
    .Act(テスト対象コード)
    .Assert(テスト対象コードの戻り値の検証, 例外の検証);
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

`Assert()` で `Act()` の結果を検証します。第１引数で `Act()` に渡されたテスト対象コードの戻り値を検証し、第２引数で `Act()` で生じた例外を検証（または例外が生じなかった事を検証）します。
```cs
    .Act(() => int.Parse("123"))
    .Assert(
        @return: ret => { /* ここで戻り値の検証。Act で例外が生じた場合は戻り値が無いので呼ばれない */ },
        exception: ex => { /* ここで例外の検証 */ }
    );
```

検証はラムダ式やデリゲートではなく値を直接入力する事でも可能です。テストの失敗は、既定では `TestAssertFailedException` のスローによって通知されます。
```cs
TestAA.Act(() => int.Parse("123")).Assert(123);  // OK
TestAA.Act(() => int.Parse("abc")).Assert(ret => { }, new FormatException());  // OK
TestAA.Act(() => int.Parse("abc")).Assert(123);  // TestAssertFailedException
```


## Examples
### Basic
```cs
TestAA.Act(() => int.Parse("123")).Assert(123);
```
### Exception
```cs
TestAA.Act(() => int.Parse("abc")).Assert(ret => { }, exception: new FormatException());
```

### Out parameter
```cs
int result = default;
TestAA.Act(() => int.TryParse("123", out result)).Assert(true);

// Additional Assert
Assert.AreEqual(123, result);
```

### Lambda assert
```cs
TestAA.Act(() => int.Parse("123")).Assert(
      @return: ret => Assert.AreEqual(123, ret)
    , exception: ex => Assert.IsNull(ex)
);
```

### Replace default assert
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

### Test cases
```cs
Action TestCase(int testNumber, string input, int expected, Exception expectedException = null) => () => {
    var msg = "No." + testNumber;

    TestAA.Act(() => int.Parse(input)).Assert(expected, expectedException, msg);
};

foreach (var action in new[] {
    TestCase( 0, null , expected: 0  , expectedException: new ArgumentNullException()),
    TestCase( 1, "abc", expected: 0  , expectedException: new FormatException()),
    TestCase( 2, "123", expected: 123),
}) { action(); }
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
