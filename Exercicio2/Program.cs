using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static int proximoId = 1;
    static List<Movimentacao> historico = new List<Movimentacao>();
    static string caminhoHistorico = "dados/historico.json";

    static void Main()
    {
        string caminho = "dados/estoque.json";

        if (!File.Exists(caminho))
        {
            Console.WriteLine("Arquivo de estoque nao encontrado.");
            return;
        }

        try
        {
            string json = File.ReadAllText(caminho);

            var opcoes = new JsonSerializerOptions();
            opcoes.PropertyNameCaseInsensitive = true;

            var dados = JsonSerializer.Deserialize<ListaEstoque>(json, opcoes);

            if (dados == null || dados.Estoque == null || dados.Estoque.Count == 0)
            {
                Console.WriteLine("Nenhum produto encontrado no arquivo.");
                return;
            }

            CarregarHistorico();

            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine();
                Console.WriteLine("=== Estoque ===");
                Console.WriteLine("1 - Listar produtos");
                Console.WriteLine("2 - Entrada de mercadoria");
                Console.WriteLine("3 - Saida de mercadoria");
                Console.WriteLine("4 - Ver historico");
                Console.WriteLine("0 - Sair");
                Console.Write("Opcao: ");

                string opcao = Console.ReadLine();

                if (opcao == "1")
                    ListarProdutos(dados.Estoque);
                else if (opcao == "2")
                    Movimentar(dados, caminho, "ENTRADA");
                else if (opcao == "3")
                    Movimentar(dados, caminho, "SAIDA");
                else if (opcao == "4")
                    ListarHistorico();
                else if (opcao == "0")
                    continuar = false;
                else
                    Console.WriteLine("Opcao invalida.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao processar o estoque: " + ex.Message);
        }
    }

    static void CarregarHistorico()
    {
        if (!File.Exists(caminhoHistorico))
            return;

        string json = File.ReadAllText(caminhoHistorico);

        var opcoes = new JsonSerializerOptions();
        opcoes.PropertyNameCaseInsensitive = true;

        var lista = JsonSerializer.Deserialize<List<Movimentacao>>(json, opcoes);

        if (lista == null || lista.Count == 0)
            return;

        historico = lista;

        int maiorId = 0;
        foreach (var m in historico)
        {
            if (m.Id > maiorId)
                maiorId = m.Id;
        }

        proximoId = maiorId + 1;
    }

    static void ListarProdutos(List<Produto> produtos)
    {
        Console.WriteLine();
        Console.WriteLine("Codigo | Produto | Estoque");
        Console.WriteLine("---------------------------");

        foreach (var p in produtos)
        {
            Console.WriteLine(p.CodigoProduto + " | " + p.DescricaoProduto + " | " + p.Estoque);
        }
    }

    static void ListarHistorico()
    {
        Console.WriteLine();

        if (historico.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentacao registrada.");
            return;
        }

        Console.WriteLine("Historico de movimentacoes:");
        Console.WriteLine();

        foreach (var m in historico)
        {
            Console.WriteLine("Id " + m.Id + " | " + m.Tipo + " | " + m.Produto + " | Qtd: " + m.Quantidade + " | Estoque final: " + m.EstoqueFinal + " | " + m.Descricao);
        }
    }

    static void Movimentar(ListaEstoque dados, string caminho, string tipo)
    {
        Console.Write("Codigo do produto: ");
        string codigoTexto = Console.ReadLine();

        int codigo;
        if (!int.TryParse(codigoTexto, out codigo))
        {
            Console.WriteLine("Codigo invalido.");
            return;
        }

        Produto produto = null;

        foreach (var p in dados.Estoque)
        {
            if (p.CodigoProduto == codigo)
            {
                produto = p;
                break;
            }
        }

        if (produto == null)
        {
            Console.WriteLine("Produto nao encontrado.");
            return;
        }

        Console.Write("Quantidade: ");
        string qtdTexto = Console.ReadLine();

        int quantidade;
        if (!int.TryParse(qtdTexto, out quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Quantidade invalida. Informe um numero maior que zero.");
            return;
        }

        if (tipo == "SAIDA" && quantidade > produto.Estoque)
        {
            Console.WriteLine("Estoque insuficiente. Disponivel: " + produto.Estoque);
            return;
        }

        Console.Write("Descricao da movimentacao: ");
        string descricao = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(descricao))
        {
            Console.WriteLine("Descricao obrigatoria.");
            return;
        }

        if (tipo == "ENTRADA")
            produto.Estoque += quantidade;
        else
            produto.Estoque -= quantidade;

        int id = proximoId;
        proximoId++;

        historico.Add(new Movimentacao
        {
            Id = id,
            Tipo = tipo,
            Descricao = descricao,
            Produto = produto.DescricaoProduto,
            Quantidade = quantidade,
            EstoqueFinal = produto.Estoque
        });

        SalvarJson(caminho, dados);
        SalvarJson(caminhoHistorico, historico);

        Console.WriteLine();
        Console.WriteLine("--- Movimentacao realizada ---");
        Console.WriteLine("Id: " + id);
        Console.WriteLine("Tipo: " + tipo);
        Console.WriteLine("Descricao: " + descricao);
        Console.WriteLine("Produto: " + produto.DescricaoProduto);
        Console.WriteLine("Estoque final: " + produto.Estoque);
    }

    static void SalvarJson(string caminho, object dados)
    {
        var opcoes = new JsonSerializerOptions();
        opcoes.WriteIndented = true;

        string json = JsonSerializer.Serialize(dados, opcoes);
        string temp = caminho + ".tmp";

        File.WriteAllText(temp, json);

        if (File.Exists(caminho))
            File.Delete(caminho);

        File.Move(temp, caminho);
    }
}

class ListaEstoque
{
    public List<Produto> Estoque { get; set; }
}

class Produto
{
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; }
    public int Estoque { get; set; }
}

class Movimentacao
{
    public int Id { get; set; }
    public string Tipo { get; set; }
    public string Descricao { get; set; }
    public string Produto { get; set; }
    public int Quantidade { get; set; }
    public int EstoqueFinal { get; set; }
}
