using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Calculo de juros ===");
        Console.WriteLine();

        Console.Write("Valor: R$ ");
        string valorTexto = Console.ReadLine();

        decimal valor;
        if (!decimal.TryParse(valorTexto, NumberStyles.Number, new CultureInfo("pt-BR"), out valor))
        {
            Console.WriteLine("Valor invalido.");
            return;
        }

        if (valor <= 0)
        {
            Console.WriteLine("O valor precisa ser maior que zero.");
            return;
        }

        Console.Write("Data de vencimento (dd/MM/yyyy): ");
        string dataTexto = Console.ReadLine();

        string[] formatos = { "dd/MM/yyyy", "dd.MM.yyyy", "dd-MM-yyyy" };

        DateTime vencimento;
        if (!DateTime.TryParseExact(dataTexto, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out vencimento))
        {
            Console.WriteLine("Data invalida. Use dd/MM/yyyy, dd.MM.yyyy ou dd-MM-yyyy.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Tipo de juros:");
        Console.WriteLine("1 - Simples");
        Console.WriteLine("2 - Compostos");
        Console.Write("Opcao: ");
        string tipo = Console.ReadLine();

        if (tipo != "1" && tipo != "2")
        {
            Console.WriteLine("Opcao invalida.");
            return;
        }

        DateTime hoje = DateTime.Today;
        int diasAtraso = (hoje - vencimento).Days;

        decimal multaDia = 0.025m;
        decimal juros = 0;
        decimal total = valor;
        bool atingiuLimite = false;
        string nomeTipo = tipo == "1" ? "simples" : "compostos";

        if (diasAtraso > 0)
        {
            if (tipo == "1")
            {
                juros = valor * multaDia * diasAtraso;
            }
            else
            {
                double montante = (double)valor * Math.Pow(1 + (double)multaDia, diasAtraso);
                juros = (decimal)montante - valor;
            }

            if (juros > valor)
            {
                juros = valor;
                atingiuLimite = true;
            }

            total = valor + juros;
        }

        Console.WriteLine();
        Console.WriteLine("--- Resultado ---");
        Console.WriteLine("Valor original: R$ " + valor.ToString("F2"));
        Console.WriteLine("Vencimento: " + vencimento.ToString("dd/MM/yyyy"));
        Console.WriteLine("Data de hoje: " + hoje.ToString("dd/MM/yyyy"));
        Console.WriteLine("Tipo de juros: " + nomeTipo);

        if (diasAtraso <= 0)
        {
            Console.WriteLine("Situacao: em dia (sem juros)");
            Console.WriteLine("Dias em atraso: 0");
            Console.WriteLine("Juros: R$ 0,00");
            Console.WriteLine("Total a pagar: R$ " + valor.ToString("F2"));
        }
        else
        {
            Console.WriteLine("Situacao: em atraso");
            Console.WriteLine("Dias em atraso: " + diasAtraso);
            Console.WriteLine("Multa: 2,5% ao dia");
            Console.WriteLine("Juros: R$ " + juros.ToString("F2"));

            if (atingiuLimite)
                Console.WriteLine("Obs: juros limitados a 100% do valor original.");

            Console.WriteLine("Total a pagar: R$ " + total.ToString("F2"));
        }
    }
}
