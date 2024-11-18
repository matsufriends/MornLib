# MornLib

現在準備中です。
以下、随時執筆中。

# 概要

`Unity`の個人開発で頻繁に使う機能を都度更新しています。

全てのコードは「**自分の開発の補助**」として作成していますが、  
一人でも多くの方の役に立てばと思い、公開しています。

また、本リポジトリのコードはすべて`Unlicense`の為、
自己責任で全てご自由にお使いいただけます。

# 動作環境

`Unity 2021.3.6`にて、全てのコードの動作を確認済です。  
より以前の`ver`では、一部の`C#`の機能に対応していない可能性があります。

# 導入方法

## 全ての機能を利用する

本リポジトリを`submodule`としてプロジェクトに導入することで、全ての機能をお使い頂けます。  
導入先が`Assets`フォルダ以下になるようご注意下さい。  
詳しくは「[git submodule](https://www.google.com/search?q=git+submodule)」等でご確認ください。

## いくつかの機能を利用する

本ライブラリの各機能は`UPM`に対応しています。  
`PackageManager`の "Add package from git URL... " で以下のURLを入力

``` url
https://github.com/matsufriends/MornLib.git?path=各機能のフォルダ名
```

詳細な機能名は、各機能の`README.md`をご確認ください。

---

[MornEnumドキュメント](MornEnum/README.md)  
[MornHierarchyドキュメント](MornHierarchy/README.md)  
[MornSoundProcessorドキュメント](MornSoundProcessor/README.md)