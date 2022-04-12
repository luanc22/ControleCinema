using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloFilme;
using ControleCinema.ConsoleApp.ModuloSala;

namespace ControleCinema.ConsoleApp.ModuloSessao
{
    public class TelaCadastroSessao : TelaBase, ITelaCadastravel
    {
        private readonly Notificador notificador;
        private readonly IRepositorio<Sessao> repositorioSessao;
        private readonly IRepositorio<Filme> repositorioFilme;
        private readonly IRepositorio<Sala> repositorioSala;
        private readonly TelaCadastroFilme telaCadastroFilme;
        private readonly TelaCadastroSala telaCadastroSala;

        public TelaCadastroSessao(
            Notificador notificador,
            IRepositorio<Sessao> repositorioSessao,
            IRepositorio<Filme> repositorioFilme,
            IRepositorio<Sala> repositorioSala,
            TelaCadastroFilme telaCadastroFilme,
            TelaCadastroSala telaCadastroSala) : base("Cadastro de Empréstimos")
        {
            this.notificador = notificador;
            this.repositorioSessao = repositorioSessao;
            this.repositorioFilme = repositorioFilme;
            this.repositorioSala = repositorioSala;
            this.telaCadastroFilme = telaCadastroFilme;
            this.telaCadastroSala = telaCadastroSala;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Registrar uma Sessão");
            Console.WriteLine("Digite 2 para Editar Sessão");
            Console.WriteLine("Digite 3 para Excluir Sessão");
            Console.WriteLine("Digite 4 para Visualizar Sessões");
            Console.WriteLine("Digite s para sair");
            Console.WriteLine();

            Console.Write("Opção Escolhida: ");
            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastrando Sessão");

            Filme filmeSelecionado = ObtemFilme();

            Console.WriteLine();

            Sala salaSelecionada = ObtemSala();      
            
            if(filmeSelecionado == null || salaSelecionada == null)
            {
                return;
            }

            Sessao sessao = ObtemSessao(filmeSelecionado, salaSelecionada);

            string statusValidacao = repositorioSessao.Inserir(sessao);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Sessão cadastrada com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Sessão");

            bool temSessoesCadastradas = VisualizarRegistros("Pesquisando");

            if (temSessoesCadastradas == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma sessão cadastrada para poder editar", TipoMensagem.Atencao);
                return;
            }
            int numeroSessao = ObterNumeroRegistro();

            Console.WriteLine();

            Sala salaSelecionada = ObtemSala();

            Console.WriteLine();

            Filme filmeSelecionado = ObtemFilme();

            Console.WriteLine();

            Sessao sessaoAtualizada = ObtemSessao(filmeSelecionado, salaSelecionada);

            bool conseguiuEditar = repositorioSessao.Editar(x => x.id == numeroSessao, sessaoAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Sessão editada com sucesso", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Sessão");

            bool temSessoesCadastradas = VisualizarRegistros("Pesquisando");

            if (temSessoesCadastradas == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma sessão cadastrada para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroSessao = ObterNumeroRegistro();

            bool conseguiuExcluir = repositorioSessao.Excluir(x => x.id == numeroSessao);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Sessão excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Sessões");

            List<Sessao> sessoes = repositorioSessao.SelecionarTodos();

            if (sessoes.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhuma sessão disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Sessao sessao in sessoes)
            {
                if (sessao.DataSessao.Day == DateTime.Now.Day &&
                   sessao.DataSessao.Month == DateTime.Now.Month &&
                   sessao.DataSessao.Year == DateTime.Now.Year &&
                   sessao.DataSessao.Hour < DateTime.Now.Hour)
                {
                    Console.WriteLine(sessao.ToString());
                }
            }

            Console.WriteLine("Aperte ENTER para prosseguir.");
            Console.ReadLine();

            return true;
        }

        public int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID da sessão que deseja editar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = repositorioSessao.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    notificador.ApresentarMensagem("ID da sessão não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

        //privados
        private Sessao ObtemSessao(Filme filme, Sala sala)
        {
            DateTime dataSessao;
            bool conversaoFeita = false;

            do 
            {
                Console.Write("Digite a data e hora da sessão: ");
                conversaoFeita = DateTime.TryParse(Console.ReadLine(), out dataSessao);

                if (!conversaoFeita)
                    notificador.ApresentarMensagem("Por favor, insira uma data válida.", TipoMensagem.Erro);

            } while(conversaoFeita == false);
            

            Sessao novaSessao = new Sessao(filme, sala, dataSessao);

            return novaSessao;
        }

        private Filme ObtemFilme()
        {
            bool temFilmesDisponiveis = telaCadastroFilme.VisualizarRegistros("");

            if (!temFilmesDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar um filme antes de cadastrar uma sessão!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número do filme: ");
            int numFilmeSelecionado = Convert.ToInt32(Console.ReadLine());

            Filme filmeSelecionado = repositorioFilme.SelecionarRegistro(x => x.id == numFilmeSelecionado);

            return filmeSelecionado;
        }

        private Sala ObtemSala()
        {
            bool temSalasDisponiveis = telaCadastroSala.VisualizarRegistros("");

            if (!temSalasDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar uma sala antes de cadastrar uma sessão!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da sala: ");
            int numSalaSelecionada = Convert.ToInt32(Console.ReadLine());

            Sala salaSelecionada = repositorioSala.SelecionarRegistro(x => x.id == numSalaSelecionada);

            return salaSelecionada;
        }
    }
}
