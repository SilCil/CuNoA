# CuNoA - Customizable Node Analysis

![Screen Shot](Images/SoftwareScreenshot.png)

Windowsインストーラーをダウンロード（リンク追加予定）

## 研究室での解析用に作成したWindowsソフトウェア

プログラムを書いたことがない、でも、手作業では大変な量のデータがある。
そんな人の助けにちょっとだけなるかもしれない。

卒業するので後輩への研究の引き継ぎも兼ねて、どうせならオープンソースにしてしまおうと思いました。

プログラムが書ける人にとってはあまり役に立たないかも。
C#で独自処理を記述できるのでC#慣れてるっていう人には、
ファイル入出力とかのお決まりの処理を毎回毎回書かなくてもいいので使えるかもしれない。

ご意見・ご要望があれば、気軽にIssue立ててくださいな。

## 「順番に実行」 - 処理はシンプルなフローに

処理は「分岐なしで、上から順番に実行していく」というシンプルな構成にしました。
それというのも、このソフトウェアの使用者に「プログラムできない人」を想定しているからです。

プログラムコードを書かない解析ソフトウェアというのはあるにはあって、
実際に候補として触ったこともあるんですが、私の肌には合わなかった。

ノードに複数のコネクタをつなげる形式だったりして、
ちょっと固有の処理をやろうとすると条件分岐やら色々あってごちゃごちゃになってしまうという経験がありまして。
これを後輩に教えるのは厳しいと感じました。

「もしxxだったらooする」というのは、わりかし複雑な思考回路だと思うの。
それを頭の中で順序立ててマウスカチカチして繋げてノードとコネクタを組み上げられるなら、
それもうプログラミングの文法を知らないだけのプログラムできる人じゃんって思うわけです。

## 独自処理のためのライブラリ作成機能

既に用意されている処理で満足できない場合、C#コードを書いて独自処理を追加できます。
いや、コード書くんかいって思うかもしれないですね。
やりたい処理は人によって違うので、そのすべてをカバーするのは無理ってもんです。
あなたがやりたい処理を私が今知ることは残念ながらできませんでした。

もちろん、コードを書くなら、必要な部分だけに集中してほしい。
ファイルの読み込みとか、データ処理の本質じゃない部分で悩んでほしくない。
だから、既存処理は全てテンプレートとして読み込めるようになりました。

あなたがやりたい処理に一番近そうな既存処理を選んで、
なんかいい感じに書き換えてくださいね。

---
### ここからは実装の話
---

#### 使用ライブラリ

開発に当たり以下のパッケージを使用させていただきました。
ありがとうございます。

- [Math.NET Numerics](https://numerics.mathdotnet.com/)
- [Roslyn](https://github.com/dotnet/roslyn)
- [RoslynPad](https://github.com/roslynpad/roslynpad)
- [FolderBrowserEx](https://github.com/evaristocuesta/FolderBrowserEx)

#### コードについて

GUIアプリケーション作るってなった時に、WPFとか最もモダンなフレームワーク使うほうがいいというのはそれはそうなんだけど、
プログラマでも何でもない人にとってはWinFormsのほうが直観的だと思うんですよ。
将来、研究室の後輩がいじりたいってなった時に、MVVMとかだと、まずそこの学習コストが嵩んでしまうのは嫌だなと。
そういうことを考えて泥臭いコードになってます。

ちょっと開発を優先して手癖で処理を書いた部分もあるけど、サンプルコードもLINQを使わないように修正していきたいな、と思ってます。
