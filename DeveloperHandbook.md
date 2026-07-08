# Developer Handbook for Code Agents

Este guia e direcionado a agentes de codigo trabalhando no projeto BarberBoss. Use-o para continuar incrementando o sistema respeitando os padroes de codigo e arquitetura ja existentes.

## Principio principal

Preserve o desenho atual do projeto. Antes de implementar, leia o codigo ao redor e copie os padroes locais. Prefira mudancas pequenas, bem verificadas e alinhadas com a estrutura existente a refatoracoes amplas.

Quando o pedido do usuario for especifico, trate o escopo literalmente. Nao aproveite a tarefa para renomear, reorganizar ou "melhorar" partes nao relacionadas.

## Estrutura do projeto

O projeto esta organizado em camadas:

- `src/BarberBoss.Api`: controllers, filtros HTTP, configuracao do pipeline, Swagger, autenticacao e provider do token recebido por HTTP.
- `src/BarberBoss.Communication`: DTOs de entrada e saida, como `Request...Json` e `Response...Json`.
- `src/BarberBoss.Application`: casos de uso, validadores, AutoMapper e injecao de dependencia da camada de aplicacao.
- `src/BarberBoss.Domain`: entidades, enums, contratos de repositorio, contratos de servicos, contratos de seguranca e regras de dominio compartilhadas.
- `src/BarberBoss.Infrastructure`: EF Core, DbContext, repositorios concretos, Unit of Work, criptografia, JWT, usuario logado e migrations.
- `src/BarberBoss.Exception`: excecoes base e mensagens centralizadas de erro.
- `tests/Validator.Tests`: testes xUnit de validadores, casos de uso e suporte com builders, doubles e mapper de teste.

## Fluxo recomendado antes de alterar codigo

