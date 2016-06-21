using System;
using System.Collections.Concurrent;
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

        public DateTime alive;
        public Boolean isOnline
        {
            get {
                    if ((DateTime.Now - alive).Minutes > 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
        }
        public long iiCodigo { get; set; }
        public int iiStatus { get; set; }
        public List<int> iListaSelecaoJogador = new List<int>();
        public DadosPartida iDadosPartida { get; set; }
        public Boolean ibJogadorPrincipal = false;

        public int VerificaVitoria()
        {
            int liRetorno = 0;
            liRetorno = this.VerificaColunas();
            
            if (liRetorno != 1)
            {
                liRetorno = this.VerificaLinhas();

                if (liRetorno != 1)
                {
                    liRetorno = this.VerificaDiagonal();
                }
            }

            return liRetorno;
        }

        private int VerificaLinhas()
        {
            int liRetorno = 0;
            if (iListaSelecaoJogador.Count > 2) {
                if ((iListaSelecaoJogador.Contains(1)) && iListaSelecaoJogador.Contains(2) && iListaSelecaoJogador.Contains(3))
                {
                    liRetorno = 1;
                }
                else
                {
                    if ((iListaSelecaoJogador.Contains(4)) && iListaSelecaoJogador.Contains(5) && iListaSelecaoJogador.Contains(6))
                    {
                        liRetorno = 1;
                    }
                    else
                    {
                        if ((iListaSelecaoJogador.Contains(7)) && iListaSelecaoJogador.Contains(8) && iListaSelecaoJogador.Contains(9))
                        {
                            liRetorno = 1;
                        }
                    }
                }
            }
            return liRetorno;
        }

        private int VerificaDiagonal()
        {
            int liRetorno = 0;
            if (iListaSelecaoJogador.Count > 2)
            {
                if ((iListaSelecaoJogador.Contains(1)) && iListaSelecaoJogador.Contains(5) && iListaSelecaoJogador.Contains(9))
                {
                    liRetorno = 1;
                }
                else
                {
                    if ((iListaSelecaoJogador.Contains(3)) && iListaSelecaoJogador.Contains(5) && iListaSelecaoJogador.Contains(7))
                    {
                        liRetorno = 1;
                    }
                }
            }
            return liRetorno;
        }

        private int VerificaColunas()
        {
            int liRetorno = 0;
            if (iListaSelecaoJogador.Count > 2)
            {
                if ((iListaSelecaoJogador.Contains(1)) && iListaSelecaoJogador.Contains(4) && iListaSelecaoJogador.Contains(7))
                {
                    liRetorno = 1;
                }
                else
                {
                    if ((iListaSelecaoJogador.Contains(2)) && iListaSelecaoJogador.Contains(5) && iListaSelecaoJogador.Contains(8))
                    {
                        liRetorno = 1;
                    }
                    else
                    {
                        if ((iListaSelecaoJogador.Contains(3)) && iListaSelecaoJogador.Contains(6) && iListaSelecaoJogador.Contains(9))
                        {
                            liRetorno = 1;
                        }
                    }
                }
            }
            return liRetorno;
        }

        public void setItemSelecionado(int aiItem)
        {
            iListaSelecaoJogador.Add(aiItem);
        }

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