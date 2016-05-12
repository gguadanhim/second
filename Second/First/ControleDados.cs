using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Second
{
    public class ControleDados
    {
        public DadosRank getDadosRanking(long alUsuario)
        {
            DadosRank lDados = new DadosRank();

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaRanking = from p in banco.resultados_usuarioSet 
                                        where (p.UsuarioSet.Id ) == (alUsuario)
                                        select p;

                    if (listaRanking.Count() > 0)
                    {
                        lDados.ilCodigoUsuario = listaRanking.First().UsuarioSet.Id;
                        lDados.isNomeUsuariao = listaRanking.First().UsuarioSet.nick;
                        lDados.ilDerrotas = listaRanking.First().derrotas;
                        lDados.ilDesistencias = listaRanking.First().desistencias;
                        lDados.ilVitorias = listaRanking.First().vitorias;
                        lDados.liCodigo = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                lDados.liCodigo = -1;
                lDados.lsMensagem = ex.Message;
                System.Console.WriteLine(ex.Message);
            }

            return lDados;
        }

        public List<DadosRank> getDadosRanking()
        {
            List<DadosRank> lLista = new List<DadosRank>();

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaRanking = from p in banco.resultados_usuarioSet
                                       select p;

                    foreach (var item in listaRanking)
                    {
                        DadosRank lDados = new DadosRank();
                        lDados.ilCodigoUsuario = item.UsuarioSet.Id;
                        lDados.isNomeUsuariao = item.UsuarioSet.nick;
                        lDados.ilDerrotas = item.derrotas;
                        lDados.ilDesistencias = item.desistencias;
                        lDados.ilVitorias = item.vitorias;
                        lDados.liCodigo = 1;
                        lLista.Add(lDados);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return lLista;
        }
    }
}