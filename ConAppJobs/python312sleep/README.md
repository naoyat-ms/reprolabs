# Dockerfile と sleep_script.py を使用した Docker イメージのビルドおよびプッシュ手順

以下は、指定された **Dockerfile** と **sleep_script.py** を使用して Docker イメージを作成し、**Docker Hub** にプッシュする手順です。

---

## ステップ 1: Dockerfile と sleep_script.py の準備

すでに Dockerfile と sleep_script.py を準備されているようです。  
この 2 つのファイルが **同じディレクトリにあること** を確認してください。

---

## ステップ 2: Docker イメージのビルド

1. **ターミナルを開く**  
   プロジェクトディレクトリ（Dockerfile と sleep_script.py が保存されている場所）に移動します。

2. **Docker イメージをビルド**  
   以下のコマンドを実行して、Dockerfile に基づいてイメージをビルドします。

   ```bash
   docker build -t <your-username>/sleep-script .
   ```

   > `<your-username>` は Docker Hub のユーザー名に置き換えてください。  
   > このコマンドにより、現在のディレクトリからコンテナがビルドされ、  
   > `<your-username>/sleep-script` という名前のイメージが作成されます。

---

## ステップ 3: Docker Hub にログイン

まだログインしていない場合、以下のコマンドで Docker Hub にログインします。

```bash
docker login
```

> ユーザー名とパスワードを入力するよう求められます。

---

## ステップ 4: Docker イメージのタグ付け

Docker Hub にプッシュする前に、イメージにタグを付けます。  
（すでに `docker build` 時にタグを指定している場合はこのステップは不要です。）

```bash
docker tag <your-username>/sleep-script <your-username>/sleep-script:v1
```

---

## ステップ 5: Docker Hub へイメージをプッシュ

次に、作成した Docker イメージを Docker Hub にプッシュします。

```bash
docker push <your-username>/sleep-script:v1
```

> これで Docker Hub にイメージがアップロードされます。

---

## ステップ 6: Docker Hub で確認

Docker Hub にログインし、作成したリポジトリ  
`<your-username>/sleep-script` にアクセスして、  
イメージが正常にアップロードされたことを確認します。

---

## （補足）Azure Container Registry（ACR）へプッシュする場合

もし Docker Hub ではなく Azure Container Registry (ACR) にプッシュする場合は、以下のコマンドを使用します。

```bash
az login
az acr login --name ntacrtest
docker push ntacrtest.azurecr.io/<your-namespace>/<your-image>
```

> `<your-namespace>` や `<your-image>` は環境に合わせて置き換えてください。

---

以上が、**Dockerfile** を使って **Docker イメージを作成し、Docker Hub（または ACR）にプッシュする手順**です。
