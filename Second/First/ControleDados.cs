using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Second
{
    public class ControleDados
    {
        public DadosRank getDadosRankingUsuario(long alUsuario)
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
                        lDados.ilPontos = listaRanking.First().pontos;
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

        public List<DadosRank> getDadosRanking(long alPosicao)
        {
            List<DadosRank> lLista = new List<DadosRank>();

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaRanking = (from v in banco.view_rank
                                       from p in banco.resultados_usuarioSet
                                       where ((v.UsuarioSet_Id == p.UsuarioSet.Id)
                                          && (alPosicao == 0 || v.rank > alPosicao))
                                       select new { v, p }).Take(2);


                    foreach (var item in listaRanking)
                    {
                        DadosRank lDados = new DadosRank();
                        lDados.ilCodigoUsuario = item.p.UsuarioSet.Id;
                        lDados.isNomeUsuariao = item.p.UsuarioSet.nick;
                        lDados.ilDerrotas = item.p.derrotas;
                        lDados.ilDesistencias = item.p.desistencias;
                        lDados.ilVitorias = item.p.vitorias;
                        lDados.ilPontos = item.p.pontos;
                        lDados.ilRank = (long)item.v.rank;
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

        public List<DadosPerfil> BuscarNovosAmigos(long alUsuario,String asNome){
            List<DadosPerfil> ilListaAmigos = new List<DadosPerfil>();

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaUsuarios = from p in banco.UsuarioSet
                                       where (p.Id) != (alUsuario)
                                          && (p.nick.Contains(asNome) )
                                      select p;

                    foreach (var item in listaUsuarios)
                    {
                        DadosPerfil lDados = new DadosPerfil();
                        lDados.ilCodigo  = item.Id ;
                        lDados.isNick = item.nick;
                        lDados.isNome = item.PerfilSet.nome;
                        lDados.iFoto = item.PerfilSet.foto;
                        ilListaAmigos.Add(lDados);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            
            return ilListaAmigos;
        }

        public List<DadosPerfil> BuscarAmigos(long alUsuario)
        {
            List<DadosPerfil> ilListaAmigos = new List<DadosPerfil>();

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaUsuarios = from p in banco.amigosSet
                                        where (p.UsuarioSet.Id == alUsuario)
                                        select p;
                    
                    var listaConvites = from p in banco.amigosSet
                                       where (p.Convidados.Id == alUsuario)
                                          && p.aceite == 0
                                      select p;

                    foreach (var item in listaUsuarios)
                    {
                        DadosPerfil lDados = new DadosPerfil();
                        lDados.ilCodigo = item.Convidados.Id;
                        lDados.isNick = item.Convidados.nick;
                        lDados.isNome = item.Convidados.PerfilSet.nome;
                        lDados.iFoto = item.Convidados.PerfilSet.foto;
                        lDados.ilConviteAceito = item.aceite; 

                        ilListaAmigos.Add(lDados);
                    }

                    foreach (var item in listaConvites)
                    {
                        DadosPerfil lDados = new DadosPerfil();
                        lDados.ilCodigo = item.UsuarioSet.Id;
                        lDados.isNick = item.UsuarioSet.nick;
                        lDados.isNome = item.UsuarioSet.PerfilSet.nome;
                        lDados.iFoto = item.UsuarioSet.PerfilSet.foto;
                        lDados.ilConviteAceito = 2;

                        ilListaAmigos.Add(lDados);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return ilListaAmigos;
        }

        public DadosRetorno AdicionarAmigo(long alUsuario, long alAmigo)
        {
            DadosRetorno lRetorno = new DadosRetorno();

            try
            {
                using (var banco = new modelo_second())
                {

                    var listaUsuario = from p in banco.UsuarioSet 
                                      where p.Id == alUsuario
                                     select p;

                    var listaUsuarioAdicionado = from p in banco.UsuarioSet
                                                where p.Id == alAmigo
                                               select p;

                    amigos lAmigos = new amigos();

                    lAmigos.aceite = 0;
                    lAmigos.UsuarioSet = listaUsuario.First();
                    lAmigos.Convidados = listaUsuarioAdicionado.First();

                    banco.amigosSet.Add(lAmigos);
                    banco.SaveChanges();
                    lRetorno.liCodigo = 1; 
                }
            }
            catch (Exception ex)
            {
                lRetorno.liCodigo = -1;
                lRetorno.lsMensagem = ex.Message;
                System.Console.WriteLine(ex.Message);
            }

            return lRetorno;
        }

        public DadosRetorno RemoverAmigo(long alUsuario, long alAmigo)
        {
            DadosRetorno lRetorno = new DadosRetorno();

            try
            {
                using (var banco = new modelo_second())
                {

                    var listaConvitesFeito = from p in banco.amigosSet
                                            where p.Convidados.Id == alUsuario
                                               && p.UsuarioSet.Id == alAmigo
                                           select p;

                    var listaConvitesAceito = from p in banco.amigosSet
                                              where p.Convidados.Id == alAmigo
                                                 && p.UsuarioSet.Id == alUsuario
                                             select p;

                    banco.amigosSet.Remove(listaConvitesFeito.First());
                    banco.amigosSet.Remove(listaConvitesAceito.First());
                    banco.SaveChanges();
                    lRetorno.liCodigo = 1;
                }
            }
            catch (Exception ex)
            {
                lRetorno.liCodigo = -1;
                lRetorno.lsMensagem = ex.Message;
                System.Console.WriteLine(ex.Message);
            }

            return lRetorno;
        }

        public DadosRetorno AceitarAmigo(long alUsuario, long alAmigo,long status)
        {
            DadosRetorno lRetorno = new DadosRetorno();
            const long ACEITO = 1;
            const long RECUSADO = 2;

            try
            {
                using (var banco = new modelo_second())
                {

                    var listaConvites = from p in banco.amigosSet 
                                       where p.Convidados.Id == alUsuario
                                          && p.UsuarioSet.Id == alAmigo 
                                       select p;

                    if (listaConvites.Count() > 0)
                    {
                        if (status == ACEITO)
                        {
                            var listaUsuario = from p in banco.UsuarioSet
                                               where p.Id == alUsuario
                                               select p;

                            var listaUsuarioAdicionado = from p in banco.UsuarioSet
                                                         where p.Id == alAmigo
                                                         select p;

                            listaConvites.First().aceite = 1;

                            amigos lAmigos = new amigos();

                            lAmigos.aceite = 1;
                            lAmigos.UsuarioSet = listaUsuario.First();
                            lAmigos.Convidados = listaUsuarioAdicionado.First();
                            banco.amigosSet.Add(lAmigos);
                            lRetorno.liCodigo = 1;
                        }
                        else
                        {
                            banco.amigosSet.Remove(listaConvites.First());
                            lRetorno.liCodigo = 1;
                        }
                        banco.SaveChanges();
                    }
                    else
                    {
                        lRetorno.liCodigo = -2;
                    }
                }
            }
            catch (Exception ex)
            {
                lRetorno.liCodigo = -1;
                lRetorno.lsMensagem = ex.Message;
                System.Console.WriteLine(ex.Message);
            }

            return lRetorno;
        }
    }
}