# MornHierarchy
## 概要
`Hierarchy`に関する機能を提供します。
- `MornHierarchyColor`コンポーネントにより、重要なオブジェクトに色を付けて目立たせます。
- `MornHierarchyLine`コンポーネントにより、オブジェクト間に区切り線を目立たせます。

## 導入

- `PackageManager`の "Add package from git URL... " で以下のURLを入力。
    - https://github.com/matsufriends/MornLib.git?path=MornHierarchy

- `Packages/manifest.json` の `"dependencies":{` 行の下に以下を追記。
```
"com.matsufriends.mornhierarchy": "https://github.com/matsufriends/MornLib.git?path=MornHierarchy",
```

## 使い方
![1](https://user-images.githubusercontent.com/50489724/174723354-9df0d429-3360-4240-87bc-45f7443784a5.jpg)

`MornHierarchyColor`コンポーネントのアタッチで、対象の`GameObject`に`Hierarchy`上で色が付きます。  
コンポーネントの`ApplyChildren`にチェックすることで、子要素にグラデーションがかかります。

子要素にもコンポーネントをアタッチすることで、親とは違う色で再び色を付けられます。  
別のグラデーションをかけることも可能です。

---

![2](https://user-images.githubusercontent.com/50489724/224517185-1e4b1f54-717c-4af9-9870-43824a4e1bdb.png)

`MornHierarchyLine`コンポーネントのアタッチで、対象の`GameObject`が区切り線になります。  
`GameObject`の名前が中央寄りになり、上下に黒線が描画されます。

---

`Preferences/MornHierarchy`より、下記の設定を変更可能です。
- 背景色の透明度 (`default: 0.3f`)
- Tagを表示するか (`default: false`)
