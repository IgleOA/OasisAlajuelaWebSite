CREATE TABLE [adm].[utbUsers] (
    [UserID]           INT           IDENTITY (1, 1) NOT NULL,
    [RoleID]           INT           NOT NULL,
    [FullName]         VARCHAR (50)  NOT NULL,
    [UserName]         VARCHAR (50)  NOT NULL,
    [Email]            VARCHAR (50)  NOT NULL,
    [PasswordHash]     BINARY (64)   NOT NULL,
    [Subscriber]	   BIT           CONSTRAINT [utbUsersDefaultSubscriberTrue] DEFAULT ((1)) NOT NULL,
	[ActiveFlag]       BIT           CONSTRAINT [utbUsersDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,	
    [LastActivityDate] DATETIME      NULL,
    [CreationDate]     DATETIME      CONSTRAINT [utbUsersDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]     VARCHAR (100) CONSTRAINT [utbUsersDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]   DATETIME      CONSTRAINT [utbUsersDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]   VARCHAR (100) CONSTRAINT [utbUsersDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbUserID] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK.adm.utbRoles.adm.utbUsers.RoleID] FOREIGN KEY ([RoleID]) REFERENCES [adm].[utbRoles] ([RoleID])
);


GO
CREATE TRIGGER [adm].[utrLogUsers] ON [adm].[utbUsers]
FOR INSERT
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT [UserID],[RoleID],[FullName],[UserName],[Email],[Subscriber],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT [UserID],[RoleID],[FullName],[UserName],[Email],[Subscriber],[ActiveFlag],[LastActivityDate],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

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
				,'[adm].[utbUsers]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;