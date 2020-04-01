CREATE TABLE [config].[utbNews] (
    [NewID]          INT             IDENTITY (1, 1) NOT NULL,
    [Title]          VARCHAR (30)    NOT NULL,
    [Description]    VARCHAR (200)   NOT NULL,
    [BannerData]     VARBINARY (MAX) NOT NULL,
    [BannerExt]      VARCHAR (5)     NOT NULL,
    [ActiveFlag]     BIT             CONSTRAINT [utbNewsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME        CONSTRAINT [utbNewsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)   CONSTRAINT [utbNewsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME        CONSTRAINT [utbNewsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)   CONSTRAINT [utbNewsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbNewID] PRIMARY KEY CLUSTERED ([NewID] ASC)
);


GO

CREATE TRIGGER [config].[utrLogNews] ON [config].[utbNews]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT [NewID],[Title],[Description],[BannerExt],[ActiveFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser] FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT [NewID],[Title],[Description],[BannerExt],[ActiveFlag],[InsertDate],[InsertUser],[LastModifyDate],[LastModifyUser] FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

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
				,'[config].[utbNews]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END
