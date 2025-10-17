
資材があるディレクトリで、以下を実行するのみ。publish フォルダ配下を、IIS のサイト物理パスに配置すれば OK。
```
dotnet restore
dotnet publish -c Release -o .\publish -r win-x64 --self-contained true
```