using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloFilme;
using ControleCinema.ConsoleApp.ModuloSala;


namespace ControleCinema.ConsoleApp.ModuloSessao
{
    public class Sessao : EntidadeBase
    {
        public Filme filme;
        public Sala sala;
        private DateTime dataSessao;

        public DateTime DataSessao { get => dataSessao;}

        public string Status { get => EstaFechada() ? "Encerrado" : "Aberto/Em Andamento"; }

        public Sessao (Filme filme, Sala sala, DateTime dataSessao)
        {
            this.filme = filme;
            this.sala = sala;
            this.dataSessao = dataSessao;
        }

        public override string ToString()
        {
            return "Número: " + id + Environment.NewLine +
                "Filme: " + filme.Nome + Environment.NewLine +
                "Sala: " + sala.id + Environment.NewLine +
                "Capacidade da Sala: " + sala.Capacidade + Environment.NewLine +
                "Data e hora do filme: " + DataSessao.ToString() + Environment.NewLine +
                "Status do filme: " + Status + Environment.NewLine;
        }

        public bool EstaFechada()
        {
            if (dataSessao < DateTime.Now)
                return true;

            return false;
        }

    }
}
