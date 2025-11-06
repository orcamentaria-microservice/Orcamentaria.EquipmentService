# 🧩 Orcamentaria Microservice Template

Template oficial para criação de **microserviços padronizados** do ecossistema **Orcamentaria**, utilizando arquitetura em camadas e bibliotecas compartilhadas (`Orcamentaria.Lib.*`).

---

## 🚀 Estrutura Gerada

Ao criar um novo microserviço, o template monta automaticamente a seguinte estrutura:

```
Orcamentaria.EquipamentService/
 ├── Orcamentaria.EquipamentService.API/
 ├── Orcamentaria.EquipamentService.Application/
 ├── Orcamentaria.EquipamentService.Domain/
 ├── Orcamentaria.EquipamentService.Infrastructure/
 └── Orcamentaria.EquipamentService.sln
```

Cada camada já vem preparada com pastas padrão (`Controllers`, `Services`, `Repositories`, etc.) e referências configuradas.

---

## 🧱 Instalação do Template

1. No terminal, entre na pasta onde está o template:

   ```bash
   cd C:\Users\<seu-usuario>\Documents\Orcamentaria.Template.Service
   ```

2. Instale o template no .NET CLI:

   ```bash
   dotnet new install .
   ```

   > ⚙️ Isso registra o template localmente, permitindo criar novos microserviços com o comando `dotnet new orcamentaria-microservice`.

3. Para atualizar o template após alterações:

   ```bash
   dotnet new uninstall .
   dotnet new install .
   ```

---

## 🏗️ Criação de um Novo Microserviço

Crie uma nova pasta para seus projetos e execute:

```bash
dotnet new orcamentaria-microservice -n PersonService
```

🔹 Isso criará a estrutura:

```
Orcamentaria.PersonService/
 ├── Orcamentaria.PersonService.API/
 ├── Orcamentaria.PersonService.Application/
 ├── Orcamentaria.PersonService.Domain/
 ├── Orcamentaria.PersonService.Infrastructure/
 └── Orcamentaria.PersonService.sln
```

---

## 📦 Dependências Automáticas

Cada camada já referencia as bibliotecas compartilhadas, sempre na **última versão** publicada:

```xml
<PackageReference Include="Orcamentaria.Lib.Domain" Version="*" />
<PackageReference Include="Orcamentaria.Lib.Application" Version="*" />
<PackageReference Include="Orcamentaria.Lib.Infrastructure" Version="*" />
```

> O `"Version='*'"` garante que sempre será instalada a versão mais recente do pacote.

---

## 🔄 Atualizando Libs

Para garantir que as dependências estão atualizadas:

```bash
dotnet restore --no-cache
```

---

## 💡 Dicas

- Para testar se o template foi instalado:
  ```bash
  dotnet new list | findstr orcamentaria
  ```
  Deve exibir algo como:
  ```
  Orcamentaria Microservice (C#)
  Short name: orcamentaria-microservice
  ```

- Se quiser remover:
  ```bash
  dotnet new uninstall Orcamentaria.MicroserviceTemplate
  ```

- Se quiser distribuir o template (por exemplo, via NuGet):
  ```bash
  dotnet pack -o ./nupkgs
  ```

---

## ✨ Autor

**Marcelo Fernando**  
Desenvolvedor Fullstack | Arquitetura de Microserviços