using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Second
{
    public class ControleUsuario
    {
        public ControleUsuario()
        {

        }

        public DadosPerfil buscarDadosPerfil(long alCodigo)
        {
            DadosPerfil lDadosPerfil = null;

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaUsuarios = from p in banco.UsuarioSet
                                        where (p.Id) == (alCodigo)
                                        select p;

                    if (listaUsuarios.Count() > 0)
                    {
                        lDadosPerfil = new DadosPerfil();
                        lDadosPerfil.iFoto = listaUsuarios.First().PerfilSet.foto;
                        lDadosPerfil.isNome = listaUsuarios.First().PerfilSet.nome;
                        lDadosPerfil.isNick = listaUsuarios.First().nick;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return lDadosPerfil;
        }

        public Boolean verificalogin(String asLogin)
        {
            Boolean lbRetorno = false;
            try
            {
                using (var banco = new modelo_second())
                {
                    var listaUsuarios = from p in banco.UsuarioSet
                                        where (p.nick) == (asLogin)
                                       select p;

                    if (listaUsuarios.Count() == 0)
                    {
                        lbRetorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
                 
            return lbRetorno;
        }

        public Boolean updateDadosPerfil(DadosPerfil aDadosPerfil)
        {
            Boolean lbRetorno = false;

            try
            {
                using (var banco = new modelo_second())
                {
                    var listaUsuarios = from p in banco.UsuarioSet
                                        where (p.Id) == (aDadosPerfil.ilCodigo)
                                        select p;

                    if (listaUsuarios.Count() > 0)
                    {
                        if (aDadosPerfil.iFoto != null)
                        {
                            listaUsuarios.First().PerfilSet.foto = aDadosPerfil.iFoto;
                        }
                        listaUsuarios.First().PerfilSet.nome = aDadosPerfil.isNome;
                        
                        banco.SaveChanges();
                        lbRetorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return lbRetorno;
        }

        public long cadastrarUsuario(String asUserId,String asUUID,byte[] asFoto, String asNome)
        {
            long llCodigoUsuario = -1;
            try
            {
                using (var banco = new modelo_second())
                {
                    UsuarioSet lUsuario = new UsuarioSet();
                    PerfilSet lPerfil = new PerfilSet();

                    lPerfil.foto = asFoto;
                    lPerfil.nome = asNome;
                    
                    lUsuario.PerfilSet = lPerfil;
                    lUsuario.nick = asUserId;
                    lUsuario.uuid = asUUID;
                    
                    banco.UsuarioSet.Add(lUsuario);
                    banco.SaveChanges();
                    llCodigoUsuario = lUsuario.Id;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return llCodigoUsuario;
        }
    }
}