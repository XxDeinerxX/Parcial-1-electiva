// ============================================================================
// SISTEMA GESTOR DE VENTAS E INVENTARIO EXPRESS (MINI-POS)
// Reto Final de Unidad 1 - Fundamentos de C# (.NET 8)
//
// Reglas respetadas:
// - Solo se usan: variables, tipos primitivos, colecciones (List<T>),
//   estructuras de control, métodos estáticos y manejo de excepciones
//   (try/catch, TryParse). NO se usan clases personalizadas ni POO.
// ============================================================================

using System.Globalization;

// Se fija la cultura a español (Colombia) para formatear moneda como
// $ 18.000,00 (punto de miles, coma decimal), tal como pide el enunciado.
CultureInfo.CurrentCulture = new CultureInfo("es-CO");

// ---------------------------------------------------------------------------
// "Base de datos" en memoria usando colecciones dinámicas (List<T>).
// Cada producto se representa por la misma posición (índice) en varias
// listas paralelas, ya que en la Unidad 1 no se permite crear clases propias.
// ---------------------------------------------------------------------------
List<string> nombresProductos = new List<string>();
List<decimal> preciosProductos = new List<decimal>();
List<int> stockProductos = new List<int>();
List<int> unidadesVendidasPorProducto = new List<int>();

int totalVentasRealizadas = 0;
decimal totalCajaDelDia = 0m;

bool continuarEjecucion = true;

do
{
    ImprimirEncabezado("SISTEMA GESTOR DE VENTAS E INVENTARIO (MINI-POS)");
    Console.WriteLine("1. Registrar nuevo producto en inventario");
    Console.WriteLine("2. Consultar inventario completo");
    Console.WriteLine("3. Registrar una venta");
    Console.WriteLine("4. Ver reporte de caja y estadísticas diarias");
    Console.WriteLine("5. Salir");
    Console.WriteLine(new string('=', 52));

    int opcionSeleccionada = LeerEntero("Seleccione una opción (1-5): ", 1, 5);

    switch (opcionSeleccionada)
    {
        case 1:
            RegistrarProducto(nombresProductos, preciosProductos, stockProductos, unidadesVendidasPorProducto);
            break;

        case 2:
            ConsultarInventario(nombresProductos, preciosProductos, stockProductos);
            break;

        case 3:
            RegistrarVenta(
                nombresProductos,
                preciosProductos,
                stockProductos,
                unidadesVendidasPorProducto,
                ref totalVentasRealizadas,
                ref totalCajaDelDia);
            break;

        case 4:
            MostrarReporteDeCaja(nombresProductos, unidadesVendidasPorProducto, totalVentasRealizadas, totalCajaDelDia);
            break;

        case 5:
            Console.WriteLine();
            Console.WriteLine("¡Gracias por usar el Sistema Gestor de Ventas! Hasta pronto.");
            continuarEjecucion = false;
            break;
    }

    if (continuarEjecucion)
    {
        Console.WriteLine();
        Console.WriteLine("Presione ENTER para continuar...");
        Console.ReadLine();
    }

} while (continuarEjecucion);


// ============================================================================
// MÉTODOS ESTÁTICOS REQUERIDOS POR EL RETO
// ============================================================================

/// <summary>
/// Lee de forma segura un número entero desde consola, validando con
/// int.TryParse dentro de un ciclo, y garantizando que esté entre
/// [min, max]. Nunca lanza una excepción no controlada.
/// </summary>
static int LeerEntero(string mensaje, int min, int max)
{
    while (true)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();

        if (!int.TryParse(entrada, out int valor))
        {
            Console.WriteLine("[ERROR] Entrada no válida. Debe ingresar un número entero.");
            continue;
        }

        if (valor < min || valor > max)
        {
            Console.WriteLine($"[ERROR] Opción fuera de rango. Ingrese un valor entre {min} y {max}.");
            continue;
        }

        return valor;
    }
}

/// <summary>
/// Lee de forma segura un número decimal desde consola, validando con
/// decimal.TryParse dentro de un ciclo, y garantizando que sea >= min.
/// </summary>
static decimal LeerDecimal(string mensaje, decimal min)
{
    while (true)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();

        if (!decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
        {
            Console.WriteLine("[ERROR] Entrada no válida. Debe ingresar un número decimal.");
            continue;
        }

        if (valor < min)
        {
            Console.WriteLine($"[ERROR] El valor debe ser mayor o igual a {min:N2}.");
            continue;
        }

        return valor;
    }
}

