using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Second
{
    public class DadosUsuario
    {
        public const int STATUS_OFFLINE = 0;
        public const int STATUS_ONLINE = 1;
        public const int STATUS_JOGANDO = 2;
        
        public int iiCodigo { get; set; }
        public int iiStatus { get; set; }
        public DadosPartida iDadosPartida { get; set; }

        public int getRespostaUsuario()
        {
            int liRetorno = DadosPartida.STATUS_PARTIDA_AGUARDANDO;
            int liContador = 0;

            while (true)
            {
                if (this.iiStatus == DadosPartida.STATUS_PARTIDA_AGUARDANDO)
                {
                    liContador++;
                    Thread.Sleep(1000);
                    
                    if (liContador == 10)
                    {
                        liRetorno = DadosPartida.STATUS_PARTIDA_RECUSADA;
                        break;
                    }
                }
                else
                {
                    liRetorno = this.iiStatus;
                    break;
                }
            }
            return liRetorno;
        }
    }
}