using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        public Boolean SetStatusUsuario(int aiUsuario,int aiStatus)
        {
            Boolean lbRetorno = false;

            lbRetorno = controlePartidas.atualizaStatusUsuario(aiUsuario, aiStatus);

            return lbRetorno;
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
        public String teste(String asNome)
        {
            return "Teste "+asNome;
        }
    }
}
