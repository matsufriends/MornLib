# MornSoundProcessor

## 概要

`AudioClip`に関する機能を提供します。

- `AudioClip`の最初の無音区間を削除する
- `AudioClip`の最後の無音区間を削除する
- `AudioClip`の音量を平滑化する

## 導入

- `PackageManager`の "Add package from git URL... " で以下のURLを入力。
    - https://github.com/matsufriends/MornLib.git?path=MornSoundProcessor

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記。

```
"com.matsufriends.mornsoundprocessor": "https://github.com/matsufriends/MornLib.git?path=MornSoundProcessor",
```

## 使い方

ツールバーの`Morn/MornSoundProcessor`よりウィンドウを開く。

### Input

- 変換するAudioClipを指定します
- `Audio Clip List` - 変換するAudioClipを割り当てます

### Cut Beginning Silence

- 最初の無音時間を除去します
- `Use Cut Beginning Silence` - この機能を利用するか
- `Beginning Amplitude` - 無音と判定する音量閾値(0~1)
- `Beginning Offset Sample` - 有音判定位置を過去側に補正するオフセット値

### CutEndSilence

- 最後の無音時間を除去します
- `Use Cut Ending Silence` - この機能を利用するか
- `Ending Amplitude` - 無音と判定する音量閾値(0~1)
- `Ending Offset Sample` - 有音判定位置を未来側に補正するオフセット値

### NormalizeAmplitude

- 音量を平滑化します
- `Use Normalize Amplitude` - この機能を利用するか
- `Normalize Amplitude` - 平滑化後の最大音量

### Output

- 出力先を設定します
- `Under Assets Folder Name` - 保存先フォルダーのパスを `Assets/` に続く形で指定します。
- `Result List` - 出力後の`Audio Clip` がここに表示されます

### Generate

- 変換を実行します
