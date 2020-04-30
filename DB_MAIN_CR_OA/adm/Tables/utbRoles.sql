CREATE TABLE [adm].[utbRoles] (
    [RoleID]          INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]        VARCHAR (100) NOT NULL,
    [RoleDescription] VARCHAR (MAX) NOT NULL,
    [ActiveFlag]      BIT           CONSTRAINT [utbRolesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]    DATETIME      CONSTRAINT [utbRolesDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]    VARCHAR (100) CONSTRAINT [utbRolesDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]  DATETIME      CONSTRAINT [utbRolesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]  VARCHAR (100) CONSTRAINT [utbRolesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbRoleID] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);


GO
CREATE TRIGGER [adm].[utrLogRoles] ON [adm].[utbRoles]
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
				,'[adm].[utbRoles]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END