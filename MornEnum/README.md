# MornEnum

## 概要

`enum`に関する機能を提供します。

- `enum`を`string`でシリアライズする`Attribute`の追加
- `enum`の全要素取得や`ToString`結果をキャッシュする`Generic`クラス

## 導入

下記どちらでも導入できます。

- `PackageManager`の "Add package from git URL... " で以下のURLを入力

``` 
https://github.com/matsufriends/MornLib.git?path=MornEnum
```

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記

``` json
"com.matsufriends.mornenum": "https://github.com/matsufriends/MornLib.git?path=MornEnum",
```

## 使い方

### enumを文字列でシリアライズする

``` csharp
[SerializeField, MornEnumToString(typeof(HogeType))] private string hoge;
```

### enumの要素数を取得する

``` csharp
MornEnumUtil<HogeType>.Count;
```

### enumの全要素を取得する

``` csharp
MornEnumUtil<HogeType>.Values;
```

### ToString結果としてキャッシュしたものを利用する

``` csharp
MornEnumUtil<HogeType>.CachedToString(HogeType.A);
```
