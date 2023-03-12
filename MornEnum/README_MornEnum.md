# MornEnum
## 概要
`enum`に関する機能を提供します。
- `enum`を`string`でシリアライズする`Attribute`
- `enum`の全要素取得や`ToString`結果のキャッシュ機能。

## 導入

- `PackageManager`の "Add package from git URL... " で以下のURLを入力。
    - https://github.com/matsufriends/MornLib.git?path=MornEnum

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記。
```
"com.matsufriends.mornenum": "https://github.com/matsufriends/MornLib.git?path=MornEnum",
```

## 使い方
```
//enumを文字列でシリアライズする
[SerializeField, MornEnumToString(typeof(HogeType))] private string hoge;

//enumの要素数を取得する
MornEnumUtil<HogeType>.Count;

//enumの全要素を取得する
MornEnumUtil<HogeType>.Values;

//ToString結果をキャッシュし、Boxingを抑える
MornEnumUtil<HogeType>.CachedToString(HogeType.A);
```
