# Desafio Tecnico - Target Sistemas

Desafio tecnico em C# (.NET 8) com 3 exercicios.

## Requisitos

- .NET SDK 8

## Como rodar

### Exercicio 1 - Comissoes

Le o JSON de vendas e calcula a comissao de cada vendedor.

Regra: abaixo de R$100 sem comissao | abaixo de R$500 = 1% | a partir de R$500 = 5%

```bash
cd Exercicio1
dotnet run
```

### Exercicio 2 - Estoque

Menu para entrada/saida de mercadoria. Cada movimentacao gera um id, pede descricao e mostra o estoque final. Estoque e historico sao salvos a cada lancamento.

```bash
cd Exercicio2
dotnet run
```

### Exercicio 3 - Juros

Informa um valor, a data de vencimento e o tipo de juros (simples ou compostos). Multa de 2,5% ao dia.

```bash
cd Exercicio3
dotnet run
```

Data aceita nos formatos: `dd/MM/yyyy`, `dd.MM.yyyy` ou `dd-MM-yyyy`.
Juros limitados a 100% do valor original.
