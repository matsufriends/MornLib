# MornHierarchy

## 概要

`FPS(Frame Per Seconds)`に関する機能を提供します。

- `MornFPSCheckerMono`コンポーネントで平均FPSを可視化できます
- `MornFPSManagerMono`コンポーネントで厳密なFPSでゲームを更新できます

## 導入

- `PackageManager`の "Add package from git URL... " で以下のURLを入力
    - https://github.com/matsufriends/MornLib.git?path=MornFPS

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記

```
"com.matsufriends.mornfps": "https://github.com/matsufriends/MornLib.git?path=MornFPS",
```

## 使い方

`MornFPSCheckerMono`コンポーネントのアタッチで、`GameObject`名に平均FPSを可視化できます。  
コンポーネントの`_saveFrames`を変更することで、直近何フレームの平均を求めるか変更できます。

---

`MornFPSManagerMono`コンポーネントがアタッチされた`GameObject`が`Scene`上に1つ存在するだけで、  
他のスクリプトの`Update`関数の頻度は自動で適切なFPSに調整されます。  
（`Singleton`パターンを採用しており、1つ目はDontDestroy領域へ、2つ目以降は即Destroyされます）

想定FPSを超える過剰な更新を`Thread.Sleep`で制御するため、  
他コンポーネントは従来どおり`Update`関数を経由して制御します。
