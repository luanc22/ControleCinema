using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloSessao;

namespace ControleCinema.ConsoleApp.ModuloSala
{
    public class Sala : EntidadeBase
    {
        private readonly int _capacidade;

        private readonly List<Sessao> historicoSessao = new List<Sessao>();

        public int Capacidade { get => _capacidade; }

        public Sala(int capacidade)
        {
            _capacidade = capacidade;
        }
        public override string ToString()
        {
            return "Sala: " + id + Environment.NewLine +
                "Capacidade: " + Capacidade + Environment.NewLine; 
        }

        public void RegistrarSessao(Sessao sessao)
        {
            historicoSessao.Add(sessao);
        }
    }
}
