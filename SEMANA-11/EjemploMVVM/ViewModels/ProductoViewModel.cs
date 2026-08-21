using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EjemploMVVM.Commands;
using EjemploMVVM.Models;
using EjemploMVVM.Repositories;

namespace EjemploMVVM.ViewModels
{
    public class ProductoViewModel
    {
        public ObservableCollection<Producto> productos { get; set; } = new ObservableCollection<Producto>();

        // Colección para el ComboBox de Categorías
        public ObservableCollection<Categoria> Categorias { get; set; } = new ObservableCollection<Categoria>();

        // Propiedad para almacenar la categoría seleccionada en el ComboBox
        private int? _categoriaSeleccionada;
        public int? CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
            }
        }

        public RelayCommand ComandoCargarProductos { get; set; }

        public string textoBuscar { get; set; } = string.Empty;

        private ProductoRepositoryImpl _repository;

        public ProductoViewModel()
        {
            // Cambiamos a ProductoRepositoryImpl para acceder a los métodos específicos de categorías
            _repository = new ProductoRepositoryImpl();
            ComandoCargarProductos = new RelayCommand(BuscarProductos);

            CargarCategorias();
            CargarProductos();
        }

        private void CargarCategorias()
        {
            List<Categoria> listaCat = _repository.ListarCategorias();
            Categorias.Clear();
            foreach (var cat in listaCat)
            {
                Categorias.Add(cat);
            }
        }

        private void BuscarProductos()
        {
            // Llama al nuevo método que filtra por categoría y texto a la vez
            List<Producto> lista = _repository.BuscarPorFiltros(CategoriaSeleccionada, textoBuscar);
            productos.Clear();
            foreach (Producto producto in lista)
            {
                productos.Add(producto);
            }
        }

        public void CargarProductos()
        {
            List<Producto> lista = _repository.ListarTodos();
            productos.Clear();
            foreach (Producto producto in lista)
            {
                productos.Add(producto);
            }
        }
    }
}