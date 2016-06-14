using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace Second
{
    /// <summary>
    /// Summary description for Second
    /// </summary>
    [WebService(Namespace = "http://second-2.apphb.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Second : System.Web.Services.WebService
    {
        private ControleUsuario iControleUsuarios;
        private ControleDados iControleDados;
        static ControlePartidas controlePartidas = new ControlePartidas();

        public Second()
        {
            iControleUsuarios = new ControleUsuario();
            iControleDados = new ControleDados();
        }

        [WebMethod]
        public List<DadosRank> BuscarRanking(long alposicao)
        {
            return iControleDados.getDadosRanking(alposicao);
        }
        
        [WebMethod]
        public DadosRetorno contaPartidas()
        {
            DadosRetorno lDadosRetorno = new DadosRetorno();

            lDadosRetorno.liCodigo = controlePartidas.getListaPartidas().Count;

            return lDadosRetorno;
        }

        [WebMethod]
        public DadosRetorno adicionarteste(long alcodigo, int alpontos)
        {
            DadosRetorno lDadosRetorno = new DadosRetorno();
            
            resultados_usuario lResultado = null;

                using (var banco = new modelo_second())
                {
                    var resultadosUsuario = from p in banco.resultados_usuarioSet
                                            where (p.UsuarioSet.Id) == (alcodigo)
                                        select p;

                    if (resultadosUsuario.Count() > 0)
                    {
                        lResultado = resultadosUsuario.First();
                    }
                    else
                    {
                        lResultado = new resultados_usuario();

                        var dadosUsuario = from p in banco.UsuarioSet
                                           where (p.Id) == (alcodigo)
                                           select p;

                        lResultado.UsuarioSet = dadosUsuario.First();
                        banco.resultados_usuarioSet.Add(lResultado);
                    }

                    lResultado.pontos += alpontos;
                     
                    banco.SaveChanges();
                }
            
            return lDadosRetorno;
        }

        [WebMethod]
        public Boolean VerificarUsuario(String asNome)
        {
            return iControleUsuarios.verificalogin(asNome);
        }

        [WebMethod]
        public long CadastrarUsuario(String asUserId,String asUUID,byte[] asFoto,String asNome)
        {
            return iControleUsuarios.cadastrarUsuario(asUserId, asUUID, asFoto,asNome);
        }

        [WebMethod]
        public DadosPerfil BuscarDadosUsuario(long alUserId)
        {
            DadosPerfil ldados = null;

            ldados = iControleUsuarios.buscarDadosPerfil(alUserId);

            return ldados;
        }
        
        [WebMethod]
        public Boolean UpdateDadosUsuario(DadosPerfil adados)
        {
            Boolean lbRetorno = false;

            lbRetorno = iControleUsuarios.updateDadosPerfil(adados);

            return lbRetorno;
        }
        
        [WebMethod]
        public List<DadosUsuario> BuscarDadosUsuarios()
        {
            return controlePartidas.buscarUsuarios().ToList();
        }

        [WebMethod]
        public DadosRetorno AtualizaStatusUsuario(long aiUsuario, int aiStatus)
        {
            DadosRetorno lDadosRetorno = new DadosRetorno();
            
            lDadosRetorno.liCodigo = controlePartidas.atualizaStatusUsuario(aiUsuario, aiStatus);

            return lDadosRetorno;
        }
        
        [WebMethod]
        public DadosRetorno BuscarPlayerPartida(long aiUsuario)
        {
            DadosRetorno ldadoRetorno = new DadosRetorno();
            DadosUsuario lUsuario1;
            List<DadosUsuario> lLista;
            int liContador = 0;

            lUsuario1 = controlePartidas.getDadosUsuario(aiUsuario);

            lLista = controlePartidas.buscaUsuarioOnline(aiUsuario).ToList<DadosUsuario>();
            
            foreach (var lUsuario2 in lLista)
            {
                if (lUsuario2 != null)
                {
                    if (controlePartidas.criarPartida(lUsuario1, lUsuario2))
                    {
                        ldadoRetorno.liCodigo = 1;
                        break;
                    }
                    else
                    {
                        liContador++;

                        if (liContador == 10)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
           }

            return ldadoRetorno;
        }

        [WebMethod]
        public DadosRetorno SetStatusPedidoJogo(long aiUsuario, int aiStatus)
        {
            DadosRetorno ldadoRetorno = new DadosRetorno();

            if (controlePartidas.setStatusPedidoJogo(aiUsuario, aiStatus))
            {
                ldadoRetorno.liCodigo = 1;
            }

            return ldadoRetorno;
        }

        [WebMethod]
        public DadosRetorno SetItemSelecionadoJogador(int aiPosicao, long aiJogador)
        {
            DadosRetorno ldadoRetorno = new DadosRetorno();
            DadosUsuario lDadosUsuario;
            
            lDadosUsuario = controlePartidas.getDadosUsuario(aiJogador);

            lDadosUsuario.setItemSelecionado(aiPosicao);

            ldadoRetorno.liCodigo = lDadosUsuario.VerificaVitoria();

            if (ldadoRetorno.liCodigo == 1)
            {
                lDadosUsuario.iDadosPartida.VerificaResultado(lDadosUsuario, false);

                lDadosUsuario.iDadosPartida.StatusPartida = DadosPartida.STATUS_PARTIDA_FINALIZADA;
            }

            lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador = aiJogador;
            lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado = aiPosicao;

            return ldadoRetorno;
        }

        [WebMethod]
        public List<DadosPerfil> BuscarNovosAmigos(long aiJogador, String asNome)
        {
            return iControleDados.BuscarNovosAmigos(aiJogador,asNome);
        }

        [WebMethod]
        public List<DadosPerfil> BuscarAmigos(long aiJogador)
        {
            return iControleDados.BuscarAmigos(aiJogador);
        }

        [WebMethod]
        public DadosRetorno AceitarAmigo(long alUsuario, long alAmigo, long status)
        {
            return iControleDados.AceitarAmigo(alUsuario, alAmigo, status);
        }

        [WebMethod]
        public DadosRetorno RemoverAmigo(long alUsuario, long alAmigo)
        {
            return iControleDados.RemoverAmigo(alUsuario, alAmigo);
        }

        [WebMethod]
        public DadosRetorno AdicionarAmigo(long alUsuario, long alAmigo)
        {
            return iControleDados.AdicionarAmigo(alUsuario, alAmigo);
        }

        [WebMethod]
        public DadosServidor BuscarDadosServidor()
        {
            DadosServidor lDados = new DadosServidor();
            lDados.ilJogadoresJogando = controlePartidas.getLista().Where(x => x.iiStatus == DadosUsuario.STATUS_JOGANDO).Count();
            lDados.ilJogadoresOnline = controlePartidas.getLista().Where(x => x.iiStatus == DadosUsuario.STATUS_ONLINE).Count();
            lDados.ilPartidas = controlePartidas.getListaPartidas().Count;
            return lDados;
        }

        [WebMethod]
        public DadosRank BuscarHistorico(long aiJogador)
        {
            return iControleDados.getDadosRankingUsuario(aiJogador);
        }

        [WebMethod]
        public DadosRetorno AguardarJogada(long aiJogador)
        {
            int liContador = 0;
            DadosRetorno ldadoRetorno = new DadosRetorno();
            Boolean lbParar = false;
            DadosUsuario lDadosUsuario;
            
            lDadosUsuario = controlePartidas.getDadosUsuario(aiJogador);

            while (true)
	        {
                if ((lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador == 0) && (lDadosUsuario.ibJogadorPrincipal))
                {
                    ldadoRetorno.liCodigo = 12;
                    lbParar = true;
                }
                else
                {
                    if (lDadosUsuario.iDadosPartida.StatusPartida == DadosPartida.STATUS_PARTIDA_FINALIZADA)
                    {
                        ldadoRetorno.lsMensagem = lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado.ToString();
                        ldadoRetorno.liCodigo = 10;
                        RemovePartida(lDadosUsuario);
                        lbParar = true;
                    }
                    else
                    {
                        if ((lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador == aiJogador) || (lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador == 0))
                        {
                            liContador++;
                            Thread.Sleep(1000);
                            if (liContador == 25)
                            {
                                lDadosUsuario.iDadosPartida.VerificaResultado(lDadosUsuario, true);
                                ldadoRetorno.liCodigo = 11;
                                RemovePartida(lDadosUsuario);
                                lbParar = true;
                            }
                        }
                        else
                        {
                            ldadoRetorno.liCodigo = lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado;
                            lbParar = true;
                        }
                    }   
                }
                if (lbParar)
                {
                    break;
                }
	        }

            return ldadoRetorno;
        }

        private void RemovePartida(DadosUsuario aUsuario)
        {
            DadosPartida ll = null;
            ll = aUsuario.iDadosPartida;

            aUsuario.iDadosPartida.lUsuario1.iListaSelecaoJogador.Clear();
            aUsuario.iDadosPartida.lUsuario2.iListaSelecaoJogador.Clear();
            
            controlePartidas.getListaPartidas().TryTake(out ll);
            controlePartidas.getListaPartidas().TryPeek(out ll);
            aUsuario.iDadosPartida = null;
            
        }
    }
}
