using EjemploMVVM.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace EjemploMVVM.Repositories
{
    // Modelo auxiliar para las Categorías dentro del repositorio
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }

    public class ProductoRepositoryImpl : IProductoRepository
    {
        string cn;
        public ProductoRepositoryImpl()
        {
            cn = ConfigurationManager.ConnectionStrings["EjemploMVVM.Properties.Settings.NorthwindDB"].ConnectionString;
        }

        // Método nuevo para listar las categorías de Northwind
        public List<Categoria> ListarCategorias()
        {
            string query = "SELECT CategoryID, CategoryName FROM Categories";
            List<Categoria> listaCategorias = new List<Categoria>();
            using (SqlConnection conex = new SqlConnection(cn))
            {
                conex.Open();
                SqlCommand sqlCommand = new SqlCommand(query, conex);
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Categoria cat = new Categoria
                    {
                        IdCategoria = reader.GetInt32(0),
                        NombreCategoria = reader.GetString(1)
                    };
                    listaCategorias.Add(cat);
                }
            }
            return listaCategorias;
        }

        // Búsqueda mejorada que acepta opcionalmente el ID de categoría y el texto
        public List<Producto> BuscarPorFiltros(int? idCategoria, string nombre)
        {
            string query = "SELECT ProductID, ProductName, UnitPrice, Discontinued FROM Products WHERE (@IdCategoria IS NULL OR CategoryID = @IdCategoria) AND (@Nombre IS NULL OR ProductName LIKE @Nombre)";
            List<Producto> listaProductos = new List<Producto>();
            using (SqlConnection conex = new SqlConnection(cn))
            {
                conex.Open();
                object catParam = (idCategoria.HasValue && idCategoria.Value > 0) ? (object)idCategoria.Value : DBNull.Value;
                object nombreParam = string.IsNullOrEmpty(nombre) ? DBNull.Value : (object)("%" + nombre + "%");

                SqlCommand sqlCommand = new SqlCommand(query, conex);
                sqlCommand.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = catParam;
                sqlCommand.Parameters.Add("@Nombre", SqlDbType.NVarChar, 40).Value = nombreParam;

                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        precio = reader.GetDecimal(2),
                        discontinuado = reader.GetBoolean(3)
                    };
                    listaProductos.Add(producto);
                }
                return listaProductos;
            }
        }

        public List<Producto> BuscarPorNombre(string nombre)
        {
            return BuscarPorFiltros(null, nombre);
        }

        public List<Producto> ListarTodos()
        {
            string query = "SELECT ProductID, ProductName, UnitPrice, Discontinued FROM Products";
            List<Producto> listaProductos = new List<Producto>();
            using (SqlConnection conex = new SqlConnection(cn))
            {
                conex.Open();
                SqlCommand sqlCommand = new SqlCommand(query, conex);
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        precio = reader.GetDecimal(2),
                        discontinuado = reader.GetBoolean(3)
                    };
                    listaProductos.Add(producto);
                }
                return listaProductos;
            }
        }
    }
}