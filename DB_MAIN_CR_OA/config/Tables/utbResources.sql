CREATE TABLE [config].[utbResources] (
    [ResourceID]     INT             IDENTITY (1, 1) NOT NULL,
    [ResourceTypeID] INT             NOT NULL,
    [FileType]       VARCHAR (100)   NOT NULL,
    [FilePath]		 VARCHAR (500)   NULL,
    [FileName]       VARCHAR (500)   NOT NULL,
    [FileURL]        VARCHAR (MAX)   NULL,
    [Description]    VARCHAR (MAX)   NOT NULL,
	[EnableStart]	 DATETIME		 NULL,
	[EnableEnd]		 DATETIME		 NULL,
    [ActiveFlag]     BIT             CONSTRAINT [utbResourcesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]     DATETIME        CONSTRAINT [utbResourcesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]     VARCHAR (100)   CONSTRAINT [utbResourcesDefaultInsertUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME        CONSTRAINT [utbResourcesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)   CONSTRAINT [utbResourcesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbResourceID] PRIMARY KEY CLUSTERED ([ResourceID] ASC),
    CONSTRAINT [FK.config.utbResourceTypes.config.utbResources.ResourceTypeID] FOREIGN KEY ([ResourceTypeID]) REFERENCES [config].[utbResourceTypes] ([ResourceTypeID])
);


GO


CREATE TRIGGER [config].[utrLogResources] ON [config].[utbResources]
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
				,'[config].[utbResources]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END