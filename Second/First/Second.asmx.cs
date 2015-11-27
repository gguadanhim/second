using System;
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
        public Boolean CadastrarUsuario(String asUserId,String asUUID,byte[] asFoto,String asNome)
        {
            return iControleUsuarios.cadastrarUsuario(asUserId, asUUID, asFoto,asNome);
        }

        /*[WebMethod]
        public List<Jogador> BuscarJogadores()
        {
            return iControleJogadores.getJogadores();
        }*/

        /*[WebMethod]
        public List<Campo> BuscarCampos()
        {
            return iControleJogadores.getCampos();
        }*/

        [WebMethod]
        public String teste(String asNome)
        {
            return "Teste "+asNome;
        }
    }
}
