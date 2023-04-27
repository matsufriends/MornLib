# MornAttribute

## 概要

`Attribute`を提供します。

- 値をInspector上から変更不可にする`[ReadOnly]`
- 参照する変数が`true`のときのみ表示する`[ShowIf("変数名")]`
- 参照する変数が`true`のとき非表示にする`[HideIf("変数名")]`
- `Vector2/Vector2Int`を`Slider`で表示する`[MinMaxSlider(最小値,最大値)]`

## 導入

下記のどちらでも導入頂けます。

- `PackageManager`の "Add package from git URL... " で以下のURLを入力

```
https://github.com/matsufriends/MornLib.git?path=MornAttribute
```

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記

```
"com.matsufriends.mornattribute": "https://github.com/matsufriends/MornLib.git?path=MornAttribute",
```

## 使い方

### 値をInspector上から変更不可にする

```
[SerializeField, ReadOnly] private int a;
```

### 参照する変数が`true`のときのみ表示する

```
[SerializeField] private bool a;
[SerializeField, ShowIf(nameof(a))] private int b;
```

`[ShowIf("a")]`でも動作しますが、`nameof`を使うことで変数名の変更に対応できます。

### 参照する変数が`true`のとき非表示にする

```
[SerializeField] private bool a;
[SerializeField, HideIf(nameof(a)] private int b;
```

`[HideIf("a")]`でも動作しますが、`nameof`を使うことで変数名の変更に対応できます。

### `Vector2/Vector2Int`を`Slider`で表示する

```
[SerializeField, MinMaxSlider(4, 10)] private Vector2Int a;
[SerializeField, MinMaxSlider(3.2f, 5.4f)] private Vector2 b;
```

下記の場合、エラーが表示されます。

- 型が`Vector2 / Vector2Int`以外
- `MinMaxSlider`の引数の大小関係が不正な場合、エラーが表示されます。
