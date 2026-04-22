# GoodHamburger.Api

API REST para gerenciamento de pedidos da lanchonete **Good Hamburger**, desenvolvida em C# com .NET 8 e ASP.NET Core.

## Como executar
### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022+ ou VS Code
- Git

### Clonando o repositório
```bash 
git clone https://github.com/devmisa/GoodHamburger.Api.git
cd GoodHamburger.Api
```

### Executando a API

```bash 
cd GoodHamburger.Api
dotnet run
```
A aplicação abrirá automaticamente no Swagger:

- **HTTP:** http://localhost:5151/swagger/index.html
- **HTTPS:** https://localhost:7241/swagger/index.html

> O banco de dados SQLite (`goodhamburger.db`) é criado automaticamente na primeira execução. Nenhuma configuração adicional é necessária.

### Executando os testes

```bash
dotnet test
```

---

## Endpoints

### Cardápio

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/goodhamburger/menu` | Lista todos os itens do cardápio |

### Pedidos

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/goodhamburger` | Consulta todos os pedidos |
| `GET` | `/api/goodhamburger/{id}` | Consulta pedido por ID |
| `POST` | `/api/goodhamburger` | Cria um novo pedido |
| `PUT` | `/api/goodhamburger/{id}` | Atualiza um pedido existente |
| `DELETE` | `/api/goodhamburger/{id}` | Remove um pedido |

### Exemplo de corpo (criar/atualizar)
```
{
   "sandwich":{
      "type":1
   },
   "includeFrenchFries":true,
   "includeSoda":false
}
```

**Tipos de sanduíche (`type`):**

| Valor | Sanduíche | Preço |
|-------|-----------|-------|
| `1` | X Burger | R$ 5,00 |
| `2` | X Egg | R$ 4,50 |
| `3` | X Bacon | R$ 7,00 |

### Exemplo de resposta
```{
   "id":1,
   "sandwich":{
      "type":1
   },
   "includeFrenchFries":true,
   "includeSoda":true,
   "subtotal":9.50,
   "discount":1.90,
   "totalPrice":7.60
}
```


---

## Regras de desconto

| Combinação | Desconto |
|------------|----------|
| Sanduíche + Batata Frita + Refrigerante | 20% |
| Sanduíche + Refrigerante | 15% |
| Sanduíche + Batata Frita | 10% |
| Apenas Sanduíche | Sem desconto |

---

## Arquitetura e decisões técnicas

### Estrutura de projetos

```
GoodHamburger.Api/
├── GoodHamburger.Api/            - Camada de apresentação (Controllers, Middleware)
├── GoodHamburger.Application/    - Camada de aplicação (Services, DTOs, Validações, Mapeamentos)
├── GoodHamburger.Domain/         - Camada de domínio (Entidades, Enums, Constantes)
├── GoodHamburger.Infrastructure/ - Camada de infraestrutura (DbContext, Repositórios)
└── GoodHamburger.UnitTests/      - Testes unitários
```

A solução segue a **Clean Architecture** com separação clara de responsabilidades:

- **Domain**: entidades de negócio (`Menu`, `Sandwich`) com regras de cálculo encapsuladas. `Subtotal`, `Discount` e `TotalPrice` são calculados em tempo de execução, sem persistência.
- **Application**: orquestra os casos de uso via `GoodHamburgerService`, expõe DTOs para isolar o domínio e usa **FluentValidation** para validação de entrada.
- **Infrastructure**: repositório com **EF Core + SQLite**. `Sandwich` mapeada como *owned entity* de `Menu`. Logging estruturado nas operações de banco.
- **Api**: controllers REST finos com tratamento centralizado de erros via middleware.

### Decisões técnicas

| Decisão | Justificativa |
|---------|---------------|
| **SQLite** | Banco leve, sem instalação, ideal para demonstração local |
| **EF Core com `EnsureCreated`** | Setup simplificado sem migrations manuais |
| **Owned Entity para `Sandwich`** | Modela composição sem tabela separada desnecessária |
| **Propriedades calculadas no domínio** | Coesão e ausência de lógica de negócio duplicada fora do domínio |
| **FluentValidation** | Validações declarativas, legíveis e extensíveis |
| **GlobalExceptionHandler** | Tratamento centralizado de erros inesperados via `IExceptionHandler` do ASP.NET Core evita `try/catch` espalhado pelas camadas |
| **Serilog** | Logging estruturado no console com níveis configuráveis via `appsettings.json` |
| **Moq + xUnit** | Stack padrão amplamente adotada para testes unitários em .NET |

---
## Testes

| Classe | Camada | O que cobre |
|--------|--------|-------------|
| `MenuTests` | Domain | `Subtotal`, `TotalPrice` e `Discount` para todos os cenários de desconto; preços de cada sanduíche |
| `GoodHamburgerServiceTests` | Application | Todos os métodos do serviço com repositório mockado via **Moq** — fluxos de sucesso e recurso não encontrado |
| `GoodHamburgerRepositoryTests` | Infrastructure | CRUD completo do repositório com **EF Core InMemory** — persistência, atualização, remoção e consulta |

---
##  Tecnologias utilizadas

| Pacote | Uso |
|--------|-----|
| .NET 8 / ASP.NET Core | Framework principal |
| Entity Framework Core 8 + SQLite | Persistência de dados |
| FluentValidation | Validação de entrada |
| Serilog + Serilog.AspNetCore | Logging estruturado |
| xUnit + Moq | Testes unitários |
| EF Core InMemory | Banco em memória para testes de repositório |
| Swashbuckle / Swagger | Documentação da API |

---
## Decisões de modelagem

### Por que `bool` para batata frita e refrigerante?

A regra de negócio define que cada pedido pode conter **no máximo um** de cada item, ou tem ou não tem. A pergunta é binária, então `bool` é o tipo correto.

- `int` exigiria validação manual contra duplicidade
- `bool` torna a duplicidade impossível por natureza
- As regras de desconto ficam diretas: `if (IncludeFrenchFries && IncludeSoda)`

## O que ficou fora

- **Frontend em Blazor** diferencial opcional não implementado nesta entrega.
- **Migrations versionadas** optou-se por `EnsureCreated`; em produção se fosse o caso seria utilizado `dotnet ef migrations`.