/// <summary>
/// Calcula el total a pagar de una venta aplicando descuento de cliente
/// frecuente (10%) e IVA (19%) sobre la base ya descontada.
/// Devuelve el total a pagar como retorno directo y los desgloses
/// (IVA y descuento) mediante parámetros de salida (out).
/// </summary>
static decimal CalcularFactura(decimal precio, int cantidad, bool tieneDescuento, out decimal montoIva, out decimal montoDescuento)
{
    decimal subtotal = precio * cantidad;

    montoDescuento = tieneDescuento ? subtotal * 0.10m : 0m;

    decimal baseConDescuento = subtotal - montoDescuento;
    montoIva = baseConDescuento * 0.19m;

    decimal totalAPagar = baseConDescuento + montoIva;
    return totalAPagar;
}

/// <summary>
/// Imprime un encabezado centrado y decorado para las distintas
/// pantallas del sistema (menú, registro, ticket, reporte, etc.).
/// </summary>
static void ImprimirEncabezado(string titulo)
{
    const int ancho = 52;
    Console.WriteLine();
    Console.WriteLine(new string('=', ancho));
    Console.WriteLine(CentrarTexto(titulo, ancho));
    Console.WriteLine(new string('=', ancho));
}

/// <summary>
/// Método utilitario auxiliar que centra un texto dentro de un ancho dado.
/// </summary>
static string CentrarTexto(string texto, int ancho)
{
    if (texto.Length >= ancho)
    {
        return texto;
    }

    int espacios = ancho - texto.Length;
    int margenIzquierdo = espacios / 2;
    int margenDerecho = espacios - margenIzquierdo;

    return new string(' ', margenIzquierdo) + texto + new string(' ', margenDerecho);
}


// ============================================================================
// FUNCIONALIDADES DEL MENÚ (Opciones 1 a 4)
// ============================================================================

/// <summary>
/// Opción 1: Registra un nuevo producto validando nombre, precio y stock.
/// </summary>
static void RegistrarProducto(
    List<string> nombres,
    List<decimal> precios,
    List<int> stocks,
    List<int> unidadesVendidas)
{
    ImprimirEncabezado("REGISTRAR NUEVO PRODUCTO");

    string nombre;
    while (true)
    {
        Console.Write("Nombre del producto: ");
        nombre = (Console.ReadLine() ?? string.Empty).Trim();

        if (string.IsNullOrEmpty(nombre))
        {
            Console.WriteLine("[ERROR] El nombre no puede estar vacío.");
            continue;
        }

        bool yaExiste = false;
        for (int i = 0; i < nombres.Count; i++)
        {
            if (nombres[i].Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                yaExiste = true;
                break;
            }
        }

        if (yaExiste)
        {
            Console.WriteLine("[ERROR] Ya existe un producto registrado con ese nombre.");
            continue;
        }

        break;
    }

    decimal precio = LeerDecimal("Precio unitario ($): ", 0.01m);
    int stock = LeerEntero("Stock inicial (cantidad disponible): ", 0, 1_000_000);

    nombres.Add(nombre);
    precios.Add(precio);
    stocks.Add(stock);
    unidadesVendidas.Add(0);

    Console.WriteLine();
    Console.WriteLine($"[OK] Producto \"{nombre}\" registrado con éxito.");
}

/// <summary>
/// Opción 2: Muestra el listado completo del inventario con alertas de bajo stock.
/// </summary>
static void ConsultarInventario(List<string> nombres, List<decimal> precios, List<int> stocks)
{
    ImprimirEncabezado("INVENTARIO COMPLETO");

    if (nombres.Count == 0)
    {
        Console.WriteLine("No hay productos registrados todavía. Use la opción 1 para registrar uno.");
        return;
    }

    for (int i = 0; i < nombres.Count; i++)
    {
        string alerta = stocks[i] < 5 ? " [ALERTA: BAJO STOCK]" : "";
        Console.WriteLine($"{i + 1}. {nombres[i],-25} | Precio: $ {precios[i],10:N2} | Stock: {stocks[i],4}{alerta}");
    }
}

