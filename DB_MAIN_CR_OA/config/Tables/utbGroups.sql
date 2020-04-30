CREATE TABLE [config].[utbGroups] (
    [GroupID]        INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]      VARCHAR (100) NOT NULL,
    [Description]    VARCHAR (MAX) NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbGroupsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME      CONSTRAINT [utbGroupsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100) CONSTRAINT [utbGroupsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbGroupsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbGroupsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbGroupID] PRIMARY KEY CLUSTERED ([GroupID] ASC)
);


GO
CREATE TRIGGER [config].[utrLogGroups] ON [config].[utbGroups]
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
				,'[config].[utbGroups]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END