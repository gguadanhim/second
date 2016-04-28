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
        static ControlePartidas controlePartidas = new ControlePartidas();

        public Second()
        {
            iControleUsuarios  = new ControleUsuario();
        }

        [WebMethod]
        public DadosRetorno contaPartidas()
        {
            DadosRetorno lDadosRetorno = new DadosRetorno();

            lDadosRetorno.liCodigo = controlePartidas.getListaPartidas().Count;

            return lDadosRetorno;
        }

        [WebMethod]
        public DadosRetorno teste()
        {
            DadosUsuario lDadosUsuario;
            DadosPartida ll = new DadosPartida();

            lDadosUsuario = controlePartidas.getDadosUsuario(1);

            lDadosUsuario.iDadosPartida.VerificaResultado(lDadosUsuario, false);
            DadosRetorno lDadosRetorno = new DadosRetorno();

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
            DadosUsuario lUsuario2;
            int liContador = 0;

            lUsuario1 = controlePartidas.getDadosUsuario(aiUsuario);

            while (true)
            {
                lUsuario2 = controlePartidas.buscaUsuarioOnline(aiUsuario);

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
                        RemovePartida(lDadosUsuario);

                        ldadoRetorno.liCodigo = 10;
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
                                RemovePartida(lDadosUsuario);
                                ldadoRetorno.liCodigo = 11;
                                lbParar = true;
                            }
                        }
                        else
                        {
                            ldadoRetorno.liCodigo = lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado;
                            lbParar = true;
                        }
                    }

                    if (lbParar)
                    {
                        break;
                    }
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
