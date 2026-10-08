using System.Globalization;

namespace edCircularLinkedList;

internal static class CsvService
{
    public static List<Node> Leer(string ruta)
    {
        if (!File.Exists(ruta))
            throw new FileNotFoundException("No se encontró el archivo CSV.", ruta);

        var datos = new List<Node>();
        var ids = new HashSet<int>();
        string[] lineas = File.ReadAllLines(ruta);
        if (lineas.Length == 0) return datos;

        char separador = DetectarSeparador(lineas[0]);
        int inicio = 0;
        var primera = ParsearLinea(lineas[0], separador);
        if (primera.Count < 4 || !int.TryParse(primera[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            inicio = 1;

        for (int i = inicio; i < lineas.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            var campos = ParsearLinea(lineas[i], separador);
            if (campos.Count < 4) continue;
            if (!int.TryParse(campos[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int id)) continue;
            if (!ids.Add(id)) throw new InvalidOperationException($"El CSV contiene el ID duplicado {id}.");

            datos.Add(new Node(id, campos[1].Trim(), campos[2].Trim(), campos[3].Trim(), null));
        }
        return datos;
    }

    private static char DetectarSeparador(string linea)
    {
        int comas = linea.Count(c => c == ',');
        int puntos = linea.Count(c => c == ';');
        return puntos > comas ? ';' : ',';
    }

    private static List<string> ParsearLinea(string linea, char separador)
    {
        var campos = new List<string>();
        var actual = new System.Text.StringBuilder();
        bool comillas = false;
        for (int i = 0; i < linea.Length; i++)
        {
            char c = linea[i];
            if (c == '"')
            {
                if (comillas && i + 1 < linea.Length && linea[i + 1] == '"') { actual.Append('"'); i++; }
                else comillas = !comillas;
            }
            else if (c == separador && !comillas) { campos.Add(actual.ToString()); actual.Clear(); }
            else actual.Append(c);
        }
        campos.Add(actual.ToString());
        return campos;
    }
}