1. Rode `git status --short` e identifique alteracoes existentes. Nao reverta nem sobrescreva trabalho que ja esteja no workspace.
2. Localize exemplos semelhantes com `rg`. Para novas features, procure por um fluxo existente equivalente em Billing, User ou Login.
3. Abra os arquivos proximos ao ponto de mudanca. Nao assuma padrao por memoria quando o codigo atual for facil de verificar.
4. Implemente no menor conjunto de arquivos possivel.
5. Rode validacao estreita primeiro, depois amplie se necessario:
   - `dotnet build`
   - `dotnet test`
   - Para problemas de build por DLL bloqueada, use uma validacao com output temporario: `dotnet build -p:BaseOutputPath=.artifacts\build\`

## Padroes de Application

Casos de uso ficam em `src/BarberBoss.Application/UseCases/<Area>/<Action>`.

Cada caso de uso normalmente tem:

- Uma interface `I<Action><Area>UseCase`.
- Uma classe concreta `<Action><Area>UseCase`.
- Dependencias recebidas por construtor.
- Metodo `Execute(...)` assincrono.
- Validacao no inicio quando ha request de entrada.
- Excecoes de dominio/aplicacao usando tipos de `BarberBoss.Exception.ExceptionsBase`.
- `IUnitOfWork.Commit()` apenas em operacoes que alteram dados.

Mantenha validadores com FluentValidation perto dos casos de uso. Para Billing, o padrao atual e `BillingValidator`; para User, ha validadores especificos como `RegisterUserValidator` e `PasswordValidator`.

Quando um validator usa `ResourceErrorMessages.<KEY>`, confirme que a chave existe no wrapper C# e no `.resx`.

## Padroes de Communication

DTOs ficam em `src/BarberBoss.Communication`.

Use:

- `Requests/<Area>/Request...Json` para entrada.
- `Responses/<Area>/Response...Json` para saida.

DTOs nao devem conter regra de negocio. Eles devem ser simples, com propriedades publicas e valores padrao quando o padrao local ja usa isso, por exemplo `string.Empty` para strings.

Quando um request receber valores como string mas a entidade usa enum, deixe a conversao explicita no AutoMapper e mantenha a validacao antes do mapeamento. Exemplo atual: `RequestBillingJson.PaymentMethod` e `RequestBillingJson.Status` sao strings, mas `Billing.PaymentMethod` e `Billing.Status` sao enums.

## Padroes de AutoMapper

O profile central fica em `src/BarberBoss.Application/AutoMapper/AutoMapping.cs`.

Adicione maps em dois grupos:

- `RequestToEntity()` para entrada -> entidade.
- `EntityToResponse()` para entidade -> resposta.

Se a conversao nao for trivial ou puder ficar ambigua, configure `.ForMember(...)` explicitamente. Nao dependa de conversoes implicitas quando a request e a entidade usam tipos diferentes.

## Padroes de Domain

A camada Domain deve conter contratos e modelos centrais, nao implementacoes de infraestrutura.

Use:

- `Entities` para entidades como `Billing` e `User`.
- `Enums` para enums como `BillingPaymentMethod`, `BillingStatus` e `UserRole`.
- `Repositories` para interfaces de persistencia.
- `Security` para contratos de criptografia e token.
- `Services` para contratos como `ILoggedUser`.

Para repositorios, siga a segmentacao existente:

- `I...ReadOnlyRepository` para consultas.
- `I...WriteOnlyRepository` para criacao/delecao.
- `I...UpdateOnlyRepository` para atualizacao.

Nao coloque EF Core, JWT concreto, BCrypt concreto ou acesso a HTTP dentro de Domain.

## Padroes de Infrastructure

Implementacoes concretas ficam em `src/BarberBoss.Infrastructure`.

O arquivo `DependencyInjectionExtension.cs` registra:

- DbContext com `UseNpgsql`.
- Repositorios concretos.
- `IUnitOfWork`.
- Criptografia.
- Geracao de token.
- Usuario logado.

Repositorios EF devem seguir estes padroes:

- Consultas somente leitura usam `AsNoTracking()`.
- Consultas para update retornam entidade rastreada.
- `Add` apenas adiciona no DbContext; quem confirma e o `IUnitOfWork`.
- `Update` marca/atualiza a entidade.
- `Delete` retorna `bool` quando o caso de uso precisa decidir se lanca not found.

## Unit of Work

`IUnitOfWork` fica em `BarberBoss.Domain.Repositories`.

`UnitOfWork` fica em `BarberBoss.Infrastructure.DataAccess`.

Casos de uso que gravam dados devem chamar `Commit()` apos operar no repositorio. Nao chame `SaveChangesAsync()` diretamente nos casos de uso ou nos repositorios, a menos que o padrao do projeto mude explicitamente.

## Padroes de Api

Controllers ficam em `src/BarberBoss.Api/Controllers`.

O padrao atual e:

- Receber o use case com `[FromServices]`.
- Receber requests com `[FromBody]`.
- Receber ids ou filtros pela rota/query conforme o endpoint.
- Retornar `Created`, `Ok`, `NoContent` ou outro resultado HTTP apropriado.
- Delegar regra de negocio ao caso de uso.

O tratamento de erro deve continuar centralizado em `ExceptionFilter`. Nao duplique try/catch de erro de negocio nos controllers se a excecao ja for tratada pelo filtro.

Endpoints autenticados devem usar o pipeline existente com JWT, `ITokenProvider` e `ILoggedUser`.

## Excecoes e mensagens

Mensagens ficam centralizadas em:

- `src/BarberBoss.Exception/ResourceErrorMessages.cs`
- `src/BarberBoss.Exception/ExceptionsBase/ResourceErrorMessages.pt-BR.resx`

Este projeto usa um wrapper manual `ResourceErrorMessages`, nao um designer gerado automaticamente. Ao adicionar uma mensagem:

1. Adicione a entrada no `.resx`.
2. Adicione a propriedade correspondente em `ResourceErrorMessages.cs`.
3. Use a propriedade no validator, use case ou exception.
4. Rode build ou teste do projeto afetado.

Excecoes de negocio devem herdar de `BarberBossException` e expor status code e lista de erros via `GetErrors()`.

## Autenticacao e usuario logado

JWT e configurado em `BarberBoss.Api/Program.cs` e registrado em Infrastructure.

Use os contratos existentes:

- `IAccessTokenGenerator` para gerar token.
- `ITokenProvider` para ler token da requisicao HTTP.
- `ILoggedUser` para obter o usuario autenticado.
- `IPasswordEncripter` para hash/verificacao de senha.

Nao leia diretamente `HttpContext` em Application. Se um caso de uso precisa do usuario atual, injete `ILoggedUser`.

## EF Core e migrations

Migrations ficam em `src/BarberBoss.Infrastructure/Migrations`.

Comando padrao para criar migration:

```powershell
dotnet ef migrations add NomeDaMigration --project src\BarberBoss.Infrastructure --startup-project src\BarberBoss.Api
```

Depois de gerar migration:

1. Inspecione o arquivo `.cs` da migration.
2. Inspecione o `BarberBossDbContextModelSnapshot`.
3. Confirme se a migration representa a mudanca pretendida. Renames devem aparecer como `RenameColumn` quando esse for o objetivo, nao como drop/create acidental.
4. Rode build.

Se a API estiver rodando e bloqueando DLLs no output padrao, gere com `--no-build` depois de ter uma build valida:

```powershell
dotnet ef migrations add NomeDaMigration --project src\BarberBoss.Infrastructure --startup-project src\BarberBoss.Api --no-build
```

Para validar sem bater no output bloqueado:

```powershell
dotnet build -p:BaseOutputPath=.artifacts\build\
```

## Testes

Testes ficam em `tests/Validator.Tests`.

Siga o padrao AAA:

- Arrange
- Act
- Assert

Use a arvore de suporte existente:

- `Support/Builders` para criar objetos de teste.
- `Support/Repositories` para doubles de repositorio.
- `Support/Services` para doubles de servicos.
- `Support/AutoMapper/TestMapper.cs` para usar o profile real de AutoMapper.

Ao adicionar ou alterar caso de uso, cubra pelo menos:

- Caminho feliz.
- Validacao invalida, quando houver request.
- Not found, quando houver busca por id.
- Commit chamado em sucesso de escrita.
- Commit nao chamado em falha.
- Mapeamentos relevantes quando houver conversao de tipo.

Nao conecte testes unitarios a banco real, HTTP real ou token real se um double simples resolve.

## Como adicionar uma nova funcionalidade

Use este roteiro e adapte ao menor escopo possivel:

1. Crie ou ajuste DTOs em `Communication`.
2. Crie/ajuste entidade, enum ou contrato em `Domain` somente se necessario.
3. Crie/ajuste interfaces de repositorio em `Domain.Repositories`.
4. Implemente persistencia em `Infrastructure.DataAccess.Repositories`.
5. Registre dependencias em `Infrastructure.DependencyInjectionExtension`.
6. Crie validator e use case em `Application.UseCases`.
7. Registre o use case em `Application.DependencyInjectionExtension`.
8. Adicione mapeamentos em `AutoMapping`.
9. Exponha endpoint no controller em `Api`.
10. Adicione mensagens em `ResourceErrorMessages` e `.resx` se houver nova validacao/erro.
11. Adicione testes com builders/doubles.
12. Rode `dotnet test`.

Nem toda tarefa precisa de todos esses passos. Se a mudanca for so em validator, mapper ou teste, mantenha o patch nesse limite.

## Regras praticas para agentes

Faca:

- Leia o codigo existente antes de editar.
- Use `rg` para localizar padroes.
- Preserve nomes, organizacao e estilo atuais.
- Prefira validacao centralizada e mensagens centralizadas.
- Use interfaces existentes em vez de acoplar camadas.
- Verifique build/test antes de finalizar quando possivel.
- Explique no final o que mudou e qual comando foi rodado.

Evite:

- Refatorar arquitetura sem pedido explicito.
- Mover arquivos apenas por preferencia.
- Criar abstracoes novas sem necessidade real.
- Duplicar mensagens de erro hardcoded.
- Chamar DbContext diretamente em Application.
- Colocar regra de negocio em controller.
- Ignorar alteracoes existentes no worktree.
- Reverter arquivos que o usuario pode ter alterado.

## Checklist de finalizacao

Antes de entregar:

- `git status --short` foi consultado.
- O patch ficou dentro do escopo pedido.
- Novas mensagens existem no wrapper e no `.resx`.
- Novos use cases estao registrados no DI.
- Novos mapeamentos estao no profile central.
- Operacoes de escrita usam `IUnitOfWork`.
- Testes relevantes foram criados ou ajustados.
- `dotnet test` ou uma validacao mais estreita foi rodada.
- Qualquer falha, warning ou limitacao foi informada claramente.
