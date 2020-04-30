CREATE TABLE [adm].[utbRightsbyRole] (
    [RightID]        INT           IDENTITY (1, 1) NOT NULL,
    [WebID]          INT           NOT NULL,
    [RoleID]         INT           NOT NULL,
    [Read]           VARCHAR (50)  NOT NULL,
    [Write]          VARCHAR (50)  NOT NULL,
    [ActiveFlag]     BIT           CONSTRAINT [utbRightsbyRoleDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]   DATETIME      CONSTRAINT [utbRightsbyRoleDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]   VARCHAR (100) CONSTRAINT [utbRightsbyRoleDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME      CONSTRAINT [utbRightsbyRoleDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100) CONSTRAINT [utbRightsbyRoleDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbRightID] PRIMARY KEY CLUSTERED ([RightID] ASC),
    CONSTRAINT [FK.adm.utbRoles.adm.utbRightsbyRole.RoleID] FOREIGN KEY ([RoleID]) REFERENCES [adm].[utbRoles] ([RoleID]),
    CONSTRAINT [FK.adm.utbWebDirectory.adm.utbRightsbyRole.WebID] FOREIGN KEY ([WebID]) REFERENCES [adm].[utbWebDirectory] ([WebID])
);


GO


CREATE TRIGGER [adm].[utrLogRightsbyRole] ON [adm].[utbRightsbyRole]
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
				,'[adm].[utbRightsbyRole]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted
	END