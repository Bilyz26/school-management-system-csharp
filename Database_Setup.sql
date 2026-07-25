USE [master]
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DB_Support_School')
BEGIN
    CREATE DATABASE [DB_Support_School];
END
GO

USE [DB_Support_School]
GO

-- 1. Table: Annee
IF OBJECT_ID('dbo.Annee', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Annee](
        [annee] [int] NOT NULL,
        PRIMARY KEY CLUSTERED ([annee] ASC)
    );
END
GO

-- 2. Table: emp
IF OBJECT_ID('dbo.emp', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[emp](
        [nomemp] [varchar](50) NOT NULL,
        [tele] [varchar](50) NULL,
        [fonction] [varchar](50) NULL,
        [username] [varchar](50) NULL,
        [pw] [varchar](50) NULL,
        PRIMARY KEY CLUSTERED ([nomemp] ASC)
    );
END
GO

-- 3. Table: matier
IF OBJECT_ID('dbo.matier', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[matier](
        [idmat] [varchar](50) NOT NULL,
        [nomMat] [varchar](50) NULL,
        PRIMARY KEY CLUSTERED ([idmat] ASC)
    );
END
GO

-- 4. Table: prof
IF OBJECT_ID('dbo.prof', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[prof](
        [nomprof] [varchar](50) NOT NULL,
        [teleprof] [varchar](50) NULL,
        [#idmat] [varchar](50) NULL,
        PRIMARY KEY CLUSTERED ([nomprof] ASC)
    );
END
GO

-- 5. Table: niveauMat
IF OBJECT_ID('dbo.niveauMat', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[niveauMat](
        [codeNiv] [varchar](50) NOT NULL,
        [#idmat] [varchar](50) NULL,
        [nomMat] [varchar](50) NULL,
        PRIMARY KEY CLUSTERED ([codeNiv] ASC)
    );
END
GO

-- 6. Table: grp
IF OBJECT_ID('dbo.grp', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[grp](
        [codegrp] [varchar](50) NOT NULL,
        [#idmat] [varchar](50) NULL,
        [#codeNiv] [varchar](50) NULL,
        PRIMARY KEY CLUSTERED ([codegrp] ASC)
    );
END
GO

-- 7. Table: inscStd
IF OBJECT_ID('dbo.inscStd', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[inscStd](
        [#cin] [varchar](10) NULL,
        [qui] [varchar](10) NULL,
        [nom] [varchar](50) NOT NULL,
        [tele] [varchar](20) NULL,
        [frinsc] [decimal](18, 0) NULL,
        [dateD] [datetime] NULL,
        PRIMARY KEY CLUSTERED ([nom] ASC)
    );
END
GO

-- 8. Table: Raff
IF OBJECT_ID('dbo.Raff', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Raff](
        [#nom] [varchar](50) NOT NULL,
        [#codegrp] [varchar](50) NOT NULL,
        [annee] [int] NOT NULL,
        [#nomprof] [varchar](50) NOT NULL,
        PRIMARY KEY CLUSTERED ([#nom] ASC, [#nomprof] ASC, [#codegrp] ASC, [annee] ASC)
    );
END
GO

-- 9. Table: seance
IF OBJECT_ID('dbo.seance', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[seance](
        [#codegrp] [varchar](50) NOT NULL,
        [#annee] [int] NOT NULL,
        [#nomprof] [varchar](50) NOT NULL,
        [dayy] [varchar](50) NOT NULL,
        [heureD] [time](7) NULL,
        [heureF] [time](7) NULL,
        PRIMARY KEY CLUSTERED ([#codegrp] ASC, [#annee] ASC, [#nomprof] ASC, [dayy] ASC)
    );
END
GO

-- 10. Table: pay
IF OBJECT_ID('dbo.pay', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[pay](
        [#nom] [varchar](50) NOT NULL,
        [#codegrp] [varchar](50) NOT NULL,
        [#nomprof] [varchar](50) NOT NULL,
        [#idmat] [varchar](50) NOT NULL,
        [#annee] [int] NOT NULL,
        [#codeNiv] [varchar](50) NULL,
        [datep] [datetime] NULL,
        [monthp] [varchar](50) NOT NULL,
        [prix] [decimal](18, 0) NOT NULL,
        PRIMARY KEY CLUSTERED ([#nom] ASC, [#codegrp] ASC, [#nomprof] ASC, [#idmat] ASC, [monthp] ASC, [#annee] ASC)
    );
END
GO

-- Foreign Key Constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_grp_matier')
    ALTER TABLE [dbo].[grp] WITH CHECK ADD FOREIGN KEY([#idmat]) REFERENCES [dbo].[matier] ([idmat]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_niveauMat_matier')
    ALTER TABLE [dbo].[niveauMat] WITH CHECK ADD FOREIGN KEY([#idmat]) REFERENCES [dbo].[matier] ([idmat]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_pay_Annee')
    ALTER TABLE [dbo].[pay] WITH CHECK ADD FOREIGN KEY([#annee]) REFERENCES [dbo].[Annee] ([annee]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_pay_grp')
    ALTER TABLE [dbo].[pay] WITH CHECK ADD FOREIGN KEY([#codegrp]) REFERENCES [dbo].[grp] ([codegrp]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_pay_matier')
    ALTER TABLE [dbo].[pay] WITH CHECK ADD FOREIGN KEY([#idmat]) REFERENCES [dbo].[matier] ([idmat]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_pay_inscStd')
    ALTER TABLE [dbo].[pay] WITH CHECK ADD FOREIGN KEY([#nom]) REFERENCES [dbo].[inscStd] ([nom]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_pay_prof')
    ALTER TABLE [dbo].[pay] WITH CHECK ADD FOREIGN KEY([#nomprof]) REFERENCES [dbo].[prof] ([nomprof]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_prof_matier')
    ALTER TABLE [dbo].[prof] WITH CHECK ADD FOREIGN KEY([#idmat]) REFERENCES [dbo].[matier] ([idmat]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Raff_grp')
    ALTER TABLE [dbo].[Raff] WITH CHECK ADD FOREIGN KEY([#codegrp]) REFERENCES [dbo].[grp] ([codegrp]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Raff_inscStd')
    ALTER TABLE [dbo].[Raff] WITH CHECK ADD FOREIGN KEY([#nom]) REFERENCES [dbo].[inscStd] ([nom]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Raff_prof')
    ALTER TABLE [dbo].[Raff] WITH CHECK ADD FOREIGN KEY([#nomprof]) REFERENCES [dbo].[prof] ([nomprof]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Raff_Annee')
    ALTER TABLE [dbo].[Raff] WITH CHECK ADD FOREIGN KEY([annee]) REFERENCES [dbo].[Annee] ([annee]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_seance_Annee')
    ALTER TABLE [dbo].[seance] WITH CHECK ADD FOREIGN KEY([#annee]) REFERENCES [dbo].[Annee] ([annee]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_seance_grp')
    ALTER TABLE [dbo].[seance] WITH CHECK ADD FOREIGN KEY([#codegrp]) REFERENCES [dbo].[grp] ([codegrp]);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_seance_prof')
    ALTER TABLE [dbo].[seance] WITH CHECK ADD FOREIGN KEY([#nomprof]) REFERENCES [dbo].[prof] ([nomprof]);
GO

-- Default Seeds
IF NOT EXISTS (SELECT * FROM dbo.emp WHERE nomemp = 'admin')
BEGIN
    INSERT INTO dbo.emp (nomemp, tele, fonction, username, pw)
    VALUES ('admin', '0600000000', 'Directeur', 'admin', 'admin123');
END
GO

IF NOT EXISTS (SELECT * FROM dbo.Annee WHERE annee = 2026)
BEGIN
    INSERT INTO dbo.Annee (annee) VALUES (2026), (2025), (2024);
END
GO

PRINT 'DB_Support_School schema synchronized successfully!';
GO
