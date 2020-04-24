CREATE TABLE [config].[utbResourcesGroups](
    [ResourceGroupID]	INT           IDENTITY (1,1) NOT NULL,
    [ResourceTypeID]	INT		      NOT NULL,
    [GroupID]			INT			  NOT NULL,
    [ActiveFlag]		BIT           CONSTRAINT [utbResourcesGroupsDefaultActiveFlagIsTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]		DATETIME      CONSTRAINT [utbResourcesGroupsDefaultInsertDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100) CONSTRAINT [utbResourcesGroupsDefaultInsertUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME      CONSTRAINT [utbResourcesGroupsDefaultLastModifyDatesysdatetime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100) CONSTRAINT [utbResourcesGroupsDefaultLastModifyUsersuser_sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbResourceGroupID] PRIMARY KEY CLUSTERED ([ResourceGroupID] ASC),
	CONSTRAINT [FK.config.utbResourceTypes.config.utbResourcesGroups.ResourceTypeID] FOREIGN KEY ([ResourceTypeID]) REFERENCES [config].[utbResourceTypes] ([ResourceTypeID]),
	CONSTRAINT [FK.config.utbGroups.config.utbResourcesGroups.GroupID] FOREIGN KEY ([GroupID]) REFERENCES [config].[utbGroups] ([GroupID])
);


GO
CREATE TRIGGER [config].[utrLogResourcesGroups] ON [config].[utbResourcesGroups]
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
				,'[config].[utbResourcesGroups]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted 
	END
