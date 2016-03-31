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
        public string HelloWorld()
        {
            return "Hello World";
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
        public int AtualizaStatusUsuario(int aiUsuario,int aiStatus)
        {
            int liRetorno = 0;

            liRetorno = controlePartidas.atualizaStatusUsuario(aiUsuario, aiStatus);

            return liRetorno;
        }

        [WebMethod]
        public Boolean BuscarPlayerPartida(int aiUsuario)
        {
            Boolean lbRetorno = false;
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
                        lbRetorno = true;
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
            
            return lbRetorno;
        }

        [WebMethod]
        public Boolean SetStatusPedidoJogo(int aiUsuario, int aiStatus)
        {
            Boolean lbRetorno = false;

            lbRetorno = controlePartidas.setStatusPedidoJogo(aiUsuario, aiStatus);

            return lbRetorno;
        }

        [WebMethod]
        public int SetItemSelecionadoJogador(int aiPosicao, int aiJogador)
        {
            int liRetorno = 0;
            DadosUsuario lDadosUsuario;
            
            lDadosUsuario = controlePartidas.getDadosUsuario(aiJogador);

            lDadosUsuario.setItemSelecionado(aiPosicao);


            liRetorno = lDadosUsuario.VerificaVitoria();

            if (liRetorno == 1)
            {
                lDadosUsuario.iDadosPartida.StatusPartida = DadosPartida.STATUS_PARTIDA_FINALIZADA;
            }

            lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador = aiJogador;
            lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado = aiPosicao;
            
            return liRetorno;
        }

        [WebMethod]
        public int AguardarJogada(int aiJogador)
        {
            int liRetorno = 0;
            int liContador = 0;
            Boolean lbParar = false;
            DadosUsuario lDadosUsuario;
            
            lDadosUsuario = controlePartidas.getDadosUsuario(aiJogador);

            while (true)
	        {
                if (lDadosUsuario.iDadosPartida.StatusPartida == DadosPartida.STATUS_PARTIDA_FINALIZADA)
                {
                    liRetorno = 10;
                    lbParar = true;
                }
                else
                {
                    if (lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilJogador == aiJogador)
                    {
                        liContador++;
                        Thread.Sleep(1000);
                        if (liContador == 15)
                        {
                            liRetorno = 11;
                            lbParar = true;
                        }
                    }
                    else
                    {
                        liRetorno = lDadosUsuario.iDadosPartida.iDadosUltimaJogada.ilSequencialJogado;
                        lbParar = true;
                    }
                }

                if (lbParar)
                {
                    break;
                }
 
	        }
            
            return liRetorno;
        }

        [WebMethod]
        public String teste(String asNome)
        {
            return "Teste "+asNome;
        }
    }
}
