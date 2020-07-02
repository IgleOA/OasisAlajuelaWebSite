CREATE TABLE [config].[utbPodcasts]
(
	[PodcastID]      INT             IDENTITY (1, 1) NOT NULL,
    [Title]          VARCHAR (30)    NOT NULL,
    [Description]    VARCHAR (200)   NOT NULL,
    [BannerPath]	 VARCHAR (500)   NOT NULL,
	[MinisterID]     INT             NOT NULL,
    [ActiveFlag]     BIT             CONSTRAINT [utbPodcastsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME        CONSTRAINT [utbPodcastsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)   CONSTRAINT [utbPodcastsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME        CONSTRAINT [utbPodcastsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)   CONSTRAINT [utbPodcastsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbPodcastID] PRIMARY KEY CLUSTERED ([PodcastID] ASC),
	CONSTRAINT [FK.config.utbMinisters.config.utbPodcasts.MinisterID] FOREIGN KEY ([MinisterID]) REFERENCES [config].[utbMinisters] ([MinisterID])
);


GO

CREATE TRIGGER [config].[utrLogPodcasts] ON [config].[utbPodcasts]
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
				,'[config].[utbPodcasts]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END