/// <summary>
/// Opción 3: Procesa una venta completa: selección de producto, validación
/// de stock y cantidad, descuento opcional, cálculo de IVA y emisión de ticket.
/// </summary>
static void RegistrarVenta(
    List<string> nombres,
    List<decimal> precios,
    List<int> stocks,
    List<int> unidadesVendidas,
    ref int totalVentasRealizadas,
    ref decimal totalCajaDelDia)
{
    ImprimirEncabezado("REGISTRAR VENTA");

    if (nombres.Count == 0)
    {
        Console.WriteLine("No hay productos registrados. Registre un producto antes de vender.");
        return;
    }

    for (int i = 0; i < nombres.Count; i++)
    {
        string alerta = stocks[i] < 5 ? " [ALERTA: BAJO STOCK]" : "";
        Console.WriteLine($"{i + 1}. {nombres[i],-25} | Precio: $ {precios[i],10:N2} | Stock: {stocks[i],4}{alerta}");
    }

    Console.WriteLine();
    int indiceSeleccionado = LeerEntero(
        $"Seleccione el número del producto a vender (1-{nombres.Count}): ",
        1,
        nombres.Count) - 1;

    int cantidad;
    while (true)
    {
        cantidad = LeerEntero("Ingrese la cantidad a comprar: ", 1, 1_000_000);

        if (cantidad > stocks[indiceSeleccionado])
        {
            Console.WriteLine($"[ERROR] Stock insuficiente. Solo quedan {stocks[indiceSeleccionado]} unidades en inventario.");
            continue;
        }

        break;
    }

    bool tieneDescuento = LeerConfirmacion("¿Aplica descuento de cliente frecuente (10%)? (S/N): ");

    decimal precioUnitario = precios[indiceSeleccionado];
    decimal subtotal = precioUnitario * cantidad;

    decimal totalAPagar = CalcularFactura(precioUnitario, cantidad, tieneDescuento, out decimal montoIva, out decimal montoDescuento);

    // Actualización de inventario y estadísticas de caja.
    stocks[indiceSeleccionado] -= cantidad;
    unidadesVendidas[indiceSeleccionado] += cantidad;
    totalVentasRealizadas++;
    totalCajaDelDia += totalAPagar;

    ImprimirEncabezado("TICKET DE VENTA");
    Console.WriteLine($" Producto:             {nombres[indiceSeleccionado]} (x{cantidad})");
    Console.WriteLine($" Subtotal:             $ {subtotal,10:N2}");
    Console.WriteLine($" Descuento (10%):     -$ {montoDescuento,10:N2}");
    Console.WriteLine($" IVA (19%):            +$ {montoIva,10:N2}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($" TOTAL A PAGAR:        $ {totalAPagar,10:N2}");
    Console.WriteLine(new string('=', 52));
    Console.WriteLine($"[OK] Venta efectuada con éxito. Stock actualizado: {stocks[indiceSeleccionado]} unidades.");
}

/// <summary>
/// Opción 4: Muestra estadísticas acumuladas de la sesión: total de ventas,
/// total en caja, promedio por venta y el producto más vendido.
/// </summary>
static void MostrarReporteDeCaja(
    List<string> nombres,
    List<int> unidadesVendidas,
    int totalVentasRealizadas,
    decimal totalCajaDelDia)
{
    ImprimirEncabezado("REPORTE DE CAJA Y ESTADÍSTICAS");

    Console.WriteLine($"Total de ventas realizadas en la sesión : {totalVentasRealizadas}");
    Console.WriteLine($"Total acumulado en caja                 : $ {totalCajaDelDia:N2}");

    decimal promedioPorVenta = totalVentasRealizadas > 0 ? totalCajaDelDia / totalVentasRealizadas : 0m;
    Console.WriteLine($"Promedio de dinero por venta             : $ {promedioPorVenta:N2}");

    if (unidadesVendidas.Count == 0)
    {
        Console.WriteLine("Producto más vendido                    : No hay productos registrados.");
        return;
    }

    int indiceMasVendido = 0;
    for (int i = 1; i < unidadesVendidas.Count; i++)
    {
        if (unidadesVendidas[i] > unidadesVendidas[indiceMasVendido])
        {
            indiceMasVendido = i;
        }
    }

    if (unidadesVendidas[indiceMasVendido] == 0)
    {
        Console.WriteLine("Producto más vendido                    : Aún no se han registrado ventas.");
    }
    else
    {
        Console.WriteLine(
            $"Producto más vendido                     : {nombres[indiceMasVendido]} ({unidadesVendidas[indiceMasVendido]} unidades)");
    }
}

/// <summary>
/// Método auxiliar de validación: solicita una respuesta S/N hasta que sea válida.
/// </summary>
static bool LeerConfirmacion(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string respuesta = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

        if (respuesta == "S")
        {
            return true;
        }

        if (respuesta == "N")
        {
            return false;
        }

        Console.WriteLine("[ERROR] Respuesta no válida. Ingrese S o N.");
    }
}
