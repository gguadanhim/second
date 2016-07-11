select * from AmigosSet

delete from AmigosSet where Convidados_Id =6

insert into  PerfilSet values (1,'','abc da silva',1);
insert into UsuarioSet values (1,'abc',1);
insert into UsuarioSet values (2,'def',2);
insert into UsuarioSet values (3,'ghi',3);
insert into UsuarioSet values (4,'jkl',4);

insert into resultados_usuarioSet values(1,2,8,5,20,1)

select *,ROW_NUMBER() OVER (ORDER BY pontos desc) AS RowNumber from resultados_usuarioSet
where usuarioset_id = 4

select *, Rank() OVER (ORDER BY pontos desc) AS rank from resultados_usuarioSet
where usuarioset_id = 4


select * from view_rank where UsuarioSet_id = 4
select * from  resultados_usuarioSet
select * from UsuarioSet
select * from PerfilSet

update resultados_usuarioSet set pontos = 10 where UsuarioSet_Id = 4
commit


--CREATE VIEW view_rank AS
SELECT UsuarioSet_Id,
	   Rank() OVER (ORDER BY pontos desc) AS rank
FROM resultados_usuarioSet