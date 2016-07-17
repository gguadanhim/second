
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/17/2016 16:31:53
-- Generated from EDMX file: D:\Git\second\second\First\Modelo_second.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbecc20a0950a3450cad9aa555018154ef];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PerfilSetUsuarioSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PerfilSet] DROP CONSTRAINT [FK_PerfilSetUsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[FK_resultados_usuarioUsuarioSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[resultados_usuarioSet] DROP CONSTRAINT [FK_resultados_usuarioUsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioSetamigos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[amigosSet] DROP CONSTRAINT [FK_UsuarioSetamigos];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioSetamigos1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[amigosSet] DROP CONSTRAINT [FK_UsuarioSetamigos1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PerfilSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PerfilSet];
GO
IF OBJECT_ID(N'[dbo].[UsuarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsuarioSet];
GO
IF OBJECT_ID(N'[dbo].[resultados_usuarioSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[resultados_usuarioSet];
GO
IF OBJECT_ID(N'[dbo].[amigosSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[amigosSet];
GO
IF OBJECT_ID(N'[dbo].[view_rank]', 'U') IS NOT NULL
    DROP TABLE [dbo].[view_rank];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PerfilSet'
CREATE TABLE [dbo].[PerfilSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [foto] varbinary(max)  NULL,
    [nome] nvarchar(max)  NOT NULL,
    [UsuarioSet_Id] bigint  NOT NULL
);
GO

-- Creating table 'UsuarioSet'
CREATE TABLE [dbo].[UsuarioSet] (
    [Id] bigint  NOT NULL,
    [nick] nvarchar(max)  NOT NULL,
    [uuid] nvarchar(max)  NOT NULL,
    [Perfil_Id] int  NOT NULL
);
GO

-- Creating table 'resultados_usuarioSet'
CREATE TABLE [dbo].[resultados_usuarioSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [vitorias] int  NOT NULL,
    [derrotas] int  NOT NULL,
    [desistencias] int  NOT NULL,
    [pontos] int  NOT NULL,
    [UsuarioSet_Id] bigint  NOT NULL
);
GO

-- Creating table 'amigosSet'
CREATE TABLE [dbo].[amigosSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [aceite] int  NOT NULL,
    [UsuarioSet_Id] bigint  NOT NULL,
    [Convidados_Id] bigint  NOT NULL
);
GO

-- Creating table 'view_rank'
CREATE TABLE [dbo].[view_rank] (
    [UsuarioSet_Id] bigint  NOT NULL,
    [rank] bigint  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'PerfilSet'
ALTER TABLE [dbo].[PerfilSet]
ADD CONSTRAINT [PK_PerfilSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UsuarioSet'
ALTER TABLE [dbo].[UsuarioSet]
ADD CONSTRAINT [PK_UsuarioSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'resultados_usuarioSet'
ALTER TABLE [dbo].[resultados_usuarioSet]
ADD CONSTRAINT [PK_resultados_usuarioSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'amigosSet'
ALTER TABLE [dbo].[amigosSet]
ADD CONSTRAINT [PK_amigosSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UsuarioSet_Id] in table 'view_rank'
ALTER TABLE [dbo].[view_rank]
ADD CONSTRAINT [PK_view_rank]
    PRIMARY KEY CLUSTERED ([UsuarioSet_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UsuarioSet_Id] in table 'PerfilSet'
ALTER TABLE [dbo].[PerfilSet]
ADD CONSTRAINT [FK_PerfilSetUsuarioSet]
    FOREIGN KEY ([UsuarioSet_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PerfilSetUsuarioSet'
CREATE INDEX [IX_FK_PerfilSetUsuarioSet]
ON [dbo].[PerfilSet]
    ([UsuarioSet_Id]);
GO

-- Creating foreign key on [UsuarioSet_Id] in table 'resultados_usuarioSet'
ALTER TABLE [dbo].[resultados_usuarioSet]
ADD CONSTRAINT [FK_resultados_usuarioUsuarioSet]
    FOREIGN KEY ([UsuarioSet_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_resultados_usuarioUsuarioSet'
CREATE INDEX [IX_FK_resultados_usuarioUsuarioSet]
ON [dbo].[resultados_usuarioSet]
    ([UsuarioSet_Id]);
GO

-- Creating foreign key on [UsuarioSet_Id] in table 'amigosSet'
ALTER TABLE [dbo].[amigosSet]
ADD CONSTRAINT [FK_UsuarioSetamigos]
    FOREIGN KEY ([UsuarioSet_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioSetamigos'
CREATE INDEX [IX_FK_UsuarioSetamigos]
ON [dbo].[amigosSet]
    ([UsuarioSet_Id]);
GO

-- Creating foreign key on [Convidados_Id] in table 'amigosSet'
ALTER TABLE [dbo].[amigosSet]
ADD CONSTRAINT [FK_UsuarioSetamigos1]
    FOREIGN KEY ([Convidados_Id])
    REFERENCES [dbo].[UsuarioSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioSetamigos1'
CREATE INDEX [IX_FK_UsuarioSetamigos1]
ON [dbo].[amigosSet]
    ([Convidados_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------