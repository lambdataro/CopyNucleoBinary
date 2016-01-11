# CopyNucleoBinary
Nucleo F401RE の bin ファイルを転送するプログラム。

## 必要なソフトウェア
- MSYS (cp コマンドを利用)

## 対応ボード
- Nucleo F401RE
- STM32 F7 Discovery

## 使い方
```
> CopyNucleoBinary
```
現在のディレクトリで最初に見つかった `.bin` ファイルを転送します。

```
> CopyNucleoBinary Nucleo-F401RE.bin
```
指定したファイルを転送します。

拡張子 `.bin` をこのプログラムで開くように設定することで、ダブルクリックで転送できます。
