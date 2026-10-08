🎮 Lista Circular Videojuegos (Consola)

Este proyecto es una aplicación de consola desarrollada en C# que implementa una Lista Circular Simplemente Enlazada desde cero. Su objetivo principal es gestionar un catálogo de videojuegos cargado a través de archivos CSV y realizar mediciones de rendimiento durante el proceso de inserción de datos.

🚀 Características Principales
Estructura de Datos Propia: Implementación manual de una lista circular enlazada (CircularListLinked).
Procesamiento de Archivos: Lectura y extracción de datos de videojuegos desde archivos CSV (CsvService).
Análisis de Rendimiento: Herramienta para medir y comparar los tiempos de ejecución y la eficiencia de las inserciones en la estructura de datos (ComparadorInserciones).

📁 Estructura del Proyecto
Program.cs: Punto de entrada de la aplicación de consola. Coordina la carga de datos y las pruebas de rendimiento.
Node.cs: Define el nodo base para la estructura de datos. Contiene la información del videojuego y el puntero al siguiente nodo.
CircularListLinked.cs: Lógica principal de la lista circular (inserción, recorrido, búsqueda, eliminación).
CsvService.cs: Encargado de leer, parsear y convertir las líneas del CSV en objetos utilizables por la lista.
ComparadorInserciones.cs: Módulo diseñado para cronometrar las inserciones y generar métricas de rendimiento.

🛠️ Tecnologías y Requisitos
Lenguaje: C#
Entorno: .NET Core / .NET 5+ (Dependiendo de la configuración del entorno local).
IDE Recomendado: Visual Studio o Visual Studio Code.

⚙️ Cómo Ejecutar
Abre una terminal y navega hasta el directorio del proyecto ListaCircularVideojuegos-Consola-ComparacionCSV/edSimpleLinkedList.
Restaura las dependencias (si las hubiera) con: dotnet restore.
Ejecuta el proyecto utilizando el comando: dotnet run.
Asegúrate de tener los archivos CSV de origen en el directorio correcto (por defecto en la ruta de ejecución o la especificada en el código).
