using ControleCinema.ConsoleApp.Compartilhado;
using System;

namespace ControleCinema.ConsoleApp.ModuloGenero
{
    public class Genero : EntidadeBase
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public Genero(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public override string ToString()
        {
            return "Id: " + id + Environment.NewLine +
                "Nome do Gênero: " + Nome + Environment.NewLine +
                "Descrição do Gênero: " + Descricao + Environment.NewLine;
        }
    }
}
