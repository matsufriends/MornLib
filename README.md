# MornLib

現在準備中です。
以下、随時執筆中。

# 概要
`Unity`の個人開発で頻繁に使う機能を都度更新しています。  

全てのコードは「**自分の開発の補助**」として作成していますが、  
一人でも多くの方の役に立てばと思い、公開しています。  

また、本リポジトリのコードはすべて`Unlicense`の為、全てご自由にお使いいただけます。  
（もちろん利用報告を頂ければ、喜びます！）

# 動作環境
`Unity 2021.3.6`にて、全てのコードの動作を確認済です。  
より以前の`ver`では、一部の`C#`の機能に対応していない可能性があります。

# 導入方法

## 全ての機能を利用する
本リポジトリを`submodule`としてプロジェクトに導入することで、全ての機能をお使い頂けます。  
導入先が`Assets`フォルダ以下になるようご注意下さい。

```
// git導入済み環境下でのコマンドの実行例です。自己責任でお願いします。
// cdコマンド等で、Assetsへ移動してから実行すること。
~/Unityのプロジェクト/Assets> git submodule add https://github.com/matsufriends/MornLib.git
```

詳しくは「[git submodule](https://www.google.com/search?q=git+submodule)」等でご確認ください。

## いくつかの機能を利用する
全ての機能は、`UPM(Unity Package Manager)`に対応しています。  

下記のどちらでも導入頂けます。
- `PackageManager`の "Add package from git URL... " で以下のURLを入力

``` url
https://github.com/matsufriends/MornLib.git?path=各機能のフォルダ名
```

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記

``` json
"com.matsufriends.各機能のパッケージ名": "https://github.com/matsufriends/MornLib.git?path=各機能のフォルダ名",
```

詳細な機能名は、各機能の`README.md`をご確認ください。

---

# MornAttribute

新たな`Attribute`を提供します。

``` json
"com.matsufriends.mornattribute": "https://github.com/matsufriends/MornLib.git?path=MornAttribute",
```

[MornAttributeドキュメント](MornAttribute/README.md)

# MornEnum

`enum`に関する機能を提供します。

``` json
"com.matsufriends.mornenum": "https://github.com/matsufriends/MornLib.git?path=MornEnum",
```

[MornEnumドキュメント](MornEnum/README.md)

# MornHierarchy

`Hierarchy`に関する機能を提供します。

``` json
"com.matsufriends.mornhierarchy": "https://github.com/matsufriends/MornLib.git?path=MornHierarchy",
```

[MornHierarchyドキュメント](MornHierarchy/README.md)

# MornSoundProcessor

`AudioClip`に関する機能を提供します。

``` json
"com.matsufriends.mornsoundprocessor": "https://github.com/matsufriends/MornLib.git?path=MornSoundProcessor",
```

[MornSoundProcessorドキュメント](MornSoundProcessor/README.md)
