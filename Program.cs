using System.Diagnostics;

namespace edCircularLinkedList;

internal static class Program
{
    private static readonly ComparadorInserciones comparador = new();
    private static List<Node> datosCsv = new();
    private static readonly string[] nombres = { "Lista circular", "LinkedList<Node>", "List<Node>" };
    private static readonly double[] tiempos = new double[3];

    static void Main()
    {
        Console.Title = "Comparación de estructuras en RAM - Videojuegos";
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("===============================================");
            Console.WriteLine("   LISTA DE VIDEOJUEGOS - MODO CONSOLA");
            Console.WriteLine("===============================================");
            Console.WriteLine("1. Agregar videojuego");
            Console.WriteLine("2. Mostrar elementos");
            Console.WriteLine("3. Buscar videojuego");
            Console.WriteLine("4. Verificar existencia");
            Console.WriteLine("5. Eliminar videojuego");
            Console.WriteLine("6. Contar elementos");
            Console.WriteLine("7. Importar CSV y comparar inserciones");
            Console.WriteLine("8. Mostrar registros importados");
            Console.WriteLine("9. Salir");
            Console.Write("\nOpción: ");
            string op = Console.ReadLine() ?? "";
            try
            {
                switch (op)
                {
                    case "1": AgregarManual(); break;
                    case "2": MostrarEstructura(); break;
                    case "3": Buscar(); break;
                    case "4": Existencia(); break;
                    case "5": Eliminar(); break;
                    case "6": Contar(); break;
                    case "7": ImportarYComparar(); break;
                    case "8": MostrarCsv(); break;
                    case "9": salir = true; continue;
                    default: Console.WriteLine("Opción no válida."); Pausa(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                Pausa();
            }
        }
    }

    private static void AgregarManual()
    {
        Console.Write("ID: "); int id = int.Parse(Console.ReadLine()!);
        Console.Write("Videojuego: "); string nombre = Console.ReadLine() ?? "";
        Console.Write("Género: "); string genero = Console.ReadLine() ?? "";
        Console.Write("Plataforma: "); string plataforma = Console.ReadLine() ?? "";
        bool agregado = comparador.Agregar(new Node(id, nombre, genero, plataforma, null));
        Console.WriteLine(agregado ? "Agregado correctamente." : "No se agregó: el ID ya existe.");
        Pausa();
    }

    private static void MostrarEstructura()
    {
        Console.WriteLine("\n1. Lista circular");
        Console.WriteLine("2. LinkedList<Node>");
        Console.WriteLine("3. List<Node>");
        Console.Write("Seleccione: ");
        int indice = int.Parse(Console.ReadLine()!) - 1;
        var lista = comparador.ObtenerElementos(indice);
        MostrarRegistros(lista, 50);
        Pausa();
    }

    private static void Buscar()
    {
        Console.Write("ID a buscar: "); int id = int.Parse(Console.ReadLine()!);
        Node? encontrado = comparador.Circular.Search(id);
        Console.WriteLine(encontrado is null ? "No encontrado." : $"Encontrado: {encontrado}");
        Pausa();
    }

    private static void Existencia()
    {
        Console.Write("ID: "); int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine(comparador.Circular.Exist(id) ? "El ID existe." : "El ID no existe.");
        Pausa();
    }

    private static void Eliminar()
    {
        Console.Write("ID a eliminar: "); int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine(comparador.Eliminar(id) ? "Eliminado de las tres estructuras." : "Ese ID no existe.");
        Pausa();
    }

    private static void Contar()
    {
        Console.WriteLine($"Lista circular: {comparador.Circular.Count()} elementos");
        Console.WriteLine($"LinkedList<Node>: {comparador.ObtenerElementos(1).Count} elementos");
        Console.WriteLine($"List<Node>: {comparador.ObtenerElementos(2).Count} elementos");
        Pausa();
    }

    private static void ImportarYComparar()
    {
        Console.Write("Ruta del CSV (ej. videojuegos.csv): ");
        string ruta = Console.ReadLine()?.Trim().Trim('"') ?? "";
        if (string.IsNullOrWhiteSpace(ruta)) return;

        Console.WriteLine("\nLeyendo CSV...");
        datosCsv = CsvService.Leer(ruta);
        Console.WriteLine($"Registros válidos: {datosCsv.Count:N0}");

        Console.WriteLine("\nInsertando los mismos registros en las tres estructuras...");

        // Medición real usando una estructura nueva por prueba.
        tiempos[0] = MedirCircular(datosCsv);
        tiempos[1] = MedirLinked(datosCsv);
        tiempos[2] = MedirList(datosCsv);

        Console.WriteLine("\n===============================================");
        Console.WriteLine("       RESULTADO DE LA COMPARACIÓN");
        Console.WriteLine("===============================================");
        Console.WriteLine($"Registros insertados: {datosCsv.Count:N0}");
        for (int i = 0; i < 3; i++)
            Console.WriteLine($"{nombres[i],-22}: {tiempos[i],12:F4} ms");
        int mejor = Array.IndexOf(tiempos, tiempos.Min());
        Console.WriteLine($"\nMejor tiempo: {nombres[mejor]}");
        Console.WriteLine("\nLos registros del CSV también quedan disponibles para mostrarlos con la opción 8.");
        Pausa();
    }

    private static double MedirCircular(List<Node> datos)
    {
        var lista = new CircularListLinked();
        var sw = Stopwatch.StartNew();
        foreach (var dato in datos) InsercionOrdenada.EnCircular(lista, dato);
        sw.Stop(); return sw.Elapsed.TotalMilliseconds;
    }

    private static double MedirLinked(List<Node> datos)
    {
        var lista = new LinkedList<Node>();
        var sw = Stopwatch.StartNew();
        foreach (var dato in datos) InsercionOrdenada.EnLinkedList(lista, dato);
        sw.Stop(); return sw.Elapsed.TotalMilliseconds;
    }

    private static double MedirList(List<Node> datos)
    {
        var lista = new List<Node>();
        var sw = Stopwatch.StartNew();
        foreach (var dato in datos) InsercionOrdenada.EnList(lista, dato);
        sw.Stop(); return sw.Elapsed.TotalMilliseconds;
    }

    private static void MostrarCsv()
    {
        if (datosCsv.Count == 0) { Console.WriteLine("Primero importa un CSV."); Pausa(); return; }
        Console.WriteLine($"\nREGISTROS DEL CSV ({datosCsv.Count:N0})");
        MostrarRegistros(datosCsv, 100);
        Pausa();
    }

    private static void MostrarRegistros(IEnumerable<Node> registros, int maximo)
    {
        Console.WriteLine("{0,-8} {1,-35} {2,-20} {3,-15}", "ID", "VIDEOJUEGO", "GÉNERO", "PLATAFORMA");
        Console.WriteLine(new string('-', 82));
        int contador = 0;
        foreach (var n in registros)
        {
            if (contador++ >= maximo) break;
            Console.WriteLine("{0,-8} {1,-35} {2,-20} {3,-15}",
                n.Id, Cortar(n.Nombre, 35), Cortar(n.Genero, 20), Cortar(n.Plataforma, 15));
        }
        if (contador <= maximo) Console.WriteLine($"\nMostrados: {contador:N0}");
        else Console.WriteLine($"\nMostrados: {maximo:N0}. Total: más registros disponibles.");
    }

    private static string Cortar(string texto, int max) => texto.Length <= max ? texto : texto[..(max - 3)] + "...";
    private static void Pausa() { Console.WriteLine("\nPresiona ENTER para continuar..."); Console.ReadLine(); }
}
