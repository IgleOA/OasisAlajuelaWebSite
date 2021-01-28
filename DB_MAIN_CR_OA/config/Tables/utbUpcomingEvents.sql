CREATE TABLE [config].[utbUpcomingEvents] (
    [EventID]			INT           IDENTITY (1, 1) NOT NULL,
    [Title]				VARCHAR (50)  NOT NULL,
    [MinisterID]		INT           NOT NULL,
    [Description]		VARCHAR (MAX) NULL,
    [ScheduledDate]		DATETIME      NOT NULL,
	[ReservationFlag]	BIT			  NOT NULL,
	[Capacity]			INT           NULL,
    [ActiveFlag]		BIT           CONSTRAINT [utbUpcomingEventsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]		DATETIME      CONSTRAINT [utbUpcomingEventsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100) CONSTRAINT [utbUpcomingEventsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME      CONSTRAINT [utbUpcomingEventsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100) CONSTRAINT [utbUpcomingEventsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbEventID] PRIMARY KEY CLUSTERED ([EventID] ASC),
    CONSTRAINT [FK.config.utbMinisters.config.utbUpcomingEvents.MinisterID] FOREIGN KEY ([MinisterID]) REFERENCES [config].[utbMinisters] ([MinisterID])
);


GO

CREATE TRIGGER [config].[utrLogUpcommingEvents] ON [config].[utbUpcomingEvents]
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
				,'[adm].[utbUpcomingEvents]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END