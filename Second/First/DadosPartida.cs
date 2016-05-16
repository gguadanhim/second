using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Second
{
    public class DadosPartida
    {
        public DadosPartida()
        {
            iDadosUltimaJogada = new DadosUltimaJogada();
        }
       
        public const int STATUS_PARTIDA_INICIANDO = 1;
        public const int STATUS_PARTIDA_INICIADA = 2;
        public const int STATUS_PARTIDA_FINALIZADA = 3;
        public const int STATUS_PARTIDA_ACEITA = 4;
        public const int STATUS_PARTIDA_RECUSADA = 5;
        public const int STATUS_PARTIDA_AGUARDANDO = 6;

        public const int VITORIA = 1;
        public const int DERROTA = 2;
        public const int DESISTENCIA = 3;

        public int StatusPartida { get; set; }
        public DadosUsuario lUsuario1 { get; set; }
        public DadosUsuario lUsuario2 { get; set; }
        public DadosTabuleiro iTabuleiro{ get; set; }
        public DadosUltimaJogada iDadosUltimaJogada { get; set; }
        private Object outputLock = new Object();

        public DadosRetorno VerificaResultado(DadosUsuario aDadosUsuarioVitoria, Boolean abVitoriaPorAbandono)
        {
            DadosRetorno lDados = new DadosRetorno();

            if (Monitor.TryEnter(outputLock))
            {
                try
                {
                    long llCodigoUsuarioDerrota = 0;

                    this.adicionarResultado(aDadosUsuarioVitoria.iiCodigo, VITORIA);

                    if (aDadosUsuarioVitoria.ibJogadorPrincipal)
                    {
                        llCodigoUsuarioDerrota = aDadosUsuarioVitoria.iDadosPartida.lUsuario2.iiCodigo;
                    }
                    else
                    {
                        llCodigoUsuarioDerrota = aDadosUsuarioVitoria.iDadosPartida.lUsuario1.iiCodigo;
                    }

                    if (abVitoriaPorAbandono)
                    {
                        this.adicionarResultado(llCodigoUsuarioDerrota, DESISTENCIA);
                    }
                    else
                    {
                        this.adicionarResultado(llCodigoUsuarioDerrota, DERROTA);
                    }
                }
                finally
                {
                    Monitor.Exit(outputLock);
                }
            }
            return lDados;
        }

        public DadosRetorno adicionarResultado(long aiCodigoUsuario, int aiTipo)
        {
            DadosRetorno lDados = new DadosRetorno();
            resultados_usuario lResultado = null;
            try
            {
                using (var banco = new modelo_second())
                {
                    var resultadosUsuario = from p in banco.resultados_usuarioSet
                                        where (p.UsuarioSet.Id) == (aiCodigoUsuario)
                                        select p;

                    if (resultadosUsuario.Count() > 0)
                    {
                        lResultado = resultadosUsuario.First();
                    }
                    else
                    {
                        lResultado = new resultados_usuario();

                        var dadosUsuario = from p in banco.UsuarioSet
                                           where (p.Id) == (aiCodigoUsuario)
                                           select p;

                        lResultado.UsuarioSet = dadosUsuario.First();
                        banco.resultados_usuarioSet.Add(lResultado);
                    }

                    switch (aiTipo)
                    {
                        case VITORIA:
                            lResultado.vitorias++;
                            break;
                        case DERROTA:
                            lResultado.derrotas++;
                            break;
                        case DESISTENCIA:
                            lResultado.desistencias++;
                            break;
                    }

                    banco.SaveChanges();
                    lDados.liCodigo = 1;
                }
            }
            catch (Exception ex)
            {
                lDados.lsMensagem = ex.Message;
                System.Console.WriteLine(ex.Message);
            }

            return lDados;
        }
    }
}