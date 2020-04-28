CREATE TABLE [config].[utbMinistries] (
    [MinistryID]     INT				IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)		NOT NULL,
    [Description]    VARCHAR (MAX)		NOT NULL,
	[Image]		 VARBINARY (MAX)	NULL,
    [ImageExt]	 VARCHAR (10)		NULL,
    [ActionLink]     VARCHAR (50)  NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbMinistriesDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbMinistriesDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbMinistriesDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbMinistriesDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbMinistriesDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbMinistryID] PRIMARY KEY CLUSTERED ([MinistryID] ASC)
);


GO
CREATE TRIGGER [config].[utrLogMinistries] ON [config].[utbMinistries]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT [MinistryID],[Name],[Description],[ImageExt],[ActionLink] FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT [MinistryID],[Name],[Description],[ImageExt],[ActionLink] FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

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
				,'[config].[utbMinistries]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END