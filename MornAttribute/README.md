# MornAttribute

## 概要

新たな`Attribute`を提供します。

- 値をInspector上から変更不可にする`[ReadOnly]`
- 参照する変数が`true`のときのみ表示する`[ShowIf("変数名")]`
- 参照する変数が`true`のとき非表示にする`[HideIf("変数名")]`
- `Vector2/Vector2Int`を`Slider`で表示する`[MinMaxSlider(最小値,最大値)]`
- Inspector上での表示名を指定する`[Label("ラベル名")]`

## 導入

下記どちらでも導入できます。

- `PackageManager`の "Add package from git URL... " で以下のURLを入力

```
https://github.com/matsufriends/MornLib.git?path=MornAttribute
```

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記

``` json
"com.matsufriends.mornattribute": "https://github.com/matsufriends/MornLib.git?path=MornAttribute",
```

## 使い方

### 値をInspector上から変更不可にする

``` csharp
[SerializeField, ReadOnly] private int a;
```

### 参照する変数が`true`のときのみ表示する

``` csharp
[SerializeField] private bool a;
[SerializeField, ShowIf(nameof(a))] private int b;
```

`[ShowIf("a")]`でも動作しますが、`nameof`を使うことで変数名の変更に対応できます。

### 参照する変数が`true`のとき非表示にする

``` csharp
[SerializeField] private bool a;
[SerializeField, HideIf(nameof(a)] private int b;
```

`[HideIf("a")]`でも動作しますが、`nameof`を使うことで変数名の変更に対応できます。

### `Vector2/Vector2Int`を`Slider`で表示する

``` csharp
[SerializeField, MinMaxSlider(4, 10)] private Vector2Int a;
[SerializeField, MinMaxSlider(3.2f, 5.4f)] private Vector2 b;
```

下記の場合、エラーが表示されます。

- 型が`Vector2 / Vector2Int`以外
- `MinMaxSlider`の引数の大小関係が不正な場合、エラーが表示されます。

### Inspector上での表示名を指定する

``` csharp
[SerializeField, Label("あいうえお")] private int a;
```
