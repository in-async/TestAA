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
    .Assert(テスト対象コードの戻り値の検証, 例外の検証, その他の検証);
```

Arrange に相当する処理は `TestAA.Act()` の呼び出しより前に記述します。
```cs
// Arrange
// ...

// Act
TestAA.Act(...)
```

`Act()` の引数には、テスト対象となるメソッドまたはコードのラムダ式やデリゲートを渡して下さい。テストの対象ではないメソッドまたはコードも含めますと、そこから発生した例外がテスト対象コードから生じたものとして扱われてしまい、正しい検証が行えなくなります。
```cs
TestAA.Act(() => { /* ここでテスト対象のメソッドを呼ぶ */ })
```

`Assert()` で `Act()` の結果を検証します。第１引数で `Act()` に渡されたテスト対象コードの戻り値を検証し、第２引数で `Act()` で生じた例外を検証（または例外が生じなかった事を検証）します。
```cs
    .Act(() => int.Parse("123"))
    .Assert(
        @return: ret => { /* ここで戻り値の検証 */ },
        exception: ex => { /* ここで例外の検証 */ },
        others: () => { /* ここで上記以外の検証。不要なら省略 */ }
    );
```


## Usage
```cs
public void IntParseTest() {
    // Success
    TestAA.Act(() => int.Parse("123")).Assert(
        ret => ret.Is(123),
        ex => ex?.GetType().Is(null)
    );

    // FormatException
    TestAA.Act(() => int.Parse("abc")).Assert(
        ret => { },
        ex => ex?.GetType().Is(typeof(FormatException))
    );
}
```

下記は *MSTest* を利用した、より実践的な例です：
```cs
[DataTestMethod]
[DataRow(0, null, null, typeof(ArgumentNullException))]
[DataRow(1, "123", 123, null)]
[DataRow(2, "abc", null, typeof(FormatException))]
public void IntParseTest(int testNumber, string input, int expected, Type expectedExceptionType) {
    var msg = "No." + testNumber;

    TestAA.Act(() => int.Parse(input)).Assert(
        ret => Assert.AreEqual(expected, ret, msg),
        ex => Assert.AreEqual(expectedExceptionType, ex?.GetType(), msg)
    );
}
```
または
```cs
[TestMethod]
public void IntParseTest() {
    Action TestCase(int testNumber, string input, int expected, Type expectedExceptionType = null) => () => {
        var msg = "No." + testNumber;

        TestAA.Act(() => int.Parse(input)).Assert(
            ret => Assert.AreEqual(expected, ret, msg),
            ex => Assert.AreEqual(expectedExceptionType, ex?.GetType(), msg)
        );
    };

    foreach (var action in new[] {
        TestCase( 0, null , expected: 0  , expectedExceptionType: typeof(ArgumentNullException)),
        TestCase( 1, "abc", expected: 0  , expectedExceptionType: typeof(FormatException)),
        TestCase( 2, "123", expected: 123),
    }) { action(); }
}
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
