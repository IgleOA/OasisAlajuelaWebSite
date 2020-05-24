CREATE TABLE [config].[utbUserNotes]
(
	[NoteID]	     INT           IDENTITY (1, 1) NOT NULL,
    [UserID]         INT           NOT NULL,
    [RequestNote]    VARCHAR(MAX)  NOT NULL,
	[ResponseRequired]		 BIT   CONSTRAINT [utbUserNotesDefaultResponseRequiredIsTrue] DEFAULT ((1)) NOT NULL,
	[ResponseNote]   VARCHAR(MAX)  NULL,
	[ResponseDate]	 DATETIME	   NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbUserNotesDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
	[InsertUserID]   INT		   NOT NULL,
	[ReadFlag]		 BIT           CONSTRAINT [utbUserNotesDefaultReadFlagIsFalse] DEFAULT ((0)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbUserNotesDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,    
    [LastModifyDate] DATETIME      CONSTRAINT [utbUserNotesDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbUserNotesDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbNoteID] PRIMARY KEY CLUSTERED ([NoteID] ASC),
    CONSTRAINT [FK.adm.utbUsers.config.utbUserNotes.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID]),
	CONSTRAINT [FK.adm.utbUsers.config.utbUserNotes.InsertUserID] FOREIGN KEY ([InsertUserID]) REFERENCES [adm].[utbUsers] ([UserID])
);


GO
CREATE TRIGGER [config].[utrLogUserNotes] ON [config].[utbUserNotes]
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
				,'[config].[utbUserNotes]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END