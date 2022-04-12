using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCinema.ConsoleApp.Compartilhado;
using ControleCinema.ConsoleApp.ModuloGenero;

namespace ControleCinema.ConsoleApp.ModuloFilme
{
    public class TelaCadastroFilme : TelaBase, ITelaCadastravel
    {
        private readonly TelaCadastroGenero telaCadastroGenero;
        private readonly IRepositorio<Genero> repositorioGenero;

        private readonly IRepositorio<Filme> _repositorioFilme;
        private readonly Notificador _notificador;

        public TelaCadastroFilme(IRepositorio<Filme> repositorioFilme, Notificador notificador, TelaCadastroGenero telaCadastroGenero,
            IRepositorio<Genero> repositorioGenero)
            : base("Cadastro de Filmes")
        {
            _repositorioFilme = repositorioFilme;
            _notificador = notificador;
            this.telaCadastroGenero = telaCadastroGenero;
            this.repositorioGenero = repositorioGenero;

        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Filme");

            Genero generoSelecionado = ObtemGenero();

            Filme novoFilme = ObterFilme(generoSelecionado);

            string statusValidacao = _repositorioFilme.Inserir(novoFilme);

            if (statusValidacao == "REGISTRO_VALIDO")
                _notificador.ApresentarMensagem("Filme inserido com sucesso", TipoMensagem.Sucesso);
            else
                _notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Filme");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum filme cadastrado para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroFilme = ObterNumeroRegistro();

            _notificador.ApresentarMensagem("Mostrando gêneros disponíveis.", TipoMensagem.Atencao);

            Genero generoSelecionado = ObtemGenero();

            _notificador.ApresentarMensagem("Novas informações a inserir.", TipoMensagem.Atencao);

            Filme filmeAtualizado = ObterFilme(generoSelecionado);

            bool conseguiuEditar = _repositorioFilme.Editar(numeroFilme, filmeAtualizado);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Filme editado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Filme");

            bool temFilmesRegistrados = VisualizarRegistros("Pesquisando");

            if (temFilmesRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum filme cadastrado para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroFilme = ObterNumeroRegistro();

            bool conseguiuExcluir = _repositorioFilme.Excluir(numeroFilme);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Filme excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Filmes");

            List<Filme> filmes = _repositorioFilme.SelecionarTodos();

            if (filmes.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum filme disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Filme filme in filmes)
                Console.WriteLine(filme.ToString());

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
                Console.Write("Digite o ID do filme que deseja editar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = _repositorioFilme.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    _notificador.ApresentarMensagem("ID do filme não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }


        private Filme ObterFilme(Genero generoSelecionado)
        {
            Console.Write("Digite o nome do filme: ");
            string nome = Console.ReadLine();

            Console.Write("Digite a duração em minutos do filme: ");
            int duracao = int.Parse(Console.ReadLine());

            return new Filme(nome, duracao, generoSelecionado);
        }

        private Genero ObtemGenero()
        {
            bool temGenerosDisponiveis = telaCadastroGenero.VisualizarRegistros("");

            if (!temGenerosDisponiveis)
            {
                _notificador.ApresentarMensagem("Você precisa cadastrar um gênero antes de cadastrar um filme!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número do gênero do filme: ");
            int numGeneroSelecionado = Convert.ToInt32(Console.ReadLine());

            Genero generoSelecionado = repositorioGenero.SelecionarRegistro(x => x.id == numGeneroSelecionado);

            return generoSelecionado;
        }
    }
}
