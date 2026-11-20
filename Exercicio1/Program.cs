using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string caminho = "dados/vendas.json";

        if (!File.Exists(caminho))
        {
            Console.WriteLine("Arquivo de vendas nao encontrado.");
            return;
        }

        try
        {
            string json = File.ReadAllText(caminho);

            var opcoes = new JsonSerializerOptions();
            opcoes.PropertyNameCaseInsensitive = true;

            var dados = JsonSerializer.Deserialize<ListaVendas>(json, opcoes);

            if (dados == null || dados.Vendas == null || dados.Vendas.Count == 0)
            {
                Console.WriteLine("Nenhuma venda encontrada no arquivo.");
                return;
            }

            var totais = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            foreach (var venda in dados.Vendas)
            {
                string vendedor = venda.Vendedor == null ? "" : venda.Vendedor.Trim();

                if (string.IsNullOrWhiteSpace(vendedor))
                {
                    Console.WriteLine("Venda ignorada: vendedor sem nome.");
                    continue;
                }

                if (venda.Valor < 0)
                {
                    Console.WriteLine("Venda ignorada: valor negativo (" + vendedor + ").");
                    continue;
                }

                decimal comissao = 0;

                if (venda.Valor < 100)
                    comissao = 0;
                else if (venda.Valor < 500)
                    comissao = venda.Valor * 0.01m;
                else
                    comissao = venda.Valor * 0.05m;

                if (totais.ContainsKey(vendedor))
                    totais[vendedor] += comissao;
                else
                    totais.Add(vendedor, comissao);
            }

            Console.WriteLine("Comissao por vendedor:");
            Console.WriteLine();

            foreach (var item in totais)
            {
                Console.WriteLine(item.Key + " - R$ " + item.Value.ToString("F2"));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao processar as vendas: " + ex.Message);
        }
    }
}

class ListaVendas
{
    public List<Venda> Vendas { get; set; }
}

class Venda
{
    public string Vendedor { get; set; }
    public decimal Valor { get; set; }
}
