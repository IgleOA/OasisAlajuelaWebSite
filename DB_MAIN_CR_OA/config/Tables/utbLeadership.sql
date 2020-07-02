CREATE TABLE [config].[utbLeadership]
(
	[LeaderID]	     INT			 IDENTITY (1, 1) NOT NULL,
    [FullName]       VARCHAR (100)	 NOT NULL,
	[ImagePath]      VARCHAR (500)	 NOT NULL,
	[Description]	 VARCHAR (MAX)   NOT NULL,
    [ActionLink]     VARCHAR (50)    NULL,
	[Order]			 INT			 NOT NULL,
    [ActiveFlag]     BIT			 CONSTRAINT [utbLeadershipDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME		 CONSTRAINT [utbLeadershipDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)	 CONSTRAINT [utbLeadershipDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME		 CONSTRAINT [utbLeadershipDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)	 CONSTRAINT [utbLeadershipDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbLeaderID] PRIMARY KEY CLUSTERED ([LeaderID] ASC)
);


GO
CREATE TRIGGER [config].[utrLogLeadership] ON [config].[utbLeadership]
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
				,'[config].[utbLeadership]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END