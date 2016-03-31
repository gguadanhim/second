using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Second
{
    public class DadosTabuleiro
    {
        int C1L1 = 0;
        int C1L2 = 0;
        int C1L3 = 0;
        int C2L1 = 0;
        int C2L2 = 0;
        int C2L3 = 0;
        int C3L1 = 0;
        int C3L2 = 0;
        int C3L3 = 0;

        const int QUANTIDADE_MINIMA = 5;

        public int iQuantidadeJogadas = 0;

        public int verificaColunas(){
            int liJogador = 0;
            Boolean lbEncontrado = false;
            if (C1L1 != 0){
                if((C1L1 == C1L2) && (C1L2 == C1L3)){
                    liJogador = C1L1;
                    lbEncontrado = true;
                }
            }
            if (!lbEncontrado)
            {
                if (C2L1 != 0)
                {
                    if ((C2L1 == C2L2) && (C2L2 == C2L3))
                    {
                        liJogador = C2L1;
                        lbEncontrado = true;
                    }
                }
            }

            if (!lbEncontrado)
            {
                if (C3L1 != 0)
                {
                    if ((C3L1 == C3L2) && (C3L2 == C3L3))
                    {
                        liJogador = C3L1;
                        lbEncontrado = true;
                    }
                }
            }
            return liJogador;
        }
        
        public int verificaLinhas()
        {
            int liJogador = 0;
            Boolean lbEncontrado = false;

            if (C1L1 != 0)
            {
                if ((C1L1 == C2L1) && (C2L1 == C3L1))
                {
                    liJogador = C1L1;
                    lbEncontrado = true;
                }
            }

            if (!lbEncontrado)
            {
                if (C1L2 != 0)
                {
                    if ((C1L2 == C2L2) && (C2L2 == C3L2))
                    {
                        liJogador = C1L2;
                        lbEncontrado = true;
                    }
                }
            }

            if (!lbEncontrado)
            {
                if (C1L3 != 0)
                {
                    if ((C1L3 == C2L3) && (C2L3 == C3L3))
                    {
                        liJogador = C1L3;
                        lbEncontrado = true;
                    }
                }
            }

            return liJogador;
        }

        public int verificaDiagonal()
        {
            int liJogador = 0;
            Boolean lbEncontrado = false;

            if (C1L1 != 0)
            {
                if((C1L1==C2L2)&&(C2L2==C3L3)){
                    liJogador = C1L1;
                    lbEncontrado = true;
                }
            }

            if (!lbEncontrado)
            {
                if (C1L3 != 0)
                {
                    if ((C1L3 == C2L2) && (C2L2 == C3L1))
                    {
                        liJogador = C1L3;
                        lbEncontrado = true;
                    }
                }
            }
            return liJogador;
        }

        public int setItemSelecionado(int aiItem, int aiJogador)
        {
            int liRetorno = 0;
            return liRetorno;
        }

        public int setLinhaColuna(int aiColuna, int aiLinha, int aiJogador)
        {
            int liRetorno = 0;
            switch (aiColuna)
            {
                case 1:
                    switch (aiLinha)
                    {
                        case 1:
                            C1L1 = aiJogador;
                            break;
                        case 2:
                            C1L2 = aiJogador;
                            break;
                        case 3:
                            C1L3 = aiJogador;
                            break;
                    }
                    break;
                case 2:
                    switch (aiLinha)
                    {
                        case 1:
                            C2L1 = aiJogador;
                            break;
                        case 2:
                            C2L2 = aiJogador;
                            break;
                        case 3:
                            C2L3 = aiJogador;
                            break;
                    }
                    break;
                case 3:
                    switch (aiLinha)
                    {
                        case 1:
                            C3L1 = aiJogador;
                            break;
                        case 2:
                            C3L2 = aiJogador;
                            break;
                        case 3:
                            C3L3 = aiJogador;
                            break;
                    }
                    break;
            }
            
            iQuantidadeJogadas++;

            return liRetorno;
        }

        public int verificaVitoria()
        {
            int liJogador = 0;

            if (iQuantidadeJogadas >= QUANTIDADE_MINIMA)
            {
                liJogador = this.verificaColunas();

                if (liJogador == 0)
                {
                    liJogador = this.verificaLinhas();
                    if (liJogador == 0)
                    {
                        liJogador = this.verificaDiagonal();
                    }
                }
            }

            return liJogador;
        }

    }

}