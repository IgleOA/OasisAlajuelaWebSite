CREATE TABLE [config].[utbSermons] (
    [SermonID]        INT             IDENTITY (1, 1) NOT NULL,
    [Title]           VARCHAR (100)   NOT NULL,
    [Description]     VARCHAR (MAX)   NOT NULL,
    [Tags]            VARCHAR (MAX)   NULL,
    [MinisterID]      INT             NOT NULL,
    [SermonDate]      DATETIME        NOT NULL,
    [SermonURL]       VARCHAR (500)   NOT NULL,
    [ImagePath]		  VARCHAR (500)   NOT NULL,
    [ActiveFlag]      BIT             CONSTRAINT [utbSermonsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]      DATETIME        CONSTRAINT [utbSermonsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]      VARCHAR (100)   CONSTRAINT [utbSermonsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]  DATETIME        CONSTRAINT [utbSermonsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]  VARCHAR (100)   CONSTRAINT [utbSermonsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [SermonID] PRIMARY KEY CLUSTERED ([SermonID] ASC),
    CONSTRAINT [FK.config.utbMinisters.config.utbSermons.MinisterID] FOREIGN KEY ([MinisterID]) REFERENCES [config].[utbMinisters] ([MinisterID])
);


GO
CREATE TRIGGER [config].[utrLogSermons] ON [config].[utbSermons]
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
				,'[adm].[utbSermons]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END