# バックエンド側

## GET http://systrialsvr/api/VirtualMachineStatus

* 仮想マシンの稼働状況一覧を取得する

## POST http://systrialsvr/api/VirtualMachineStatus/Refresh

* 仮想マシンの [状態] と [接続] を更新する

## GET http://systrialsvr/api/VirtualMachineStatus/RefreshTime

* 更新時間を取得する

## GET http://systrialsvr/api/VirtualMachineStatus/Reset

* 仮想マシンの [状態] と [接続] をテスト用データで更新する
* デバッグ用

## GET http://systrialsvr/api/VirtualMachineStatus/Message

* 表示メッセージを取得する

## POST http://systrialsvr/api/VirtualMachineStatus/Message

* 表示メッセージを更新する
* payload { "message" }

## フロントエンド側は[こちら](https://github.com/ahy72/FrontEnd)
