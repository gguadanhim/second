using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Second
{
    public class ControlePartidas
    {
        private ConcurrentBag<DadosUsuario> iLista = new ConcurrentBag<DadosUsuario>();
        private ConcurrentBag<DadosPartida> iListaPartidas = new ConcurrentBag<DadosPartida>();

        public ConcurrentBag<DadosPartida> getListaPartidas()
        {
            return iListaPartidas;
        }

        public ConcurrentBag<DadosUsuario> getLista()
        {
            return iLista;
        }

        public ConcurrentBag<DadosUsuario> buscarUsuarios()
        {
            return this.getLista();
        }

        public int atualizaStatusUsuario(long aiCodigo,int aiStatus){
            int liRetorno = 0;
            DadosUsuario ldados;
            IEnumerable<DadosUsuario> lResult;
            try
            {
                lResult = this.getLista().Where(item => item.iiCodigo == aiCodigo);

                if (lResult.Count() == 0)
                {
                    ldados = new DadosUsuario();
                    ldados.iiCodigo = aiCodigo;
                    this.getLista().Add(ldados);
                }
                else
                {
                    ldados = lResult.First();
                }

                if (ldados.iiStatus != DadosPartida.STATUS_PARTIDA_AGUARDANDO)
                {
                    ldados.iiStatus = aiStatus;
                }

                liRetorno = ldados.iiStatus;
            }
            catch (Exception ex)
            {
                liRetorno = -1;
            }
            return liRetorno;
        }

        public DadosUsuario getDadosUsuario(long aiCodigo)
        {
            DadosUsuario ldados;

            ldados = this.getLista().Where(item => item.iiCodigo == aiCodigo).First();

            return ldados;
        }

        public IEnumerable<DadosUsuario> buscaUsuarioOnline(long aiUsuarioAtual)
        {
            IEnumerable<DadosUsuario> lResult;

            lResult = this.getLista().Where(item => item.iiStatus == DadosUsuario.STATUS_ONLINE && item.iiCodigo != aiUsuarioAtual);
            
            return lResult;
        }

        public Boolean criarPartida(DadosUsuario aUsuario1, DadosUsuario aUsuario2)
        {
            Boolean lbRetorno = false;
            DadosPartida lDadosPartida = new DadosPartida();

            lDadosPartida.StatusPartida = DadosPartida.STATUS_PARTIDA_INICIANDO;
            aUsuario2.iiStatus = DadosPartida.STATUS_PARTIDA_AGUARDANDO;
            
            lDadosPartida.lUsuario1 = aUsuario1;
            lDadosPartida.lUsuario2 = aUsuario2;

            getListaPartidas().Add(lDadosPartida);

            aUsuario1.iDadosPartida = lDadosPartida;
            aUsuario2.iDadosPartida = lDadosPartida;

            if (aUsuario2.getRespostaUsuario() == DadosPartida.STATUS_PARTIDA_ACEITA)
            {
                lbRetorno = true;
                aUsuario1.ibJogadorPrincipal = true;
                aUsuario1.iiStatus = DadosUsuario.STATUS_JOGANDO;
                aUsuario2.iiStatus = DadosUsuario.STATUS_JOGANDO;
                lDadosPartida.iTabuleiro = new DadosTabuleiro();
            }
            else
            {
                aUsuario2.iiStatus = DadosUsuario.STATUS_ONLINE;
            }

            return lbRetorno;
        }

        public Boolean setStatusPedidoJogo(long aiUsuario, int aiStatus)
        {
            DadosUsuario ldados = null;
            Boolean lbRetorno = false;
            try
            {
                ldados = this.getDadosUsuario(aiUsuario);

                ldados.iDadosPartida.lUsuario2.iiStatus = aiStatus;

                if (aiStatus == DadosPartida.STATUS_PARTIDA_RECUSADA)
                {
                    ldados.iDadosPartida = null;
                }
                else
                {
                    ldados.ibJogadorPrincipal = false;
                    ldados.iDadosPartida.StatusPartida = DadosPartida.STATUS_PARTIDA_INICIADA;
                }
                
                lbRetorno = true;
            }
            catch (Exception ex)
            {
                lbRetorno = false;
            }
            return lbRetorno;
        }
    }
}