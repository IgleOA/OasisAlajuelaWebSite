CREATE TABLE [adm].[utbWebDirectory] (
    [WebID]          INT           IDENTITY (1, 1) NOT NULL,
    [AppID]          INT           NOT NULL,
    [Controller]     VARCHAR (50)  NOT NULL,
    [Action]         VARCHAR (50)  NOT NULL,
    [PublicMenu]     BIT           NOT NULL,
    [AdminMenu]      BIT           NOT NULL,
    [DisplayName]    VARCHAR (50)  NOT NULL,
    [Parameter]      VARCHAR (50)  NULL,
    [Order]          INT           NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbWebDirectoryDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]   DATETIME      CONSTRAINT [utbWebDirectoryDefaultCreationDateSysDatetime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]   VARCHAR (100) CONSTRAINT [utbWebDirectoryDefaultCreationUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbWebDirectoryDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbWebDirectoryDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbWebID] PRIMARY KEY CLUSTERED ([WebID] ASC),
    CONSTRAINT [fk.adm.utbWebDirectory.adm.AppDirectory.AppID] FOREIGN KEY ([AppID]) REFERENCES [adm].[utbAppDirectory] ([AppID])
);


GO



CREATE TRIGGER [adm].[utrLogWebDirectory] ON [adm].[utbWebDirectory]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT * FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT * FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

		CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

		INSERT INTO #DBCC
		EXEC ('DBCC INPUTBUFFER(@@SPID)')

		--Assume it is an insert
		SET @INSERTUPDATE ='INSERT'
		--If there's data in deleted, it's an update
		IF EXISTS(SELECT * FROM Deleted)
		  SET @INSERTUPDATE='UPDATE'

		INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
		SELECT	@INSERTUPDATE
				,'[adm].[utbWebDirectory]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted
	END