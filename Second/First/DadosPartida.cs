using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Second
{
    public class DadosPartida
    {
        public const int STATUS_PARTIDA_INICIANDO = 1;
        public const int STATUS_PARTIDA_INICIADA = 2;
        public const int STATUS_PARTIDA_FINALIZADA = 3;
        public const int STATUS_PARTIDA_ACEITA = 4;
        public const int STATUS_PARTIDA_RECUSADA = 5;
        public const int STATUS_PARTIDA_AGUARDANDO = 6;

        public int StatusPartida { get; set; }
        public DadosUsuario lUsuario1 { get; set; }
        public DadosUsuario lUsuario2 { get; set; }
    }
}