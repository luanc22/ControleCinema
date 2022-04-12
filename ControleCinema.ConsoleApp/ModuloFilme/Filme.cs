using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloGenero;
using ControleCinema.ConsoleApp.ModuloSessao;

namespace ControleCinema.ConsoleApp.ModuloFilme
{
    public class Filme : EntidadeBase
    {
        private readonly string _nome;
        private readonly int _duracao;
        public Genero genero;

        private readonly List<Sessao> historicoSessao = new List<Sessao>();

        public string Nome { get => _nome; }

        public int Duracao { get => _duracao; }

        public Filme(string nome, int duracao, Genero generoSelecionado)
        {
            _nome = nome;
            _duracao = duracao;

            genero = generoSelecionado;
        }

        public override string ToString()
        {
            return "Id: " + id + Environment.NewLine +
                "Nome: " + Nome + Environment.NewLine +
                "Duracao: " + Duracao + " minutos." + Environment.NewLine +
                "Gênero: " + genero.Nome + Environment.NewLine;
        }

        public void RegistrarSessao(Sessao sessao)
        {
            historicoSessao.Add(sessao);
        }
    }
}
