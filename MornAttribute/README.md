# MornAttribute

## 概要

新たな`Attribute`を提供します。

- 値をInspector上から変更不可にする`[ReadOnly]`
- 参照する変数/プロパティが`true`のときのみ表示する`[ShowIf("変数名")]`
- 参照する変数/プロパティが`true`のとき非表示にする`[HideIf("変数名")]`
- 参照する変数/プロパティが`true`のとき変更可能にする`[EnableIf("変数名")]`
- 参照する変数/プロパティが`true`のとき変更不可にする`[DisableIf("変数名")]`
- `Vector2/Vector2Int`を`Slider`で表示する`[MinMaxSlider(最小値,最大値)]`
- Inspector上での表示名を指定する`[Label("ラベル名")]`

## 導入方法

`PackageManager`の "Add package from git URL... " にて、以下のURLを入力して下さい。

```
https://github.com/matsufriends/MornLib.git?path=MornAttribute
```

## 使用例

``` csharp
public class MornAttributeUsage : MonoBehaviour
{
    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private string name = "Morn";
    
    [Header("Parameter")]
    [SerializeField] private int hp = 10;
    private bool IsAlive => hp > 0;
    
    [Header("ShowIf / HideIf")]
    [SerializeField, ShowIf(nameof(IsAlive))] private string showIfMessage;
    [SerializeField, HideIf(nameof(IsAlive))] private string hideIfMessage;
    
    [Header("EnableIf / DisableIf")]
    [SerializeField, EnableIf(nameof(IsAlive))] private string enableIfMessage;
    [SerializeField, DisableIf(nameof(IsAlive))] private string disableIfMessage;
    
    [Header("MinMaxSlider")]
    [SerializeField, MinMaxSlider(4, 10)] private Vector2Int intRange;
    [SerializeField, MinMaxSlider(3.2f, 5.4f)] private Vector2 floatRange;
    
    [Header("Label")]
    [SerializeField, Label("移動速度")] private float moveSpeed;
}
```
![InspectorWindow001](https://github.com/matsufriends/MornLib/assets/50489724/40361502-3418-4963-9e5c-0dc10c10ea23)
`hp`が0より大きいとき（`IsAlive`が`true`のとき）

![InspectorWindow002](https://github.com/matsufriends/MornLib/assets/50489724/b62c2f08-2d3c-43b2-a360-f205f10af96e)
`hp`が0以下のとき（`IsAlive`が`false`のとき）
