# Sistema Gestor de Ventas e Inventario Express (Mini-POS)

**Estudiante:** [Tu Nombre Completo]
**Curso:** Profundización en .NET — Unidad 1: Fundamentos de C#
**Reto:** Reto Final de Unidad 1 — Mini-POS

## 📋 Descripción del proyecto

Aplicación de consola desarrollada en **C# (.NET 8)** que simula un punto de venta (POS) básico para una tienda de barrio. Permite:

- Registrar productos en un inventario en memoria (nombre, precio y stock).
- Consultar el inventario completo, con alertas de bajo stock (menos de 5 unidades).
- Registrar ventas, validando stock disponible y cantidades, aplicando un descuento opcional de cliente frecuente (10%) y calculando el IVA (19%) sobre la base con descuento.
- Consultar un reporte de caja con el total de ventas, el dinero acumulado, el promedio por venta y el producto más vendido de la sesión.

El proyecto se desarrolló usando **únicamente** los temas cubiertos en las Semanas 1 a 4 de la Unidad 1: variables, tipos primitivos, colecciones (`List<T>`), estructuras de control (`if`, `switch`, `do-while`, `while`, `for`), métodos estáticos y manejo seguro de errores (`try/catch` implícito mediante `int.TryParse` / `decimal.TryParse`). **No se utiliza Programación Orientada a Objetos** (no hay clases personalizadas, constructores ni herencia); toda la información de los productos se maneja con listas paralelas en memoria.

## 🧩 Métodos estáticos principales

| Método | Descripción |
|---|---|
| `LeerEntero(string mensaje, int min, int max)` | Lee y valida un número entero dentro de un rango, usando `int.TryParse` en un ciclo. |
| `LeerDecimal(string mensaje, decimal min)` | Lee y valida un número decimal mayor o igual a un mínimo, usando `decimal.TryParse`. |
| `CalcularFactura(decimal precio, int cantidad, bool tieneDescuento, out decimal montoIva, out decimal montoDescuento)` | Calcula el descuento, el IVA (19%) y el total a pagar de una venta. |
| `ImprimirEncabezado(string titulo)` | Imprime un encabezado centrado y decorado para cada pantalla del sistema. |

## ⚙️ Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- Git instalado.

Puedes verificar tu versión de .NET con:

```bash
dotnet --version
```

## 🚀 Cómo clonar y ejecutar el proyecto

1. Clona el repositorio:

   ```bash
   git clone https://github.com/<tu-usuario>/<nombre-del-repositorio>.git
   cd <nombre-del-repositorio>
   ```

2. Restaura y ejecuta el proyecto:

   ```bash
   dotnet run
   ```

3. Se mostrará el menú principal en consola. Navega usando las opciones **1 a 5**.

> 💡 Antes de entregar, se recomienda clonar el repositorio en una carpeta distinta y correr `dotnet build` seguido de `dotnet run` para confirmar que compila y funciona correctamente desde cero.

## 🖥️ Ejemplo de ejecución

### Menú principal

```
====================================================
   SISTEMA GESTOR DE VENTAS E INVENTARIO (MINI-POS)
====================================================
1. Registrar nuevo producto en inventario
2. Consultar inventario completo
3. Registrar una venta
4. Ver reporte de caja y estadísticas diarias
5. Salir
====================================================
Seleccione una opción (1-5):
```

### Validación de entradas inválidas

```
Seleccione una opción (1-5): abc
[ERROR] Entrada no válida. Debe ingresar un número entero.

Seleccione una opción (1-5): 9
[ERROR] Opción fuera de rango. Ingrese un valor entre 1 y 5.
```

### Registro de una venta y generación de ticket

```
====================================================
                 REGISTRAR VENTA
====================================================
1. Café Colombiano 500g   | Precio: $  18.000,00 | Stock:   10
2. Pan Tajado Integral    | Precio: $   6.500,00 | Stock:    3 [ALERTA: BAJO STOCK]

Seleccione el número del producto a vender (1-2): 1
Ingrese la cantidad a comprar: 15
[ERROR] Stock insuficiente. Solo quedan 10 unidades en inventario.

Ingrese la cantidad a comprar: 2
¿Aplica descuento de cliente frecuente (10%)? (S/N): S

====================================================
                  TICKET DE VENTA
====================================================
 Producto:             Café Colombiano 500g (x2)
 Subtotal:             $  36.000,00
 Descuento (10%):     -$   3.600,00
 IVA (19%):            +$   6.156,00
----------------------------------------------------
 TOTAL A PAGAR:        $  38.556,00
====================================================
[OK] Venta efectuada con éxito. Stock actualizado: 8 unidades.
```

### Reporte de caja

```
====================================================
        REPORTE DE CAJA Y ESTADÍSTICAS
====================================================
Total de ventas realizadas en la sesión : 1
Total acumulado en caja                 : $ 38.556,00
Promedio de dinero por venta             : $ 38.556,00
Producto más vendido                     : Café Colombiano 500g (2 unidades)
```

## 📁 Estructura del repositorio

```
GestorVentasUnidad1/
├── Program.cs                  # Lógica completa del sistema
├── GestorVentasUnidad1.csproj  # Archivo de proyecto .NET 8
├── .gitignore                  # Ignora bin/ y obj/
└── README.md                   # Este archivo
```
