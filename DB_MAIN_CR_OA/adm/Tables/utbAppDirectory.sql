CREATE TABLE [adm].[utbAppDirectory] (
    [AppID]          INT           IDENTITY (1, 1) NOT NULL,
    [AppName]        VARCHAR (50)  NOT NULL,
    [Order]          INT           NOT NULL,
    [PrivateSite]    BIT           NOT NULL,
    [URL]            VARCHAR (200) NOT NULL,
    [Description]    VARCHAR (MAX) NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbAppDirectoryDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]   DATETIME      CONSTRAINT [utbAppDirectoryDefaultCreationDateSysDatetime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]   VARCHAR (100) CONSTRAINT [utbAppDirectoryDefaultCreationUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbAppDirectoryDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbAppDirectoryDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbAppID] PRIMARY KEY CLUSTERED ([AppID] ASC)
);


GO


CREATE TRIGGER [adm].[utrLogAppDirectory] ON [adm].[utbAppDirectory]
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
				,'[adm].[utbAppDirectory]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END