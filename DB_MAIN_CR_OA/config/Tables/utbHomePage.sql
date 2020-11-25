CREATE TABLE [config].[utbHomePage] (
    [SectionID]			INT				IDENTITY (1, 1) NOT NULL,
	[Title]				VARCHAR(500)	NOT NULL,
	[Description]		VARCHAR(MAX)	NOT NULL,
	[RouterLink]		VARCHAR(100)	NULL,
	[Image]				VARCHAR(500)	NULL,
	[Order]				INT				NULL,
    [ActiveFlag]        BIT				CONSTRAINT [utbHomePageDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]        DATETIME		CONSTRAINT [utbHomePageDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]        VARCHAR (100)	CONSTRAINT [utbHomePageDefaultInsertUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]    DATETIME		CONSTRAINT [utbHomePageDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]    VARCHAR (100)	CONSTRAINT [utbHomePageDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbSectionID] PRIMARY KEY CLUSTERED ([SectionID] ASC)
);


GO
CREATE TRIGGER [config].[utrHomePage] ON [config].[utbHomePage]
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
				,'[config].[utbHomePage]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted
	END;