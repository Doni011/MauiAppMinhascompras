﻿using SQLite;

namespace MauiAppMinhascompras.Models
{
    public class Produto
    {
        string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }

                _descricao = value;
            }
        }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }

        string _categoria;
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a categoria do produto");
                }

                _categoria = value;
            }


        }
    }
}